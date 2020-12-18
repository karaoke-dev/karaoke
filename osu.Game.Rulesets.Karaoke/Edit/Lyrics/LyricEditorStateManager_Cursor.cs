// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public partial class LyricEditorStateManager
    {
        public Bindable<CursorPosition> BindableCursorHoverPosition { get; } = new Bindable<CursorPosition>();

        public Bindable<CursorPosition> BindableCursorPosition { get; } = new Bindable<CursorPosition>();

        public void UpdateSplitCursorPosition(Lyric lyric, TimeTagIndex index)
        {
            BindableCursorPosition.Value = new CursorPosition(lyric, index);
        }

        public void ClearSplitCursorPosition()
        {
            BindableCursorPosition.Value = new CursorPosition();
        }
    }
}
