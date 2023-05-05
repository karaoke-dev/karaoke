// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

public interface ILyricEditorState
{
    IBindable<LyricEditorMode> BindableMode { get; }

    IBindable<ModeWithSubMode> BindableModeAndSubMode { get; }

    LyricEditorMode Mode { get; }

    void SwitchMode(LyricEditorMode mode);

    void SwitchSubMode<TSubMode>(TSubMode subMode) where TSubMode : Enum;

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

    public TSubMode GetSubMode<TSubMode>() where TSubMode : Enum
    {
        if (SubMode is not TSubMode subMode)
            throw new InvalidOperationException();

        return subMode;
    }
}
