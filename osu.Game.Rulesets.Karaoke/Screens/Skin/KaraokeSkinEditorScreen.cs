// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Screens.Edit;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin
{
    public abstract class KaraokeSkinEditorScreen : GenericEditorScreen<KaraokeSkinEditorScreenMode>
    {
        private const float section_scale = 0.75f;
        private const float left_column_width = 200;
        private const float right_column_width = 300;

        private readonly ISkin skin;

        protected KaraokeSkinEditorScreen(ISkin skin, KaraokeSkinEditorScreenMode type)
            : base(type)
        {
            this.skin = skin;
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            AddInternal(new SkinProvidingContainer(skin)
            {
                Children = new[]
                {
                    new Container
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Width = left_column_width,
                        RelativeSizeAxes = Axes.Y,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                Name = "Background",
                                Colour = colourProvider.Background2,
                                RelativeSizeAxes = Axes.Both,
                            },
                            new OsuScrollContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Scale = new Vector2(section_scale),
                                Size = new Vector2(1 / section_scale),
                                Child = new FillFlowContainer<Section>
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Children = CreateSelectionContainer(),
                                }
                            }
                        }
                    },
                    new Container
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Width = right_column_width,
                        RelativeSizeAxes = Axes.Y,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                Name = "Background",
                                Colour = colourProvider.Background2,
                                RelativeSizeAxes = Axes.Both,
                            },
                            new OsuScrollContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Scale = new Vector2(section_scale),
                                Size = new Vector2(1 / section_scale),
                                Child = new FillFlowContainer<Section>
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Children = CreatePropertiesContainer(),
                                }
                            }
                        }
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding { Left = left_column_width, Right = right_column_width },
                        Child = CreatePreviewArea(),
                    }
                }
            });
        }

        /// <summary>
        /// Create all sections with selectable options.
        /// </summary>
        /// <returns></returns>
        protected abstract Section[] CreateSelectionContainer();

        /// <summary>
        /// Create properties for the skin part.
        /// </summary>
        /// <returns></returns>
        protected abstract Section[] CreatePropertiesContainer();

        /// <summary>
        /// Create preview container.
        /// </summary>
        /// <returns></returns>
        protected abstract Container CreatePreviewArea();
    }
}
