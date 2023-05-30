// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Toolbar.TimeTags;

public partial class CreateTimeTagButton : KeyActionButton
{
    protected override KaraokeEditAction EditAction
        => TextIndexUtils.GetValueByState(indexState, KaraokeEditAction.CreateStartTimeTag, KaraokeEditAction.CreateEndTimeTag);

    [Resolved]
    private ILyricCaretState lyricCaretState { get; set; } = null!;

    [Resolved]
    private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; } = null!;

    private readonly TextIndex.IndexState indexState;

    public CreateTimeTagButton(TextIndex.IndexState indexState)
    {
        this.indexState = indexState;

        SetIcon(FontAwesome.Solid.Tag);

        Action = () =>
        {
            if (lyricCaretState.CaretPosition is not TimeTagIndexCaretPosition charIndexCaretPosition)
                throw new InvalidOperationException();

            int index = charIndexCaretPosition.CharIndex;
            lyricTimeTagsChangeHandler.AddByPosition(new TextIndex(index, this.indexState));
        };
    }
}
