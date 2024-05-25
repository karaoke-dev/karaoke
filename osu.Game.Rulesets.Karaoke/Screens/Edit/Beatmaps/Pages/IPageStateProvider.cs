// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Pages;

public interface IPageStateProvider
{
    Bindable<PageEditorEditMode> BindableEditMode { get; }

    PageEditorEditMode EditMode => BindableEditMode.Value;

    PageInfo PageInfo { get; }

    BindableList<Page> SelectedItems { get; }

    void Select(Page item);

    BindableFloat BindableZoom { get; }

    BindableFloat BindableCurrent { get; }
}
