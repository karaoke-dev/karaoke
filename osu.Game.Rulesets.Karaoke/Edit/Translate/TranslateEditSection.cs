// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Extensions;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Translate.Components;
using osu.Game.Rulesets.Karaoke.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
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

        public TranslateEditSection(EditorBeatmap editorBeatmap)
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
                                languageDropdown = new LanguageDropdown
                                {
                                    RelativeSizeAxes = Axes.X,
                                },
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
                            new GridContainer
                            {
                                Name = "Translates",
                                RowDimensions = createRowDimension(),
                                ColumnDimensions = columnDimensions,
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Content = createContent(editorBeatmap, languageDropdown.Current)
                            }
                        }
                    }
                },
            };

            Dimension[] createRowDimension() => editorBeatmap.HitObjects.OfType<Lyric>()
                                                             .Select(x => new Dimension(GridSizeMode.Absolute, row_height))
                                                             .ToArray();
        }

        [BackgroundDependencyLoader]
        private void load(TranslateManager translateManager, OsuColour colours)
        {
            languageDropdown.ItemSource = translateManager?.Languages ?? new BindableList<BeatmapSetOnlineLanguage>();

            timeSectionBackground.Colour = colours.ContextMenuGray;
            lyricSectionBackground.Colour = colours.Gray9;
        }

        private Drawable[][] createContent(EditorBeatmap editorBeatmap, Bindable<BeatmapSetOnlineLanguage> bindable)
        {
            var lyrics = editorBeatmap.HitObjects.OfType<Lyric>().ToArray();

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
            var startTime = lyric.StartTime.ToEditorFormattedString();
            var endTime = lyric.EndTime.ToEditorFormattedString();
            return new OsuSpriteText
            {
                Text = startTime + " - " + endTime,
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

        private Drawable createTranslateTextBox(Lyric lyric, Bindable<BeatmapSetOnlineLanguage> bindable)
        {
            var textBox = new OsuTextBox
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                RelativeSizeAxes = Axes.X,
            };
            languageDropdown.Current.BindValueChanged(v => { textBox.Text = lyric.Translates.TryGetValue(v.NewValue.Id, out string translate) ? translate : null; });
            textBox.Current.BindValueChanged(textBoxValue =>
            {
                var translateText = textBoxValue.NewValue;
                var languageId = languageDropdown.Current.Value.Id;

                if (!lyric.Translates.TryAdd(languageId, translateText))
                    lyric.Translates[languageId] = translateText;
            });
            return textBox;
        }
    }
}
