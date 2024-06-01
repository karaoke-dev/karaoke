// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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

    private readonly IBindable<ICaretPosition?> bindableCaretPosition = new Bindable<ICaretPosition?>();
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
    private void load(IEditTimeTagModeState editTimeTagModeState, ILyricCaretState lyricCaretState)
    {
        bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);
        bindableCreateType.BindTo(editTimeTagModeState.BindableCreateType);
    }

    public bool OnPressed(KeyBindingPressEvent<KaraokeEditAction> e)
    {
        var action = e.Action;
        var caretPosition = bindableCaretPosition.Value;

        if (caretPosition is not CreateRemoveTimeTagCaretPosition createRemoveTimeTagCaretPosition)
            return false;

        if (LyricEditor.ToMovingCaretAction(e.Action) != null)
        {
            bindableCreateType.Value = CreateTimeTagType.Keyboard;
            return false;
        }

        if (createTimeTagByKeyboard(createRemoveTimeTagCaretPosition.CharIndex, action))
        {
            bindableCreateType.Value = CreateTimeTagType.Keyboard;
            return true;
        }

        return false;
    }

    private bool createTimeTagByKeyboard(int charIndex, KaraokeEditAction action)
    {
        switch (action)
        {
            case KaraokeEditAction.CreateStartTimeTag:
                lyricTimeTagsChangeHandler.AddByPosition(new TextIndex(charIndex));
                return true;

            case KaraokeEditAction.CreateEndTimeTag:
                lyricTimeTagsChangeHandler.AddByPosition(new TextIndex(charIndex, TextIndex.IndexState.End));
                return true;

            case KaraokeEditAction.RemoveStartTimeTag:
                lyricTimeTagsChangeHandler.RemoveByPosition(new TextIndex(charIndex));
                return true;

            case KaraokeEditAction.RemoveEndTimeTag:
                lyricTimeTagsChangeHandler.RemoveByPosition(new TextIndex(charIndex, TextIndex.IndexState.End));
                return true;

            default:
                return false;
        }
    }

    public void OnReleased(KeyBindingReleaseEvent<KaraokeEditAction> e)
    {
    }
}
