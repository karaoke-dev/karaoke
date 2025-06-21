// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Skin.Style;

public partial class StyleScreen : KaraokeSkinEditorScreen
{
    public StyleScreen(ISkin skin)
        : base(skin, KaraokeSkinEditorScreenMode.Style)
    {
    }

    protected override Section[] CreateSelectionContainer()
        => Array.Empty<Section>();

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

    protected override Drawable CreatePreviewArea()
        => new LyricStylePreview
        {
            Name = "Lyric style preview area",
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Size = new Vector2(0.95f),
            RelativeSizeAxes = Axes.Both,
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
    [System.ComponentModel.Description("Lyric")]
    Lyric,

    [System.ComponentModel.Description("Note")]
    Note,
}
