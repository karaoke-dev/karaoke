// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Edit
{
    public class EditSingerDialog : PopupFocusedOverlayContainer, IHasCurrentValue<Singer>
    {
        private const float section_scale = 0.75f;

        [Cached]
        private readonly BindableWithCurrent<Singer> current = new BindableWithCurrent<Singer>();

        public Bindable<Singer> Current
        {
            get => current.Current;
            set => current.Current = value;
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            AutoSizeAxes = Axes.Both;

            // todo : also has apply, cancel and reset button.
            Child = new Container
            {
                Name = "Layout adjustment area",
                Width = 400,
                Height = 600,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Masking = true,
                CornerRadius = 10,
                Children = new Drawable[]
                {
                    new Box
                    {
                        Colour = colourProvider.Background2,
                        RelativeSizeAxes = Axes.Both,
                    },
                    new SectionsContainer<EditSingerSection>
                    {
                        FixedHeader = new EditSingerScreenHeader(),
                        RelativeSizeAxes = Axes.Both,
                        Scale = new Vector2(section_scale),
                        Size = new Vector2(1 / section_scale),
                        Children = new EditSingerSection[]
                        {
                            new AvatarSection(),
                            new MetadataSection(),
                        }
                    }
                }
            };
        }

        internal class EditSingerScreenHeader : OverlayHeader
        {
            protected override OverlayTitle CreateTitle() => new LayoutScreenTitle();

            private class LayoutScreenTitle : OverlayTitle
            {
                public LayoutScreenTitle()
                {
                    Title = "edit singer";
                    Description = "edit singer of your beatmap";
                    IconTexture = "Icons/Hexacons/social";
                }
            }
        }
    }
}
