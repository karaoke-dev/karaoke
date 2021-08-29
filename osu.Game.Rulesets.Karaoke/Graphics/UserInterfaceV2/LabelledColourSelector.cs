// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterfaceV2;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2
{
    // refactor this shit
    public class LabelledColourSelector : LabelledComponent<LabelledColourSelector.ColourSelectorDisplay, Colour4>
    {
        public LabelledColourSelector()
            : base(true)
        {
        }

        protected override ColourSelectorDisplay CreateComponent()
            => new();

        public class ColourSelectorDisplay : CompositeDrawable, IHasCurrentValue<Colour4>, IHasPopover
        {
            private readonly BindableWithCurrent<Colour4> current = new();

            private Box fill;
            private OsuSpriteText colourHexCode;

            public Bindable<Colour4> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colour)
            {
                AutoSizeAxes = Axes.Y;
                RelativeSizeAxes = Axes.X;

                InternalChild = new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(0, 10),
                    Children = new Drawable[]
                    {
                        new OsuClickableContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 60,
                            CornerRadius = 10,
                            Masking = true,
                            BorderThickness = 2f,
                            BorderColour = colour.Gray5,
                            Children = new Drawable[]
                            {
                                fill = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                },
                                colourHexCode = new OsuSpriteText
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Font = OsuFont.Default.With(size: 12)
                                }
                            },
                            Action = this.ShowPopover
                        },
                    }
                };
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                current.BindValueChanged(_ => updateColour(), true);
            }

            private void updateColour()
            {
                fill.Colour = current.Value;
                colourHexCode.Text = current.Value.ToHex();
                colourHexCode.Colour = OsuColour.ForegroundTextColourFor(current.Value);
            }

            public Popover GetPopover() => new OsuPopover(false)
            {
                Child = new OsuColourPicker
                {
                    Current = { BindTarget = Current }
                }
            };
        }
    }
}
