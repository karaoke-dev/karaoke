// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji
{
    public abstract partial class TextTagEditModeSection<TEditModeState, TEditMode> : LyricEditorEditModeSection<TEditModeState, TEditMode>
        where TEditModeState : IHasEditModeState<TEditMode>
        where TEditMode : Enum
    {
        protected override OverlayColourScheme CreateColourScheme()
            => OverlayColourScheme.Pink;
    }
}
