// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public class LyricRomajiTagsChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricRomajiTagsChangeHandler, Lyric>
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
                Text = "風",
                Language = new CultureInfo(17)
            });

            TriggerHandlerChanged(c => c.AutoGenerate());

            AssertSelectedHitObject(h =>
            {
                var romajiTags = h.RomajiTags;
                Assert.AreEqual(1, romajiTags.Count);
                Assert.AreEqual("kaze", romajiTags[0].Text);
            });
        }

        [Test]
        public void TestAutoGenerateNonSupportedLyric()
        {
            PrepareHitObjects(new[]
            {
                new Lyric
                {
                    Text = "風",
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
                Assert.IsEmpty(h.RomajiTags);
            });
        }

        [Test]
        public void TestAdd()
        {
            PrepareHitObject(new Lyric
            {
                Text = "風",
                Language = new CultureInfo(17)
            });

            TriggerHandlerChanged(c => c.Add(new RomajiTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "kaze",
            }));

            AssertSelectedHitObject(h =>
            {
                var romajiTags = h.RomajiTags;
                Assert.AreEqual(1, romajiTags.Count);
                Assert.AreEqual("kaze", romajiTags[0].Text);
            });
        }

        [Test]
        public void TestRemove()
        {
            var removedTag = new RomajiTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "kaze",
            };

            PrepareHitObject(new Lyric
            {
                Text = "風",
                Language = new CultureInfo(17),
                RomajiTags = new List<RomajiTag>
                {
                    removedTag
                }
            });

            TriggerHandlerChanged(c => c.Remove(removedTag));

            AssertSelectedHitObject(h =>
            {
                Assert.IsEmpty(h.RomajiTags);
            });
        }

        [Test]
        public void TestRemoveAll()
        {
            var removedTag = new RomajiTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "ka",
            };

            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Language = new CultureInfo(17),
                RomajiTags = new List<RomajiTag>
                {
                    removedTag,
                    new()
                    {
                        StartIndex = 1,
                        EndIndex = 2,
                        Text = "ra",
                    }
                }
            });

            TriggerHandlerChanged(c => c.RemoveAll(new[] { removedTag }));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(1, h.RomajiTags.Count);
            });
        }

        [Test]
        public void TestSetIndex()
        {
            var targetTag = new RomajiTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "ka",
            };

            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Language = new CultureInfo(17),
                RomajiTags = new List<RomajiTag>
                {
                    targetTag
                }
            });

            TriggerHandlerChanged(c => c.SetIndex(targetTag, 1, 2));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(1, targetTag.StartIndex);
                Assert.AreEqual(2, targetTag.EndIndex);
            });
        }

        [Test]
        public void TestShiftingIndex()
        {
            var targetTag = new RomajiTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "ka",
            };

            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Language = new CultureInfo(17),
                RomajiTags = new List<RomajiTag>
                {
                    targetTag
                }
            });

            TriggerHandlerChanged(c => c.ShiftingIndex(new[] { targetTag }, 1));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(1, targetTag.StartIndex);
                Assert.AreEqual(2, targetTag.EndIndex);
            });
        }

        [Test]
        public void TestSetText()
        {
            var targetTag = new RomajiTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "ka",
            };

            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Language = new CultureInfo(17),
                RomajiTags = new List<RomajiTag>
                {
                    targetTag
                }
            });

            TriggerHandlerChanged(c => c.SetText(targetTag, "karaoke"));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual("karaoke", targetTag.Text);
            });
        }
    }
}
