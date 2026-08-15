// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.Skinning.Legacy;

public partial class LegacyKaraokeColumnElement : LegacyKaraokeElement
{
    protected ScrollingNotePlayfield? NotePlayfield => Playfield?.NotePlayfield;

    // TODO : should override GetKaraokeSkinConfig() to pass the current column index once it's available.
}
