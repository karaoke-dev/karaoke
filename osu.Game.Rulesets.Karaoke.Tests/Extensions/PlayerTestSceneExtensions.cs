// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Extensions
{
    public static class PlayerTestSceneExtensions
    {
        public static DrawableKaraokeRuleset GetDrawableRuleset(this TestPlayer testPlayer)
        {
            return testPlayer.DrawableRuleset as DrawableKaraokeRuleset;
        }

        public static KaraokePlayfield GetPlayfield(this TestPlayer testPlayer)
        {
            return testPlayer.GetDrawableRuleset()?.Playfield;
        }

        public static NotePlayfield GetNotePlayfield(this TestPlayer testPlayer)
        {
            return testPlayer.GetPlayfield()?.NotePlayfield;
        }

        public static LyricPlayfield GetLyricPlayfield(this TestPlayer testPlayer)
        {
            return testPlayer.GetPlayfield()?.LyricPlayfield;
        }
    }
}
