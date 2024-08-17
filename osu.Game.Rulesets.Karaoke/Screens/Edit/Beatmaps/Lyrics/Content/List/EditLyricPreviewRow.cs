// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.List;

public partial class EditLyricPreviewRow : PreviewRow
{
    private const int min_height = 75;

    public EditLyricPreviewRow(Lyric lyric)
        : base(lyric)
    {
    }

    protected override Drawable CreateLyricInfo(Lyric lyric)
    {
        return new InfoControl(lyric)
        {
            // todo : cannot use relative size to both because it will cause size cannot roll-back if make lyric smaller.
            RelativeSizeAxes = Axes.X,
            Height = min_height,
        };
    }

    protected override Drawable CreateContent(Lyric lyric)
    {
        return new InteractableLyric(lyric)
        {
            Margin = new MarginPadding { Left = 10 },
            RelativeSizeAxes = Axes.X,
            TextSizeChanged = (self, size) =>
            {
                self.Height = size.Y;
            },
            Loaders = new LayerLoader[]
            {
                new LayerLoader<LyricLayer>(),
                new LayerLoader<EditLyricLayer>(),
                new LayerLoader<TimeTagLayer>(),
                new LayerLoader<CaretLayer>(),
                new LayerLoader<BlueprintLayer>(),
            },
        };
    }
}
