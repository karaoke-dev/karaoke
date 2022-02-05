// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Layout
{
    public class LayoutScreen : KaraokeSkinEditorScreen
    {
        [Cached]
        protected readonly LayoutManager LayoutManager;

        public LayoutScreen(ISkin skin)
            : base(skin, KaraokeSkinEditorScreenMode.Layout)
        {
            AddInternal(LayoutManager = new LayoutManager());
        }

        protected override Section[] CreateSelectionContainer()
            => new Section[]
            {
                new LayoutSelection(),
            };

        protected override Section[] CreatePropertiesContainer()
            => new Section[]
            {
                new LayoutAlignmentSection(),
                new PreviewSection(),
            };

        protected override Container CreatePreviewArea()
            => new LayoutPreview
            {
                Name = "Layout preview area",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(0.95f),
                RelativeSizeAxes = Axes.Both
            };
    }
}
