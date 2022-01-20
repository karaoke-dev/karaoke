// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Languages;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Translate.Components;
using osu.Game.Rulesets.Karaoke.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

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

        public readonly Bindable<CultureInfo> NewLanguage = new();

        [Cached(typeof(IBindable<CultureInfo>))]
        private readonly IBindable<CultureInfo> currentLanguage = new Bindable<CultureInfo>();

        [Resolved]
        private ILanguagesChangeHandler languagesChangeHandler { get; set; }

        [Resolved]
        private ITranslateInfoProvider translateInfoProvider { get; set; }

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
                        Content = new[]
                        {
                            new Drawable[]
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
                                        new Dimension(),
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
                                                    if (languagesChangeHandler.IsLanguageContainsTranslate(currentLanguage.Value))
                                                    {
                                                        DialogOverlay.Push(new DeleteLanguagePopupDialog(currentLanguage.Value, isOk =>
                                                        {
                                                            if (isOk)
                                                                languagesChangeHandler.Remove(currentLanguage.Value);
                                                        }));
                                                    }
                                                    else
                                                    {
                                                        languagesChangeHandler.Remove(currentLanguage.Value);
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

            currentLanguage.BindTo(languageDropdown.Current);

            NewLanguage.BindValueChanged(e =>
            {
                languagesChangeHandler.Add(e.NewValue);
            });
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            LanguageSelectionDialog.Current = NewLanguage;

            languageDropdown.ItemSource = languagesChangeHandler.Languages;

            timeSectionBackground.Colour = colours.ContextMenuGray;
            lyricSectionBackground.Colour = colours.Gray9;

            translateGrid.RowDimensions = translateInfoProvider.TranslatableLyrics.Select(_ => new Dimension(GridSizeMode.Absolute, row_height)).ToArray();
            translateGrid.Content = createContent();
        }

        protected override void Dispose(bool isDisposing)
        {
            NewLanguage.UnbindAll();
            base.Dispose(isDisposing);
        }

        private Drawable[][] createContent()
        {
            var lyrics = translateInfoProvider.TranslatableLyrics;
            return lyrics.Select(x =>
            {
                return new[]
                {
                    createTimeDrawable(x),
                    null,
                    createPreviewSpriteText(x),
                    null,
                    createTranslateTextBox(x),
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

        private Drawable createTranslateTextBox(Lyric lyric) =>
            new LyricTranslateTextBox(lyric)
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.X,
                TabbableContentContainer = this,
                CommitOnFocusLost = true,
            };

        private class LyricTranslateTextBox : OsuTextBox
        {
            [Resolved]
            private EditorBeatmap beatmap { get; set; }

            [Resolved]
            private ILyricTranslateChangeHandler lyricTranslateChangeHandler { get; set; }

            [Resolved]
            private ITranslateInfoProvider translateInfoProvider { get; set; }

            private readonly IBindable<CultureInfo> currentLanguage = new Bindable<CultureInfo>();

            private readonly Lyric lyric;

            public LyricTranslateTextBox(Lyric lyric)
            {
                this.lyric = lyric;

                currentLanguage.BindValueChanged(v =>
                {
                    bool hasCultureInfo = v.NewValue != null;

                    // disable and clear text box if contains no language in language list.
                    Text = hasCultureInfo ? translateInfoProvider.GetLyricTranslate(lyric, v.NewValue) : null;
                    ScheduleAfterChildren(() =>
                    {
                        Current.Disabled = !hasCultureInfo;
                    });
                }, true);

                OnCommit += (t, _) =>
                {
                    string text = t.Text.Trim();

                    var cultureInfo = currentLanguage.Value;
                    if (cultureInfo == null)
                        return;

                    lyricTranslateChangeHandler.UpdateTranslate(cultureInfo, text);
                };
            }

            [BackgroundDependencyLoader]
            private void load(IBindable<CultureInfo> currentLanguage)
            {
                this.currentLanguage.BindTo(currentLanguage);
            }

            protected override void OnFocus(FocusEvent e)
            {
                base.OnFocus(e);
                beatmap.SelectedHitObjects.Add(lyric);
            }

            protected override void OnFocusLost(FocusLostEvent e)
            {
                base.OnFocusLost(e);
                Schedule(() =>
                {
                    // should remove lyric until commit finished.
                    beatmap.SelectedHitObjects.Remove(lyric);
                });
            }
        }
    }
}
