// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public interface ILyricEditorState
    {
        IBindable<LyricEditorMode> BindableMode { get; }

        LyricEditorMode Mode { get; }

        void SwitchMode(LyricEditorMode mode);

        void NavigateToFix(LyricEditorMode mode);
    }
}
