// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

public partial class GridLayer : Layer
{
    private readonly RectangularPositionSnapGrid rectangularPositionSnapGrid;

    public GridLayer(Lyric lyric)
        : base(lyric)
    {
        InternalChild = rectangularPositionSnapGrid = new RectangularPositionSnapGrid
        {
            RelativeSizeAxes = Axes.Both,
        };
    }

    public int Spacing
    {
        get => rectangularPositionSnapGrid.Spacing;
        set => rectangularPositionSnapGrid.Spacing = value;
    }

    public override void UpdateDisableEditState(bool editable)
    {
        this.FadeTo(editable ? 1 : 0.5f, 100);
    }

    private partial class RectangularPositionSnapGrid : LinedPositionSnapGrid
    {
        protected override void CreateContent()
        {
            GenerateGridLines(new Vector2(0, -Spacing), DrawSize);
            GenerateGridLines(new Vector2(0, Spacing), DrawSize);

            GenerateGridLines(new Vector2(-Spacing, 0), DrawSize);
            GenerateGridLines(new Vector2(Spacing, 0), DrawSize);

            GenerateOutline(DrawSize);
        }

        public override Vector2 GetSnappedPosition(Vector2 original)
        {
            return StartPosition.Value + original;
        }

        private int spacing;

        public int Spacing
        {
            get => spacing;
            set
            {
                spacing = value;
                GridCache.Invalidate();
            }
        }
    }
}
