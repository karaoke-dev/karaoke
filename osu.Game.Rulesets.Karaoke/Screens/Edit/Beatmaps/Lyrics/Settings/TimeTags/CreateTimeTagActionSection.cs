// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;

public partial class CreateTimeTagActionSection : EditorSection, IKeyBindingHandler<KaraokeEditAction>
{
    protected override LocalisableString Title => "Action";

    [Resolved]
    private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; } = null!;

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    private readonly Bindable<CreateTimeTagType> bindableCreateType = new();

    public CreateTimeTagActionSection()
    {
        Children = new[]
        {
            new CreateTimeTagTypeSubsection
            {
                Current = bindableCreateType,
            },
        };
    }

    [BackgroundDependencyLoader]
    private void load(IEditTimeTagModeState editTimeTagModeState)
    {
        bindableCreateType.BindTo(editTimeTagModeState.BindableCreateType);
    }

    public bool OnPressed(KeyBindingPressEvent<KaraokeEditAction> e)
    {
        var action = e.Action;
        var caretPosition = lyricCaretState.CaretPosition;

        return caretPosition switch
        {
            CreateRemoveTimeTagCaretPosition timeTagIndexCaretPosition => processCreateTimeTagAction(timeTagIndexCaretPosition, action),
            _ => throw new NotSupportedException(nameof(caretPosition)),
        };
    }

    private bool processCreateTimeTagAction(CreateRemoveTimeTagCaretPosition createRemoveTimeTagCaretPosition, KaraokeEditAction action)
    {
        int index = createRemoveTimeTagCaretPosition.CharIndex;

        switch (action)
        {
            case KaraokeEditAction.CreateStartTimeTag:
                lyricTimeTagsChangeHandler.AddByPosition(new TextIndex(index));
                return true;

            case KaraokeEditAction.CreateEndTimeTag:
                lyricTimeTagsChangeHandler.AddByPosition(new TextIndex(index, TextIndex.IndexState.End));
                return true;

            case KaraokeEditAction.RemoveStartTimeTag:
                lyricTimeTagsChangeHandler.RemoveByPosition(new TextIndex(index));
                return true;

            case KaraokeEditAction.RemoveEndTimeTag:
                lyricTimeTagsChangeHandler.RemoveByPosition(new TextIndex(index, TextIndex.IndexState.End));
                return true;

            default:
                return false;
        }
    }

    public void OnReleased(KeyBindingReleaseEvent<KaraokeEditAction> e)
    {
    }
}
