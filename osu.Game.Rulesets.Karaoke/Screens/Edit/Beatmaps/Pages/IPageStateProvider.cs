// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages;

public interface IPageStateProvider
{
    IBindable<PageEditorEditMode> BindableEditMode { get; }

    PageEditorEditMode EditMode => BindableEditMode.Value;

    void ChangeEditMode(PageEditorEditMode mode);
}
