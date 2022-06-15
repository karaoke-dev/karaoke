// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers
{
    public class BeatmapChangeHandlerTest : BaseChangeHandlerTest<BeatmapChangeHandler>
    {
        [Test]
        public void TestImport()
        {
            SetUpKaraokeBeatmap(karaokeBeatmap =>
            {
                karaokeBeatmap.TotalColumns = 10;
            });

            TriggerHandlerChanged(c =>
            {
                var beatmap = new KaraokeBeatmap
                {
                    HitObjects = new List<KaraokeHitObject>
                    {
                        new Lyric(),
                        new Lyric(),
                        new Note(),
                    }
                };
                c.Import(beatmap);
            });

            AssertKaraokeBeatmap(karaokeBeatmap =>
            {
                // should not change the property in the karaoke beatmap.
                Assert.AreEqual(karaokeBeatmap.TotalColumns, 10);

                // check the hit objects.
                // and notice that we only import the lyric from other beatmap.
                Assert.AreEqual(karaokeBeatmap.HitObjects.Count, 2);
            });
        }

        [Test]
        public void TestSetScorable()
        {
            SetUpKaraokeBeatmap(karaokeBeatmap =>
            {
                karaokeBeatmap.Scorable = true;
            });

            TriggerHandlerChanged(c =>
            {
                c.SetScorable(false);
            });

            AssertKaraokeBeatmap(karaokeBeatmap =>
            {
                Assert.AreEqual(karaokeBeatmap.Scorable, false);
            });
        }
    }
}
