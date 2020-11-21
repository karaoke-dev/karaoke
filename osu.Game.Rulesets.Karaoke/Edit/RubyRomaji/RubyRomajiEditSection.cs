// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.RubyRomaji.Components;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.RubyRomaji
{
    public class RubyRomajiEditSection : Container
    {
        private const int area_margin = 10;

        private readonly LyricList lyricList;
        private readonly LyricPreviewArea lyricPreviewArea;
        private readonly RubyListPreview rubyListPreview;
        private readonly RomajiListPreview romajiListPreview;

        public RubyRomajiEditSection(EditorBeatmap editorBeatmap)
        {
            Padding = new MarginPadding(10);

            Child = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                ColumnDimensions = new[]
                {
                    new Dimension(GridSizeMode.Relative, 0.4f)
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        lyricList = new LyricList
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding(area_margin)
                        },
                        new GridContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Content = new[]
                            {
                                new Drawable[]
                                {
                                    lyricPreviewArea = new LyricPreviewArea
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Margin = new MarginPadding(area_margin)
                                    }
                                },
                                new Drawable[]
                                {
                                    new GridContainer
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Content = new[]
                                        {
                                            new Drawable[]
                                            {
                                                rubyListPreview = new RubyListPreview
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Padding = new MarginPadding(area_margin)
                                                },
                                                romajiListPreview = new RomajiListPreview
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Padding = new MarginPadding(area_margin)
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            lyricList.Lyrics = getLyrics(editorBeatmap);
            lyricList.BindableLyric.BindValueChanged(value =>
            {
                var newValue = value.NewValue;
                if (newValue == null)
                    return;

                // Apply new lyric line
                lyricPreviewArea.Lyric = newValue;

                // Apply new tag and max position
                var maxLyricPosition = newValue.Text.Length - 1;
                rubyListPreview.Tags = newValue.RubyTags;
                rubyListPreview.MaxTagPosition = maxLyricPosition;
                romajiListPreview.Tags = newValue.RomajiTags;
                romajiListPreview.MaxTagPosition = maxLyricPosition;
            }, true);

            rubyListPreview.BindableTag.BindValueChanged(value => { lyricPreviewArea.Lyric.RubyTags = value.NewValue.ToArray(); });
            romajiListPreview.BindableTag.BindValueChanged(value => { lyricPreviewArea.Lyric.RomajiTags = value.NewValue.ToArray(); });
        }

        private Lyric[] getLyrics(EditorBeatmap editorBeatmap)
            => editorBeatmap.HitObjects.OfType<Lyric>().ToArray();
    }
}
