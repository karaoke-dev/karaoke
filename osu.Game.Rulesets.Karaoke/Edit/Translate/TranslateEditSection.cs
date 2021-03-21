// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Translate.Components;
using osu.Game.Rulesets.Karaoke.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate
{
    public class TranslateEditSection : Container
    {
        private const int row_height = 50;
        private const int column_spacing = 10;
        private const int row_inner_spacing = 10;

        private readonly CornerBackground timeSectionBackground;
        private readonly CornerBackground lyricSectionBackground;
        private readonly LanguageDropdown languageDropdown;
        private readonly GridContainer translateGrid;

        public readonly Bindable<CultureInfo> NewLanguage = new Bindable<CultureInfo>();

        [Resolved]
        private TranslateManager translateManager { get; set; }

        [Resolved]
        protected DialogOverlay DialogOverlay { get; private set; }

        [Resolved]
        protected LanguageSelectionDialog LanguageSelectionDialog { get; private set; }

        public TranslateEditSection()
        {
            Padding = new MarginPadding(10);

            var columnDimensions = new[]
            {
                new Dimension(GridSizeMode.Absolute, 200),
                new Dimension(GridSizeMode.Absolute, column_spacing),
                new Dimension(GridSizeMode.Absolute, 400),
                new Dimension(GridSizeMode.Absolute, column_spacing),
                new Dimension()
            };

            Child = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    new GridContainer
                    {
                        Name = "LanguageSelection",
                        RowDimensions = new[]
                        {
                            new Dimension(GridSizeMode.AutoSize)
                        },
                        ColumnDimensions = columnDimensions,
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Content = new Drawable[][]
                        {
                            new[]
                            {
                                null,
                                null,
                                null,
                                null,
                                new GridContainer
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    ColumnDimensions = new[]
                                    {
                                        new Dimension(GridSizeMode.Distributed),
                                        new Dimension(GridSizeMode.Absolute, column_spacing),
                                        new Dimension(GridSizeMode.Absolute, 50),
                                        new Dimension(GridSizeMode.Absolute, column_spacing),
                                        new Dimension(GridSizeMode.Absolute, 50),
                                    },
                                    RowDimensions = new[]
                                    {
                                        new Dimension(GridSizeMode.AutoSize),
                                    },
                                    Content = new[]
                                    {
                                        new Drawable[]
                                        {
                                            languageDropdown = new LanguageDropdown
                                            {
                                                RelativeSizeAxes = Axes.X,
                                            },
                                            null,
                                            new IconButton
                                            {
                                                Y = 5,
                                                Icon = FontAwesome.Solid.Plus,
                                                Action = () =>
                                                {
                                                    LanguageSelectionDialog.Show();
                                                }
                                            },
                                            null,
                                            new IconButton
                                            {
                                                Y = 5,
                                                Icon = FontAwesome.Solid.Trash,
                                                Action = () =>
                                                {
                                                    var currentLanguage = languageDropdown.Current.Value;

                                                    if (translateManager.LanguageContainsTranslateAmount(currentLanguage) > 0)
                                                    {
                                                        DialogOverlay.Push(new DeleteLanguagePopupDialog(currentLanguage, isOk =>
                                                        {
                                                            if (isOk)
                                                                translateManager.RemoveLanguage(currentLanguage);
                                                        }));
                                                    }
                                                    else
                                                    {
                                                        translateManager.RemoveLanguage(currentLanguage);
                                                    }
                                                }
                                            },
                                        }
                                    }
                                }
                            },
                        }
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Children = new[]
                        {
                            new GridContainer
                            {
                                Name = "Background",
                                RowDimensions = new[]
                                {
                                    new Dimension(GridSizeMode.AutoSize)
                                },
                                ColumnDimensions = columnDimensions,
                                RelativeSizeAxes = Axes.Both,
                                Content = new[]
                                {
                                    new[]
                                    {
                                        new CornerBackground
                                        {
                                            Alpha = 0,
                                        },
                                        null,
                                        null,
                                        null,
                                        null,
                                    },
                                    new[]
                                    {
                                        timeSectionBackground = new CornerBackground
                                        {
                                            RelativeSizeAxes = Axes.Both
                                        },
                                        null,
                                        lyricSectionBackground = new CornerBackground
                                        {
                                            RelativeSizeAxes = Axes.Both
                                        },
                                        null,
                                        null,
                                    },
                                }
                            },
                            translateGrid = new GridContainer
                            {
                                Name = "Translates",
                                ColumnDimensions = columnDimensions,
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                            }
                        }
                    }
                },
            };

            NewLanguage.BindValueChanged(e =>
            {
                translateManager.AddLanguage(e.NewValue);
            });
        }

        [BackgroundDependencyLoader]
        private void load(TranslateManager translateManager, OsuColour colours)
        {
            LanguageSelectionDialog.Current = NewLanguage;

            languageDropdown.ItemSource = translateManager.Languages;

            timeSectionBackground.Colour = colours.ContextMenuGray;
            lyricSectionBackground.Colour = colours.Gray9;

            translateGrid.RowDimensions = translateManager.Lyrics.Select(x => new Dimension(GridSizeMode.Absolute, row_height)).ToArray();
            translateGrid.Content = createContent(languageDropdown.Current);
        }

        protected override void Dispose(bool isDisposing)
        {
            NewLanguage.UnbindAll();
            base.Dispose(isDisposing);
        }

        private Drawable[][] createContent(Bindable<CultureInfo> bindable)
        {
            var lyrics = translateManager.Lyrics;
            return lyrics.Select(x =>
            {
                return new[]
                {
                    createTimeDrawable(x),
                    null,
                    createPreviewSpriteText(x),
                    null,
                    createTranslateTextBox(x, bindable),
                };
            }).ToArray();
        }

        private Drawable createTimeDrawable(Lyric lyric)
        {
            return new OsuSpriteText
            {
                Text = LyricUtils.LyricTimeFormattedString(lyric),
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Margin = new MarginPadding { Left = row_inner_spacing },
                Font = OsuFont.GetFont(size: 18, fixedWidth: true),
                AllowMultiline = false
            };
        }

        private Drawable createPreviewSpriteText(Lyric lyric)
        {
            return new PreviewLyricSpriteText(lyric)
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Margin = new MarginPadding { Left = row_inner_spacing },
                Font = new FontUsage(size: 25),
                RubyFont = new FontUsage(size: 10),
                RomajiFont = new FontUsage(size: 10),
            };
        }

        private Drawable createTranslateTextBox(Lyric lyric, Bindable<CultureInfo> bindable)
        {
            var textBox = new OsuTextBox
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.X,
            };
            languageDropdown.Current.BindValueChanged(v =>
            {
                var hasCultureInfo = v.NewValue != null;

                // disable and clear textbox if contains no language in language list.
                textBox.Text = hasCultureInfo ? translateManager.GetTranslate(lyric, v.NewValue) : null;
                ScheduleAfterChildren(() =>
                {
                    textBox.Current.Disabled = !hasCultureInfo;
                });
            }, true);
            textBox.Current.BindValueChanged(textBoxValue =>
            {
                var cultureInfo = languageDropdown.Current.Value;
                if (cultureInfo == null)
                    return;

                var translateText = textBoxValue.NewValue;
                translateManager.SaveTranslate(lyric, cultureInfo, translateText);
            });
            return textBox;
        }
    }
}
