// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Translate.Components;
using osu.Game.Rulesets.Karaoke.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate
{
    public class TranslateScreen : EditorScreen
    {
        public const int DROPDOWN_SPACING = 40;
        public const int TEXT_HEIGHT = 35;
        public const int COLUMN_SPACING = 10;
        public const int SPACING = 5;

        private readonly Box background;
        private readonly TimePreview timePreview;
        private readonly LyricPreview lyricPreview;
        private readonly TranslateEditor translateEditor;

        private LyricLine[] lyricLines => EditorBeatmap.HitObjects.OfType<LyricLine>().ToArray();

        public TranslateScreen()
           : base(EditorScreenMode.SongSetup)
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
            
            translateEditor.LanguageDropdown.Items = new[]
            {
                new BeatmapSetOnlineLanguage
                {
                    Id = 1,
                    Name = "zh-TW"
                },
                new BeatmapSetOnlineLanguage
                {
                    Id = 2,
                    Name = "en-US"
                },
                new BeatmapSetOnlineLanguage
                {
                    Id = 3,
                    Name = "ja-JP"
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.ContextMenuGray;

            timePreview.LyricLines = lyricLines;
            lyricPreview.LyricLines = lyricLines;
            translateEditor.LyricLines = lyricLines;
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
                            Y = TEXT_HEIGHT / 2 - SPACING
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
            private readonly FillFlowContainer<PreviewLyricSpriteText> previewSpriteTexts;

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
                        previewSpriteTexts = new FillFlowContainer<PreviewLyricSpriteText>
                        {
                            Direction = FillDirection.Vertical,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Spacing = new Vector2(SPACING),
                            Margin = new MarginPadding { Left = SPACING, Right = SPACING },
                            Y = TEXT_HEIGHT / 2 - SPACING
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
                    previewSpriteTexts.Children = LyricLines.Select(x => new PreviewLyricSpriteText(x)
                    {
                        RelativeSizeAxes = Axes.X,
                        Font = new FontUsage(size: 25),
                        RubyFont = new FontUsage(size: 10),
                        RomajiFont = new FontUsage(size: 10),
                        Height = TEXT_HEIGHT,
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
            public LanguageDropdown LanguageDropdown { get; }

            private readonly FillFlowContainer<OsuTextBox> translateTextBoxes;

            public TranslateEditor()
            {
                AutoSizeAxes = Axes.Y;
                Spacing = new Vector2(COLUMN_SPACING);
                Children = new Drawable[]
                {
                    LanguageDropdown = new LanguageDropdown
                    {
                        RelativeSizeAxes = Axes.X,
                    },
                    translateTextBoxes = new FillFlowContainer<OsuTextBox>
                    {
                        Direction = FillDirection.Vertical,
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Spacing = new Vector2(SPACING)
                    }
                };

                LanguageDropdown.Current.BindValueChanged(value =>
                {
                    // Reload to switch new laugnage.
                    LyricLines = LyricLines;
                }, true);
            }

            private LyricLine[] lyricLines;

            public LyricLine[] LyricLines
            {
                get => lyricLines;
                set
                {
                    lyricLines = value;

                    if (lyricLines == null)
                        return;

                    translateTextBoxes.Children = LyricLines?.Select(x =>
                    {
                        if (LanguageDropdown.Current.Value == null)
                            return new OsuTextBox();

                        var languageId = LanguageDropdown.Current.Value.Id;

                        var textBox = new OsuTextBox
                        {
                            Text = x.Translates.TryGetValue(languageId, out string translate) ? translate : null,
                            RelativeSizeAxes = Axes.X,
                            Height = TEXT_HEIGHT
                        };
                        textBox.Current.BindValueChanged(textBoxValue =>
                        {
                            var index = translateTextBoxes.Children.IndexOf(textBox);
                            var translateText = textBoxValue.NewValue;

                            if (!LyricLines[index].Translates.TryAdd(languageId, translateText))
                                LyricLines[index].Translates[languageId] = translateText;
                        });
                        return textBox;
                    }).ToList();
                }
            }
        }
    }
}
