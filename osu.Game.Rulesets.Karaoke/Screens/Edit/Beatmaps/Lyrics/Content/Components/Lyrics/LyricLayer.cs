// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

public partial class LyricLayer : Layer
{
    [Resolved]
    private OsuColour colours { get; set; } = null!;

    public LyricLayer(Lyric lyric)
        : base(lyric)
    {
    }

    public void ApplyDrawableLyric(Drawable drawable)
    {
        InternalChild = drawable;
    }

    public override void UpdateDisableEditState(bool editable)
    {
        this.FadeTo(editable ? 1 : 0.5f, 100);
    }

    public override void TriggerDisallowEditEffect(LyricEditorMode editorMode)
    {
        this.FlashColour(colours.Red, 200);
    }
}
