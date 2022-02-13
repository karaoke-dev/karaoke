// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public class LyricTimeTagsChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricTimeTagsChangeHandler, Lyric>
    {
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var baseDependencies = new DependencyContainer(base.CreateChildDependencies(parent));
            baseDependencies.Cache(new KaraokeRulesetEditGeneratorConfigManager());
            return baseDependencies;
        }

        [Test]
        public void TestAutoGenerateSupportedLyric()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Language = new CultureInfo(17)
            });

            TriggerHandlerChanged(c => c.AutoGenerate());

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(5, h.TimeTags.Count);
            });
        }

        [Test]
        public void TestAutoGenerateNonSupportedLyric()
        {
            PrepareHitObjects(new[]
            {
                new Lyric
                {
                    Text = "カラオケ",
                },
                new Lyric
                {
                    Text = "",
                },
                new Lyric
                {
                    Text = null,
                },
                new Lyric
                {
                    Text = "",
                    Language = new CultureInfo(17)
                },
                new Lyric
                {
                    Text = null,
                    Language = new CultureInfo(17)
                }
            });

            TriggerHandlerChanged(c => c.AutoGenerate());

            AssertSelectedHitObject(h =>
            {
                // should not able to generate time-tag if lyric text is empty, or did not have language.
                Assert.IsEmpty(h.TimeTags);
            });
        }

        [Test]
        public void TestSetTimeTagTime()
        {
            var timeTag = new TimeTag(new TextIndex(), 1000);
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                TimeTags = new[]
                {
                    timeTag
                }
            });

            TriggerHandlerChanged(c => c.SetTimeTagTime(timeTag, 2000));

            AssertSelectedHitObject(_ =>
            {
                Assert.AreEqual(2000, timeTag.Time);
            });
        }

        [Test]
        public void TestClearTimeTagTime()
        {
            var timeTag = new TimeTag(new TextIndex(), 1000);
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                TimeTags = new[]
                {
                    timeTag
                }
            });

            TriggerHandlerChanged(c => c.ClearTimeTagTime(timeTag));

            AssertSelectedHitObject(_ =>
            {
                Assert.IsNull(timeTag.Time);
            });
        }

        [Test]
        public void TestAdd()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
            });

            TriggerHandlerChanged(c => c.Add(new TimeTag(new TextIndex(), 1000)));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(1, h.TimeTags.Count);
                Assert.AreEqual(1000, h.TimeTags[0].Time);
            });
        }

        [Test]
        public void TestRemove()
        {
            var removedTag = new TimeTag(new TextIndex(), 1000);

            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                TimeTags = new[]
                {
                    removedTag,
                }
            });

            TriggerHandlerChanged(c => c.Remove(removedTag));

            AssertSelectedHitObject(h =>
            {
                Assert.IsEmpty(h.TimeTags);
            });
        }

        [Test]
        public void TestAddByPosition()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
            });

            TriggerHandlerChanged(c => c.AddByPosition(new TextIndex(3, TextIndex.IndexState.End)));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(1, h.TimeTags.Count);

                var actualTimeTag = h.TimeTags[0];
                Assert.AreEqual(new TextIndex(3, TextIndex.IndexState.End), actualTimeTag.Index);
                Assert.IsNull(actualTimeTag.Time);
            });
        }

        [Test]
        public void TestRemoveByPosition()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                TimeTags = new[]
                {
                    new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
                    new TimeTag(new TextIndex(3, TextIndex.IndexState.End)),
                    new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 5000),
                }
            });

            TriggerHandlerChanged(c => c.RemoveByPosition(new TextIndex(3, TextIndex.IndexState.End)));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(2, h.TimeTags.Count);

                // should delete the min time of the time-tag
                var actualTimeTag = h.TimeTags[0];
                Assert.AreEqual(new TextIndex(3, TextIndex.IndexState.End), actualTimeTag.Index);
                Assert.AreEqual(4000, actualTimeTag.Time);
            });
        }

        [Test]
        public void TestRemoveByPositionCase2()
        {
            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                TimeTags = new[]
                {
                    new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 5000),
                    new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
                }
            });

            TriggerHandlerChanged(c => c.RemoveByPosition(new TextIndex(3, TextIndex.IndexState.End)));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(1, h.TimeTags.Count);

                // should delete the min time of the time-tag
                var actualTimeTag = h.TimeTags[0];
                Assert.AreEqual(new TextIndex(3, TextIndex.IndexState.End), actualTimeTag.Index);
                Assert.AreEqual(5000, actualTimeTag.Time);
            });
        }
    }
}
