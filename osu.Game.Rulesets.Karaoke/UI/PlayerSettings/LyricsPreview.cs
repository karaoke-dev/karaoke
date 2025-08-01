// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
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
using osu.Game.Rulesets.Mods;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings;

public partial class LyricsPreview : CompositeDrawable
{
    private readonly Bindable<Lyric[]> singingLyrics = new();

    [Resolved]
    private IBindable<WorkingBeatmap> beatmap { get; set; } = null!;

    [Resolved]
    private ILyricNavigator lyricNavigator { get; set; } = null!;

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
                Spacing = new Vector2(15),
            },
        };

        singingLyrics.BindValueChanged(value =>
        {
            var oldValue = value.OldValue;
            if (oldValue != null)
                lyricTable.Where(x => oldValue.Contains(x.Lyric)).ForEach(x => { x.Selected = false; });

            var newValue = value.NewValue;
            if (newValue != null)
                lyricTable.Where(x => newValue.Contains(x.Lyric)).ForEach(x => { x.Selected = true; });
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
        lyricNavigator.SeekTimeByLyric(lyric);

        // because playback might not clear singing lyrics, so we should re-assign the lyric here.
        // todo: find a better place.
        singingLyrics.Value = new[] { lyric };
    }

    [BackgroundDependencyLoader]
    private void load(IBindable<IReadOnlyList<Mod>> mods, KaraokeSessionStatics session)
    {
        session.BindWith(KaraokeRulesetSession.SingingLyrics, singingLyrics);
    }

    private partial class ClickableLyric : ClickableContainer
    {
        private const float fade_duration = 100;

        private Color4 hoverTextColour;
        private Color4 idolTextColour;

        private readonly Box background;
        private readonly Drawable icon;
        private readonly DrawableLyricSpriteText drawableLyric;

        public readonly Lyric Lyric;

        public ClickableLyric(Lyric lyric)
        {
            Lyric = lyric;

            AutoSizeAxes = Axes.Y;
            RelativeSizeAxes = Axes.X;
            Masking = true;
            CornerRadius = 5;
            Children = new[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                },
                icon = new SpriteIcon
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Size = new Vector2(15),
                    Icon = FontAwesome.Solid.Play,
                    Margin = new MarginPadding { Left = 5 },
                    Alpha = 0,
                },
                drawableLyric = new DrawableLyricSpriteText(lyric)
                {
                    Font = new FontUsage(size: 25),
                    TopTextFont = new FontUsage(size: 10),
                    BottomTextFont = new FontUsage(size: 10),
                    Margin = new MarginPadding { Left = 25 },
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
                drawableLyric.FadeColour(Selected ? hoverTextColour : idolTextColour, fade_duration);
            }
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            hoverTextColour = colours.Yellow;
            idolTextColour = colours.Gray9;

            drawableLyric.Colour = idolTextColour;
            background.Colour = colours.Blue;
            icon.Colour = hoverTextColour;
        }
    }
}
