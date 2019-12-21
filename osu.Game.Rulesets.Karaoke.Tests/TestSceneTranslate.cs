// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Tests.Visual;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Tests
{
    [TestFixture]
    public class TestSceneTranslate : OsuTestScene
    {
        public const int DROPDOWN_SPACING = 40;
        public const int TEXT_HEIGHT = 35;
        public const int COLUMN_SPACING = 10;
        public const int SPACING = 5;

        private readonly Box background;
        private readonly TimePreview timePreview;
        private readonly LyricPreview lyricPreview;
        private readonly TranslateEditor translateEditor;

        private readonly IBeatmap beatmap;
        private TranslateDictionary translateDictionary => beatmap.HitObjects.OfType<TranslateDictionary>().FirstOrDefault();
        private LyricLine[] lyricLines => beatmap.HitObjects.OfType<LyricLine>().ToArray();

        protected override IBeatmap CreateBeatmap(RulesetInfo ruleset) => new TestKaraokeBeatmap(ruleset);

        public TestSceneTranslate()
        {
            Child = new OsuScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = new Container
                {
                    Masking = true,
                    CornerRadius = 5,
                    AutoSizeAxes = Axes.Both,
                    Margin = new MarginPadding(30),
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            Name = "Background",
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                        },
                        new FillFlowContainer
                        {
                            Margin = new MarginPadding(SPACING),
                            Direction = FillDirection.Horizontal,
                            AutoSizeAxes = Axes.Both,
                            Spacing = new Vector2(COLUMN_SPACING),
                            Children = new Drawable[]
                            {
                                timePreview = new TimePreview
                                {
                                    Width = 200
                                },
                                lyricPreview = new LyricPreview
                                {
                                    Width = 400
                                },
                                translateEditor = new TranslateEditor
                                {
                                    Width = 400
                                },
                            }
                        }
                    }
                }
            };

            beatmap = CreateBeatmap(new KaraokeRuleset().RulesetInfo);
            timePreview.LyricLines = lyricLines;
            lyricPreview.LyricLines = lyricLines;
            translateEditor.LanguageDropdown.Items = new string[]
            {
                "zh-TW",
                "en-US",
                "ja-JP"
            };
            translateEditor.LanguageDropdown.Current.BindValueChanged(value =>
            {
                // Save translate first
                if (translateDictionary?.Translates != null && !string.IsNullOrEmpty(value.OldValue))
                {
                    if (translateDictionary.Translates.ContainsKey(value.OldValue))
                        translateDictionary.Translates[value.OldValue] = translateEditor.Translates.ToList();
                    else
                        translateDictionary.Translates.Add(value.OldValue, translateEditor.Translates.ToList());
                }

                if (translateDictionary?.Translates!=null && translateDictionary.Translates.TryGetValue(value.NewValue, out var translate))
                {
                    translateEditor.Translates = translate.ToArray();
                }
                else
                {
                    translateEditor.Translates = lyricLines.Select(x => "").ToArray();
                }
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.ContextMenuGray;
        }

        public class TimePreview : Container
        {
            private readonly Box background;
            private readonly FillFlowContainer<OsuSpriteText> timeSpriteTexts;

            public TimePreview()
            {
                AutoSizeAxes = Axes.Y;
                Child = new Container
                {
                    Masking = true,
                    CornerRadius = 5,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Margin = new MarginPadding { Top = DROPDOWN_SPACING + COLUMN_SPACING },
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            Name = "Background",
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                        },
                        timeSpriteTexts = new FillFlowContainer<OsuSpriteText>
                        {
                            Direction = FillDirection.Vertical,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Spacing = new Vector2(SPACING),
                            Margin = new MarginPadding { Left = SPACING, Right = SPACING },
                            Y = (TEXT_HEIGHT) / 2 - SPACING
                        }
                    }
                };
            }

            private LyricLine[] lyricLines;

            public LyricLine[] LyricLines
            {
                get => lyricLines;
                set
                {
                    lyricLines = value;
                    timeSpriteTexts.Children = LyricLines.Select(x =>
                    {
                        var startTime = TimeSpan.FromMilliseconds(x.StartTime).ToString(@"mm\:ss\:fff");
                        var endTime = TimeSpan.FromMilliseconds(x.EndTime).ToString(@"mm\:ss\:fff");
                        return new OsuSpriteText
                        {
                            Text = startTime + " - " + endTime,
                            RelativeSizeAxes = Axes.X,
                            Font = OsuFont.GetFont(size: 18, fixedWidth: true),
                            Height = TEXT_HEIGHT,
                            AllowMultiline = false
                        };
                    }).ToList();
                }
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                background.Colour = colours.Gray9;
            }
        }

        public class LyricPreview : Container
        {
            private readonly Box background;
            private readonly FillFlowContainer<OsuSpriteText> priviewSpriteTexts;

            public LyricPreview()
            {
                AutoSizeAxes = Axes.Y;
                Child = new Container
                {
                    Masking = true,
                    CornerRadius = 5,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Margin = new MarginPadding { Top = DROPDOWN_SPACING + COLUMN_SPACING },
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            Name = "Background",
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                        },
                        priviewSpriteTexts = new FillFlowContainer<OsuSpriteText>
                        {
                            Direction = FillDirection.Vertical,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Spacing = new Vector2(SPACING),
                            Margin = new MarginPadding { Left = SPACING, Right = SPACING },
                            Y = (TEXT_HEIGHT) / 2 - SPACING
                        }
                    }
                };
            }

            private LyricLine[] lyricLines;

            public LyricLine[] LyricLines
            {
                get => lyricLines;
                set
                {
                    lyricLines = value;
                    priviewSpriteTexts.Children = LyricLines.Select(x => new OsuSpriteText
                    {
                        Text = x.Text,
                        RelativeSizeAxes = Axes.X,
                        Font = OsuFont.GetFont(size: 18),
                        Height = TEXT_HEIGHT
                    }).ToList();
                }
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                background.Colour = colours.Gray9;
            }
        }

        public class TranslateEditor : FillFlowContainer
        {
            public OsuDropdown<string> LanguageDropdown { get; private set; }

            private readonly FillFlowContainer<OsuTextBox> translateTextBoxs;

            public TranslateEditor()
            {
                AutoSizeAxes = Axes.Y;
                Spacing = new Vector2(COLUMN_SPACING);
                Children = new Drawable[]
                {
                    LanguageDropdown = new OsuDropdown<string>
                    {
                        RelativeSizeAxes = Axes.X,
                    },
                    translateTextBoxs = new FillFlowContainer<OsuTextBox>
                    {
                        Direction = FillDirection.Vertical,
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Spacing = new Vector2(SPACING)
                    }
                };
            }

            private string[] translates;

            public string[] Translates
            {
                get => translates;
                set
                {
                    translates = value;

                    translateTextBoxs.Children = Translates?.Select(x =>
                    {
                        var textbox = new OsuTextBox
                        {
                            Text = x,
                            RelativeSizeAxes = Axes.X,
                            Height = TEXT_HEIGHT
                        };
                        textbox.Current.BindValueChanged(textBoxValue =>
                        {
                            var index = translateTextBoxs.Children.IndexOf(textbox);
                            Translates[index] = textBoxValue.NewValue;
                        });
                        return textbox;
                    }).ToList();
                }
            }
        }
    }
}
