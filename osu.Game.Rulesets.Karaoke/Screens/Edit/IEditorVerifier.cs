// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit.Checks.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

public interface IEditorVerifier<in TEnum> where TEnum : struct, Enum
{
    IBindableList<Issue> GetIssueByType(TEnum type);

    void Refresh();
}

public interface IEditorVerifier
{
    IBindableList<Issue> Issues { get; }

    void Refresh();
}
