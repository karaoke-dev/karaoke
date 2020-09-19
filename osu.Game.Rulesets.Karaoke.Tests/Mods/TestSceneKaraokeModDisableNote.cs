// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Mods;
using osu.Game.Rulesets.Karaoke.Tests.Beatmaps;
using osu.Game.Rulesets.Karaoke.Tests.Extensions;

namespace osu.Game.Rulesets.Karaoke.Tests.Mods
{
    public class TestSceneKaraokeModDisableNote : KaraokeModTestScene
    {
        [Test]
        public void TestCheckNoteExistInPlayfield() => CreateModTest(new ModTestData
        {
            Mod = new KaraokeModDisableNote(),
            Autoplay = true,
            Beatmap = new TestKaraokeBeatmap(null),
            PassCondition = () =>
            {
                var lyricPlayfield = Player.GetLyricPlayfield();
                var notePlayfield = Player.GetNotePlayfield();
                if (lyricPlayfield == null || notePlayfield == null)
                    return false;

                // check has no note in playfield
                return lyricPlayfield.AllHitObjects.Any() && !notePlayfield.AllHitObjects.Any();
            }
        });
    }
}
