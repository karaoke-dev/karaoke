// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Edit.Checks.Components;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages;

public interface IPageEditorVerifier
{
    IBindableList<Issue> Issues { get; }

    void Refresh();

    void Navigate(Issue issue);
}
