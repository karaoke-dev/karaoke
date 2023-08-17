// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

public interface ILyricEditorState
{
    IBindable<LyricEditorMode> BindableMode { get; }

    IBindable<EditorModeWithEditStep> BindableModeWithEditStep { get; }

    LyricEditorMode Mode { get; }

    void SwitchMode(LyricEditorMode mode);

    void SwitchSubMode<TSubMode>(TSubMode subMode) where TSubMode : Enum;

    void NavigateToFix(LyricEditorMode mode);
}

public struct EditorModeWithEditStep
{
    public LyricEditorMode Mode;

    public Enum? EditStep;

    public bool Default;

    public EditorModeWithEditStep()
    {
        Mode = LyricEditorMode.View;
        EditStep = null;
        Default = true;
    }

    public TEditStep GetEditStep<TEditStep>() where TEditStep : Enum
    {
        if (EditStep is not TEditStep editStep)
            throw new InvalidOperationException();

        return editStep;
    }
}
