// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.ComponentModel;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Skinning;
using osuTK;
using Container = osu.Framework.Graphics.Containers.Container;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Style
{
    public class StyleScreen : KaraokeSkinEditorScreen
    {
        [Cached]
        protected readonly StyleManager StyleManager;

        public StyleScreen(ISkin skin)
            : base(skin, KaraokeSkinEditorScreenMode.Style)
        {
            AddInternal(StyleManager = new StyleManager());
        }

        protected override Section[] CreateSelectionContainer()
            => new Section[] { };

        protected override Section[] CreatePropertiesContainer()
            => new Section[]
            {
                // style
                new LyricColorSection(),
                new LyricFontSection(),
                new LyricShadowSection(),
                // note
                new NoteColorSection(),
                new NoteFontSection(),
            };

        protected override Container CreatePreviewArea()
            => new LyricStylePreview
            {
                Name = "Lyric style preview area",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(0.95f),
                RelativeSizeAxes = Axes.Both
            };

        /*
        protected override Container CreatePreviewArea()
            => new NoteStylePreview
            {
                Name = "Note style preview area",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(0.95f),
                RelativeSizeAxes = Axes.Both
            };
        */
    }

    public enum Style
    {
        [Description("Lyric")]
        Lyric,

        [Description("Note")]
        Note
    }
}
