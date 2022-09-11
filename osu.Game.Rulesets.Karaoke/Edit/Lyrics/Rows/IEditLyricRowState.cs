// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows
{
    public interface IEditLyricRowState
    {
        event Action<LyricEditorMode> WritableVersionChanged;
    }
}
