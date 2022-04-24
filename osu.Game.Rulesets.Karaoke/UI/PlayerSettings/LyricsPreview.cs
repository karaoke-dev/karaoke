// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings
{
    public class LyricsPreview : CompositeDrawable
    {
        private readonly Bindable<double> bindablePreemptTime = new();
        private readonly Bindable<Lyric[]> singingLyrics = new();

        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; }

        public LyricsPreview()
        {
            FillFlowContainer<ClickableLyric> lyricTable;

            InternalChild = new OsuScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = lyricTable = new FillFlowContainer<ClickableLyric>
                {
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(15)
                }
            };

            singingLyrics.BindValueChanged(value =>
            {
                var oldValue = value.OldValue;
                if (oldValue != null)
                    lyricTable.Where(x => oldValue.Contains(x.HitObject)).ForEach(x => { x.Selected = false; });

                var newValue = value.NewValue;
                if (newValue != null)
                    lyricTable.Where(x => newValue.Contains(x.HitObject)).ForEach(x => { x.Selected = true; });
            });

            Schedule(() =>
            {
                var lyrics = beatmap.Value.Beatmap.HitObjects.OfType<Lyric>().ToList();
                lyricTable.Children = lyrics.Select(x => createLyricContainer(x).With(c =>
                {
                    c.Selected = false;
                    c.Action = () => triggerLyric(x);
                })).ToList();
            });
        }

        private ClickableLyric createLyricContainer(Lyric lyric) => new(lyric);

        private void triggerLyric(Lyric lyric)
        {
            double time = lyric.LyricStartTime - bindablePreemptTime.Value;
            beatmap.Value.Track.Seek(time);

            // because playback might not clear singing lyrics, so we should re-assign the lyric here.
            // todo: find a better place.
            singingLyrics.Value = new[] { lyric };
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetConfigManager config, KaraokeSessionStatics session)
        {
            config.BindWith(KaraokeRulesetSetting.PracticePreemptTime, bindablePreemptTime);
            session.BindWith(KaraokeRulesetSession.SingingLyrics, singingLyrics);
        }

        private class ClickableLyric : ClickableContainer
        {
            private const float fade_duration = 100;

            private Color4 hoverTextColour;
            private Color4 idolTextColour;

            private readonly Box background;
            private readonly Drawable icon;
            private readonly PreviewLyricSpriteText previewLyric;

            public ClickableLyric(Lyric lyric)
            {
                AutoSizeAxes = Axes.Y;
                RelativeSizeAxes = Axes.X;
                Masking = true;
                CornerRadius = 5;
                Children = new[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                    icon = new SpriteIcon
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Size = new Vector2(15),
                        Icon = FontAwesome.Solid.Play,
                        Margin = new MarginPadding { Left = 5 }
                    },
                    previewLyric = new PreviewLyricSpriteText(lyric)
                    {
                        Font = new FontUsage(size: 25),
                        RubyFont = new FontUsage(size: 10),
                        RomajiFont = new FontUsage(size: 10),
                        Margin = new MarginPadding { Left = 25 }
                    },
                };
            }

            private bool selected;

            public bool Selected
            {
                get => selected;
                set
                {
                    if (value == selected) return;

                    selected = value;

                    background.FadeTo(Selected ? 1 : 0, fade_duration);
                    icon.FadeTo(Selected ? 1 : 0, fade_duration);
                    previewLyric.FadeColour(Selected ? hoverTextColour : idolTextColour, fade_duration);
                }
            }

            public Lyric HitObject => previewLyric.HitObject;

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                hoverTextColour = colours.Yellow;
                idolTextColour = colours.Gray9;

                previewLyric.Colour = idolTextColour;
                background.Colour = colours.Blue;
                background.Alpha = 0;
                icon.Colour = hoverTextColour;
                icon.Alpha = 0;
            }
        }
    }
}
