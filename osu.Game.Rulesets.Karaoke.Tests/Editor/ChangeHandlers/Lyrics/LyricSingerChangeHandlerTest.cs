// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public class LyricSingerChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricSingerChangeHandler, Lyric>
    {
        [Test]
        public void TestAdd()
        {
            var singer = new Singer(1)
            {
                Name = "Singer1",
            };
            PrepareHitObject(new Lyric());

            TriggerHandlerChanged(c => c.Add(singer));

            AssertSelectedHitObject(h =>
            {
                var singers = h.Singers;
                Assert.AreEqual(1, singers.Count);
                Assert.AreEqual(singer.ID, singers.FirstOrDefault());
            });
        }

        [Test]
        public void TestAddRange()
        {
            var singer = new Singer(1)
            {
                Name = "Singer1",
            };
            PrepareHitObject(new Lyric());

            TriggerHandlerChanged(c => c.AddRange(new[] { singer }));

            AssertSelectedHitObject(h =>
            {
                var singers = h.Singers;
                Assert.AreEqual(1, singers.Count);
                Assert.AreEqual(singer.ID, singers.FirstOrDefault());
            });
        }

        [Test]
        public void TestRemove()
        {
            var singer = new Singer(1)
            {
                Name = "Singer1",
            };
            var anotherSinger = new Singer(2)
            {
                Name = "Another singer",
            };
            PrepareHitObject(new Lyric
            {
                Singers = new[]
                {
                    singer.ID,
                    anotherSinger.ID,
                }
            });

            TriggerHandlerChanged(c => c.Remove(singer));

            AssertSelectedHitObject(h =>
            {
                var singers = h.Singers;

                // should not contains removed singer.
                Assert.IsFalse(singers.Contains(singer.ID));

                // should only contain remain singer.
                Assert.AreEqual(1, singers.Count);
                Assert.IsTrue(singers.Contains(anotherSinger.ID));
            });
        }

        [Test]
        public void TestRemoveRange()
        {
            var singer = new Singer(1)
            {
                Name = "Singer1",
            };
            var anotherSinger = new Singer(2)
            {
                Name = "Another singer",
            };
            PrepareHitObject(new Lyric
            {
                Singers = new[]
                {
                    singer.ID,
                    anotherSinger.ID,
                }
            });

            TriggerHandlerChanged(c => c.RemoveRange(new[] { singer }));

            AssertSelectedHitObject(h =>
            {
                var singers = h.Singers;

                // should not contains removed singer.
                Assert.IsFalse(singers.Contains(singer.ID));

                // should only contain remain singer.
                Assert.AreEqual(1, singers.Count);
                Assert.IsTrue(singers.Contains(anotherSinger.ID));
            });
        }

        [Test]
        public void TestClear()
        {
            var singer = new Singer(1)
            {
                Name = "Singer1",
            };
            PrepareHitObject(new Lyric
            {
                Singers = new[]
                {
                    singer.ID,
                }
            });

            TriggerHandlerChanged(c => c.Clear());

            AssertSelectedHitObject(h =>
            {
                Assert.IsEmpty(h.Singers);
            });
        }
    }
}
