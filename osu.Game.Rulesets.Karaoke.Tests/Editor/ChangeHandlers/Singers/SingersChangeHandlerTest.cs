// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Singers
{
    public class SingersChangeHandlerTest : BaseChangeHandlerTest<SingersChangeHandler>
    {
        [Test]
        [Ignore("Not working because singer in karaoke beatmap only sync to change handler once.")]
        public void TestChangeOrder()
        {
            var firstSinger = new Singer(0)
            {
                Order = 1
            };
            var secondSinger = new Singer(1)
            {
                Order = 2
            };

            SetUpKaraokeBeatmap(karaokeBeatmap =>
            {
                karaokeBeatmap.Singers = new List<Singer>
                {
                    firstSinger,
                    secondSinger,
                };
            });

            TriggerHandlerChanged(c =>
            {
                c.ChangeOrder(firstSinger, 2);
            });

            AssertKaraokeBeatmap(karaokeBeatmap =>
            {
                Assert.AreEqual(1, firstSinger.ID);
                Assert.AreEqual(2, firstSinger.Order);

                Assert.AreEqual(2, secondSinger.ID);
                Assert.AreEqual(1, secondSinger.Order);
            });
        }

        [Test]
        [Ignore("It's hard to test this because it needs lots of dependencies.")]
        public void TestChangeSingerAvatar()
        {
        }

        [Test]
        [Ignore("Not working because singer in karaoke beatmap only sync to change handler once.")]
        public void TestAdd()
        {
            SetUpKaraokeBeatmap(karaokeBeatmap =>
            {
                karaokeBeatmap.Singers = new List<Singer>
                {
                    new(0)
                    {
                        Order = 1
                    },
                    new(1)
                    {
                        Order = 2
                    },
                };
            });

            TriggerHandlerChanged(c =>
            {
                c.Add(new Singer(2));
            });

            AssertKaraokeBeatmap(karaokeBeatmap =>
            {
                var singers = karaokeBeatmap.Singers;
                Assert.AreEqual(3, singers.Count);

                var lastSinger = singers.LastOrDefault();
                Assert.IsNotNull(lastSinger);
                Assert.AreEqual(2, lastSinger.ID);
                Assert.AreEqual(3, lastSinger.Order);
            });
        }

        [Test]
        [Ignore("Not working because singer in karaoke beatmap only sync to change handler once.")]
        public void TestRemove()
        {
            var firstSinger = new Singer(0)
            {
                Order = 1
            };
            var secondSinger = new Singer(1)
            {
                Order = 2
            };

            SetUpKaraokeBeatmap(karaokeBeatmap =>
            {
                karaokeBeatmap.Singers = new List<Singer>
                {
                    firstSinger,
                    secondSinger
                };

                karaokeBeatmap.HitObjects = new List<KaraokeHitObject>
                {
                    new Lyric
                    {
                        Singers = { firstSinger.ID }
                    }
                };
            });

            TriggerHandlerChanged(c =>
            {
                c.Remove(firstSinger);
            });

            AssertKaraokeBeatmap(karaokeBeatmap =>
            {
                var singers = karaokeBeatmap.Singers;
                Assert.AreEqual(1, singers.Count);

                Assert.AreEqual(1, secondSinger.ID);
                Assert.AreEqual(1, secondSinger.Order);

                var lyrics = karaokeBeatmap.HitObjects.OfType<Lyric>().Where(x => x.Singers.Contains(firstSinger.ID));
                Assert.IsEmpty(lyrics);
            });
        }
    }
}
