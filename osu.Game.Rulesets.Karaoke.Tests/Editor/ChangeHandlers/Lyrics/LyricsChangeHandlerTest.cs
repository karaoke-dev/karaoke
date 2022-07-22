// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public class LyricsChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricsChangeHandler, Lyric>
    {
        [Test]
        public void TestSplit()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ"
            });

            TriggerHandlerChanged(c => c.Split(2));

            AssertHitObjects(objects =>
            {
                var lyrics = objects.ToArray();
                var firstLyric = lyrics.First();
                var secondLyric = lyrics.Last();

                // test property in the first lyric.
                Assert.AreEqual("カラ", firstLyric.Text);
                Assert.AreEqual(2, firstLyric.ID);
                Assert.AreEqual(0, firstLyric.Order);

                // test property in the second lyric.
                Assert.AreEqual("オケ", secondLyric.Text);
                Assert.AreEqual(1, secondLyric.ID);
                Assert.AreEqual(1, secondLyric.Order);
            });
        }

        [Test]
        public void TestCombine()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラ",
                ID = 0,
                Order = 1,
            }, false);

            PrepareHitObject(new Lyric
            {
                Text = "オケ",
                ID = 1,
                Order = 2,
            });

            PrepareHitObject(new Lyric
            {
                Text = "karaoke",
                ID = 2,
                Order = 3,
            }, false);

            TriggerHandlerChanged(c => c.Combine());

            AssertHitObjects(objects =>
            {
                var lyrics = objects.ToArray();
                Assert.AreEqual(2, lyrics.Length);

                var combinedLyric = lyrics.First(x => x.Text == "カラオケ");
                var notAffectedLyric = lyrics.First(x => x.Text == "karaoke");

                Assert.AreEqual(3, combinedLyric.ID);
                Assert.AreEqual(1, combinedLyric.Order);

                Assert.AreEqual(2, notAffectedLyric.ID);
                Assert.AreEqual(2, notAffectedLyric.Order);
            });
        }

        [Test]
        public void TestCreateAtPosition()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                ID = 1,
                Order = 1,
            });

            PrepareHitObject(new Lyric
            {
                Text = "karaoke",
                ID = 2,
                Order = 2,
            }, false);

            TriggerHandlerChanged(c => c.CreateAtPosition());

            AssertHitObjects(hitObjects =>
            {
                var lyrics = hitObjects.ToArray();
                Assert.AreEqual(3, lyrics.Length);

                var firstLyric = lyrics.First(x => x.Text == "カラオケ");
                Assert.AreEqual(1, firstLyric.ID);
                Assert.AreEqual(1, firstLyric.Order);

                var secondLyric = lyrics.First(x => x.Text == "New lyric");
                Assert.AreEqual(3, secondLyric.ID);
                Assert.AreEqual(2, secondLyric.Order);

                var thirdLyric = lyrics.First(x => x.Text == "karaoke");
                Assert.AreEqual(2, thirdLyric.ID);
                Assert.AreEqual(3, thirdLyric.Order);
            });
        }

        [Test]
        public void TestCreateAtLast()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                ID = 1,
                Order = 1,
            });

            PrepareHitObject(new Lyric
            {
                Text = "karaoke",
                ID = 2,
                Order = 2,
            }, false);

            TriggerHandlerChanged(c => c.CreateAtLast());

            AssertHitObjects(hitObjects =>
            {
                var lyrics = hitObjects.ToArray();
                Assert.AreEqual(3, lyrics.Length);

                var firstLyric = lyrics.First(x => x.Text == "カラオケ");
                Assert.AreEqual(1, firstLyric.ID);
                Assert.AreEqual(1, firstLyric.Order);

                var secondLyric = lyrics.First(x => x.Text == "karaoke");
                Assert.AreEqual(2, secondLyric.ID);
                Assert.AreEqual(2, secondLyric.Order);

                var thirdLyric = lyrics.First(x => x.Text == "New lyric");
                Assert.AreEqual(3, thirdLyric.ID);
                Assert.AreEqual(3, thirdLyric.Order);
            });
        }

        [Test]
        public void TestCreateAtLastWithEmptyBeatmap()
        {
            TriggerHandlerChanged(c => c.CreateAtLast());

            AssertHitObjects(hitObjects =>
            {
                var lyrics = hitObjects.ToArray();
                Assert.AreEqual(1, lyrics.Length);

                var addedLyric = lyrics.First(x => x.Text == "New lyric");
                Assert.AreEqual(1, addedLyric.ID);
                Assert.AreEqual(1, addedLyric.Order);
            });
        }

        [Test]
        public void TestAddBelowToSelection()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ"
            });

            TriggerHandlerChanged(c => c.AddBelowToSelection(new Lyric
            {
                Text = "New lyric"
            }));

            AssertHitObjects(hitObjects =>
            {
                var lyrics = hitObjects.ToArray();
                Assert.AreEqual(2, lyrics.Length);

                var addedLyric = lyrics.First(x => x.Text == "New lyric");
                Assert.AreEqual(1, addedLyric.ID);
                Assert.AreEqual(1, addedLyric.Order);
            });
        }

        [Test]
        public void TestAddRangeBelowToSelection()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ"
            });

            TriggerHandlerChanged(c => c.AddRangeBelowToSelection(new[]
            {
                new Lyric
                {
                    Text = "New lyric"
                }
            }));

            AssertHitObjects(hitObjects =>
            {
                var lyrics = hitObjects.ToArray();
                Assert.AreEqual(2, lyrics.Length);

                var addedLyric = lyrics.First(x => x.Text == "New lyric");
                Assert.AreEqual(1, addedLyric.ID);
                Assert.AreEqual(1, addedLyric.Order);
            });
        }

        [Test]
        public void TestRemove()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                ID = 1,
                Order = 1,
            });

            PrepareHitObject(new Lyric
            {
                Text = "karaoke",
                ID = 2,
                Order = 2,
            }, false);

            TriggerHandlerChanged(c => c.Remove());

            AssertHitObjects(hitObjects =>
            {
                var lyrics = hitObjects.ToArray();
                Assert.AreEqual(1, lyrics.Length);

                var secondLyric = lyrics.First(x => x.Text == "karaoke");
                Assert.AreEqual(2, secondLyric.ID);
                Assert.AreEqual(1, secondLyric.Order);
            });
        }

        [Test]
        public void TestChangeOrder()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                ID = 1,
                Order = 1,
            });

            PrepareHitObject(new Lyric
            {
                Text = "karaoke",
                ID = 2,
                Order = 2,
            }, false);

            // move the "カラオケ" lyric next of the "karaoke" lyric.
            TriggerHandlerChanged(c => c.ChangeOrder(1));

            AssertHitObjects(hitObjects =>
            {
                var lyrics = hitObjects.ToArray();
                Assert.AreEqual(2, lyrics.Length);

                var firstLyric = lyrics.First(x => x.Text == "karaoke");
                Assert.AreEqual(2, firstLyric.ID);
                Assert.AreEqual(1, firstLyric.Order);

                var secondLyric = lyrics.First(x => x.Text == "カラオケ");
                Assert.AreEqual(1, secondLyric.ID);
                Assert.AreEqual(2, secondLyric.Order);
            });
        }
    }
}
