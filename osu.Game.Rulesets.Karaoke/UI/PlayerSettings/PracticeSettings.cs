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
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Screens.Play.PlayerSettings;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI.PlayerSettings
{
    public class PracticeSettings : PlayerSettingsGroup, IKeyBindingHandler<KaraokeAction>
    {
        protected override string Title => "Practice";

        private readonly PlayerSliderBar<double> preemptTimeSliderBar;
        private readonly LyricPreview lyricPreview;

        public PracticeSettings(IBeatmap beatmap)
        {
            var lyrics = beatmap.HitObjects.OfType<LyricLine>().ToList();

            Children = new Drawable[]
            {
                new OsuSpriteText
                {
                    Text = "Practice preempt time:"
                },
                preemptTimeSliderBar = new PlayerSliderBar<double>(),
                new OsuSpriteText
                {
                    Text = "Lyric:"
                },
                lyricPreview = new LyricPreview(lyrics)
                {
                    Height = 580
                }
            };
        }

        public bool OnPressed(KaraokeAction action)
        {
            switch (action)
            {
                case KaraokeAction.FirstLyric:
                    // TODO : switch to first lyric
                    break;

                case KaraokeAction.PreviousLyric:
                    // TODO : switch to pervious lyric
                    break;

                case KaraokeAction.NextLyric:
                    // TODO : switch to next lyric
                    break;

                case KaraokeAction.PlayAndPause:
                    // TODO : pause
                    break;

                default:
                    return false;
            }

            return true;
        }

        public void OnReleased(KaraokeAction action)
        {
        }

        [BackgroundDependencyLoader]
        private void load(KaraokeRulesetConfigManager config)
        {
            preemptTimeSliderBar.Bindable = config.GetBindable<double>(KaraokeRulesetSetting.PracticePreemptTime);
        }

        internal class LyricPreview : Container
        {
            private readonly Bindable<LyricLine> selectedLyricLine = new Bindable<LyricLine>();
            private readonly FillFlowContainer<ClickableLyric> lyricTable;

            public LyricPreview(List<LyricLine> lyrics)
            {
                RelativeSizeAxes = Axes.X;
                Child = new OsuScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = lyricTable = new FillFlowContainer<ClickableLyric>
                    {
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(15),
                        Children = lyrics.Select(x => new ClickableLyric(x)
                        {
                            Selected = false,
                            Action = () => triggerLyricLine(x)
                        }).ToList()
                    }
                };

                selectedLyricLine.BindValueChanged(value =>
                {
                    var oldValue = value.OldValue;
                    if (oldValue != null)
                        lyricTable.Where(x => x.HitObject == oldValue).ForEach(x =>
                        {
                            x.Selected = false;
                        });

                    var newValue = value.NewValue;
                    if (newValue != null)
                        lyricTable.Where(x => x.HitObject == newValue).ForEach(x =>
                        {
                            x.Selected = true;
                        });
                });
            }

            [BackgroundDependencyLoader]
            private void load(KaroakeSessionStatics session)
            {
                session.BindWith(KaraokeRulesetSession.NowLyric, selectedLyricLine);
            }

            private void triggerLyricLine(LyricLine lyric)
            {
                if (selectedLyricLine.Value == lyric)
                    selectedLyricLine.TriggerChange();
                else
                    selectedLyricLine.Value = lyric;
            }

            internal class ClickableLyric : ClickableContainer
            {
                private const float fade_duration = 100;

                private Color4 hoverTextColour;
                private Color4 idolTextColour;

                private readonly Box background;
                private readonly SpriteIcon icon;
                private readonly PreviewLyricSpriteText previewLyric;

                public ClickableLyric(LyricLine lyric)
                {
                    AutoSizeAxes = Axes.Y;
                    RelativeSizeAxes = Axes.X;
                    Masking = true;
                    CornerRadius = 5;
                    Children = new Drawable[]
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
                            Margin = new MarginPadding{ Left = 25 }
                        }
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

                public LyricLine HitObject => previewLyric.HitObject;

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
}
