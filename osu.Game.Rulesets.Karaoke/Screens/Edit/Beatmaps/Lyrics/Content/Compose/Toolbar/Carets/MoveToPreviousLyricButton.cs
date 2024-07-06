// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Compose.Toolbar.Carets;

public partial class MoveToPreviousLyricButton : MoveToCaretPositionButton
{
    protected override KaraokeEditAction EditAction => KaraokeEditAction.MoveToPreviousLyric;

    protected override MovingCaretAction AcceptAction => MovingCaretAction.PreviousLyric;

    public MoveToPreviousLyricButton()
    {
        SetIcon(FontAwesome.Solid.ArrowUp);
    }
}
