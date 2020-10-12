// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;

namespace osu.Game.Rulesets.Karaoke.Tests.Beatmaps.Metadatas
{
    [TestFixture]
    public class SingerMetadataTest
    {
        private SingerMetadata metadata;

        [SetUp]
        public void SetUp()
        {
            metadata = new SingerMetadata();
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void TestCreateSinger(int count)
        {
            for (int i = 0; i < count; i++)
            {
                metadata.CreateSinger(x =>
                {
                    x.Name = $"Singer {x}";
                    x.RomajiName = $"Singer {x}";
                });
            }

            Assert.AreEqual(metadata.Singers.Count, count);
            Assert.AreEqual(metadata.Singers.Last().ID, count);
        }

        [TestCase(1, 1)]
        [TestCase(10, 5)]
        [TestCase(100, 100)]
        public void TestRemoveSinger(int createAmount, int removeAmount)
        {
            // Create singer
            for (int i = 0; i < createAmount; i++)
            {
                metadata.CreateSinger(x =>
                {
                    x.Name = $"Singer {x}";
                    x.RomajiName = $"Singer {x}";
                });
            }

            // Remove singer
            var existSingers = metadata.Singers.ToList();

            for (int i = 0; i < removeAmount; i++)
            {
                metadata.RemoveSinger(existSingers[i]);
            }

            Assert.AreEqual(metadata.Singers.Count, createAmount - removeAmount);
        }

        [TestCase(1, 1, 1, true)]
        [TestCase(1, 1, 10, true)]
        [TestCase(10, 10, 1, true)]
        [TestCase(0, 10, 10, false)]
        [TestCase(1, 10, 10, false)]
        public void TestCreateSubSinger(int parentAmount, int targetIndex, int amount, bool valid)
        {
            // Create parent singer
            for (int i = 0; i < parentAmount; i++)
            {
                metadata.CreateSinger(x =>
                {
                    x.Name = $"Parent singer {x}";
                    x.RomajiName = $"Singer {x}";
                });
            }

            try
            {
                // Create sub-ssinger
                var targetSinger = metadata.Singers.FirstOrDefault(x => x.ID == targetIndex);

                for (int i = 0; i < amount; i++)
                {
                    metadata.CreateSubSinger(targetSinger, x =>
                    {
                        if (targetSinger != null)
                            x.Description = $"Add sub singer into singer {targetSinger.Name}";
                    });
                }

                Assert.AreEqual(metadata.GetSubSingers(targetSinger).Count, amount);
            }
            catch
            {
                Assert.IsFalse(valid);
            }
        }

        [Test]
        public void TestRemoveSubSinger()
        {
            // Create singer
            metadata.CreateSinger(x =>
            {
                x.Name = $"Parent singer {x}";
                x.RomajiName = $"Singer {x}";
            });

            // Create subsinger
            var targetSinger = metadata.Singers.FirstOrDefault();
            metadata.CreateSubSinger(targetSinger, x =>
            {
                if (targetSinger != null)
                    x.Description = $"Add sub singer into singer {targetSinger.Name}";
            });

            Assert.AreEqual(metadata.GetSubSingers(targetSinger).Count, 1);

            //Remove
            var subSinger = metadata.GetSubSingers(targetSinger).FirstOrDefault();
            metadata.RemoveSubSinger(subSinger);

            Assert.AreEqual(metadata.GetSubSingers(targetSinger).Count, 0);
        }

        [TestCase(1, 0, new[] { 1 }, 1)]
        [TestCase(10, 0, new[] { 1, 2, 3 }, 7)]
        [TestCase(3, 3, new[] { 1, 4, 5 }, 25)]
        public void TestGetLayoutIndex(int singerAmount, int subSingerAmount, int[] indexs, int targetLayoutIndex)
        {
            var querySingers = new List<ISinger>();

            // Create parent singer
            for (int i = 0; i < singerAmount; i++)
            {
                metadata.CreateSinger(x =>
                {
                    x.Name = $"Parent singer {x}";
                    x.RomajiName = $"Singer {x}";

                    if (indexs.Contains(x.ID))
                        querySingers.Add(x);
                });
            }

            // Create sub-ssinger
            var targetSinger = metadata.Singers.LastOrDefault();

            for (int i = 0; i < subSingerAmount; i++)
            {
                metadata.CreateSubSinger(targetSinger, x =>
                {
                    if (targetSinger != null)
                        x.Description = $"Add sub singer into singer {targetSinger.Name}";

                    if (indexs.Contains(x.ID))
                        querySingers.Add(x);
                });
            }

            var layoutIndex = metadata.GetLayoutIndex(querySingers.ToArray());
            Assert.AreEqual(layoutIndex, targetLayoutIndex);
        }
    }
}
