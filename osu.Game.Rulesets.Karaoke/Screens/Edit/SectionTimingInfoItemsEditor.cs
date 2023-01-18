// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Components.UserInterface;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

public abstract partial class SectionTimingInfoItemsEditor<TItem> : SectionItemsEditor<TItem> where TItem : class
{
    protected sealed override Drawable CreateDrawable(TItem item)
        => CreateTimingInfoDrawable(item);

    protected abstract DrawableTimingInfoItem CreateTimingInfoDrawable(TItem item);

    protected abstract partial class DrawableTimingInfoItem : CompositeDrawable
    {
        [Resolved, AllowNull]
        private ISectionItemsEditorProvider sectionItemsEditorProvider { get; set; }

        public readonly TItem Item;

        private readonly Box background;
        private readonly OsuSpriteText spriteText;
        private readonly DeleteIconButton deleteIconButton;

        protected DrawableTimingInfoItem(TItem item)
        {
            Item = item;

            Masking = true;
            CornerRadius = 5;
            RelativeSizeAxes = Axes.X;
            Height = 28;
            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                spriteText = new OsuSpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Padding = new MarginPadding
                    {
                        Horizontal = 5,
                    },
                },
                deleteIconButton = new DeleteIconButton
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    X = -5,
                    Size = new Vector2(20),
                    Action = () =>
                    {
                        RemoveItem(item);
                    },
                }
            };
        }

        protected abstract void RemoveItem(TItem item);

        protected string Text
        {
            set => spriteText.Text = value;
        }

        protected void ChangeDisplayOrder(int order)
        {
            Schedule(() =>
            {
                sectionItemsEditorProvider.UpdateDisplayOrder(this, order);
            });
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colour)
        {
            background.Colour = colour.YellowLight;
            spriteText.Colour = colour.YellowDarker;
            deleteIconButton.IconColour = colour.YellowDarker;
        }
    }
}
