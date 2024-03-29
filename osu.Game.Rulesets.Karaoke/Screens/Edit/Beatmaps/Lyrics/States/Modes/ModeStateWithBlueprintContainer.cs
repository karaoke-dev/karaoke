﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public abstract partial class ModeStateWithBlueprintContainer<TObject> : Component, IHasBlueprintSelection<TObject> where TObject : class
{
    private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
    private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();
    private readonly IBindable<int> bindableLyricPropertyWritableVersion = new Bindable<int>();

    public BindableList<TObject> SelectedItems { get; } = new();

    protected ModeStateWithBlueprintContainer()
    {
        bindableMode.BindValueChanged(e =>
        {
            TriggerDisableStateChanged();
        });

        bindableCaretPosition.BindValueChanged(e =>
        {
            bindableLyricPropertyWritableVersion.UnbindBindings();

            var lyric = e.NewValue?.Lyric;

            if (lyric == null)
                return;

            bindableLyricPropertyWritableVersion.BindTo(lyric.LyricPropertyWritableVersion);
            TriggerDisableStateChanged();
        });

        bindableLyricPropertyWritableVersion.BindValueChanged(_ =>
        {
            TriggerDisableStateChanged();
        });
    }

    protected virtual void TriggerDisableStateChanged()
    {
        var caret = bindableCaretPosition.Value;
        if (caret == null)
            return;

        var lyric = caret.Lyric;

        SelectedItems.Clear();
        bool locked = IsWriteLyricPropertyLocked(lyric);
        if (locked)
            return;

        // should not select the items from the lyric when mouse click to the lyric.
        // because selected items will be clear when mouse up.
        bool mousePressed = isMousePressed();
        if (mousePressed || !SelectFirstProperty(lyric))
            return;

        var firstItem = SelectableProperties(lyric).FirstOrDefault();
        if (firstItem != null)
            SelectedItems.Add(firstItem);
    }

    private bool isMousePressed() => GetContainingInputManager().CurrentState.Mouse.Buttons.HasAnyButtonPressed;

    protected abstract bool IsWriteLyricPropertyLocked(Lyric lyric);

    protected abstract bool SelectFirstProperty(Lyric lyric);

    protected abstract IEnumerable<TObject> SelectableProperties(Lyric lyric);

    public void Select(TObject item)
    {
        // not trigger again if already focus.
        if (SelectedItems.Contains(item) && SelectedItems.Count == 1)
            return;

        // trigger selected.
        SelectedItems.Clear();
        SelectedItems.Add(item);
    }

    [BackgroundDependencyLoader]
    private void load(ILyricEditorState state, ILyricCaretState lyricCaretState)
    {
        bindableMode.BindTo(state.BindableMode);
        bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);
    }
}
