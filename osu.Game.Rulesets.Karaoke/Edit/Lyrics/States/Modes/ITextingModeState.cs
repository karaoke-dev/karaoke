// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Manage;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public interface ITextingModeState : IHasEditModeState<TextingEditMode>, IHasSpecialAction<ManageEditModeSpecialAction>
    {
    }
}
