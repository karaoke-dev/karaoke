// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public interface ILyricEditorState
    {
        IBindable<LyricEditorMode> BindableMode { get; }

        IBindable<ModeWithSubMode> BindableModeAndSubMode { get; }

        LyricEditorMode Mode { get; }

        void SwitchMode(LyricEditorMode mode);

        void NavigateToFix(LyricEditorMode mode);
    }

    public struct ModeWithSubMode
    {
        public LyricEditorMode Mode;

        public Enum? SubMode;

        public bool Default;

        public ModeWithSubMode()
        {
            Mode = LyricEditorMode.View;
            SubMode = null;
            Default = true;
        }
    }
}
