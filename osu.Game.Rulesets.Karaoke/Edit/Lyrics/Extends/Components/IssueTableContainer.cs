// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components
{
    public abstract class IssueTableContainer : TableContainer
    {
        protected const float ROW_HEIGHT = 25;

        public const int TEXT_SIZE = 14;

        protected readonly FillFlowContainer<RowBackground> BackgroundFlow;

        protected IssueTableContainer()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            RowSize = new Dimension(GridSizeMode.Absolute, ROW_HEIGHT);
            Columns = CreateHeaders();

            AddInternal(BackgroundFlow = new FillFlowContainer<RowBackground>
            {
                RelativeSizeAxes = Axes.Both,
                Depth = 1f,
                Margin = new MarginPadding { Top = ROW_HEIGHT }
            });
        }

        protected abstract TableColumn[] CreateHeaders();

        protected override Drawable CreateHeader(int index, TableColumn column) => new HeaderText(column?.Header ?? string.Empty);

        public class HeaderText : OsuSpriteText
        {
            public HeaderText(string text)
            {
                Text = text.ToUpper();
                Font = OsuFont.GetFont(size: 12, weight: FontWeight.Bold);
            }
        }

        public class RowBackground : OsuClickableContainer
        {
            private const int fade_duration = 100;

            private readonly Box hoveredBackground;

            public RowBackground()
            {
                RelativeSizeAxes = Axes.X;
                Height = 25;

                AlwaysPresent = true;

                CornerRadius = 3;
                Masking = true;

                Children = new Drawable[]
                {
                    hoveredBackground = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                    },
                };
            }

            private Color4 colourHover;
            private Color4 colourSelected;

            [BackgroundDependencyLoader]
            private void load(LyricEditorColourProvider colourProvider, ILyricEditorState state)
            {
                hoveredBackground.Colour = colourHover = colourProvider.Background1(LyricEditorMode.CreateTimeTag);
                colourSelected = colourProvider.Colour3(LyricEditorMode.CreateTimeTag);
            }

            protected override bool OnHover(HoverEvent e)
            {
                UpdateState(selected);
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                UpdateState(selected);
                base.OnHoverLost(e);
            }

            private bool selected;

            protected void UpdateState(bool selected)
            {
                this.selected = selected;
                hoveredBackground.FadeColour(selected ? colourSelected : colourHover, 450, Easing.OutQuint);

                if (selected || IsHovered)
                    hoveredBackground.FadeIn(fade_duration, Easing.OutQuint);
                else
                    hoveredBackground.FadeOut(fade_duration, Easing.OutQuint);
            }
        }
    }
}
