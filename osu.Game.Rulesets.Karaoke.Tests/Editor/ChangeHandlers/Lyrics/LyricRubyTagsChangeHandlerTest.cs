// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public class LyricRubyTagsChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricRubyTagsChangeHandler, Lyric>
    {
        [Test]
        public void TestAdd()
        {
            PrepareHitObject(new Lyric
            {
                Text = "風",
                Language = new CultureInfo(17)
            });

            TriggerHandlerChanged(c => c.Add(new RubyTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "かぜ",
            }));

            AssertSelectedHitObject(h =>
            {
                var rubyTags = h.RubyTags;
                Assert.AreEqual(1, rubyTags.Count);
                Assert.AreEqual("かぜ", rubyTags[0].Text);
            });
        }

        [Test]
        public void TestAddRange()
        {
            PrepareHitObject(new Lyric
            {
                Text = "風",
                Language = new CultureInfo(17)
            });

            TriggerHandlerChanged(c => c.AddRange(new[]
            {
                new RubyTag
                {
                    StartIndex = 0,
                    EndIndex = 1,
                    Text = "かぜ",
                }
            }));

            AssertSelectedHitObject(h =>
            {
                var rubyTags = h.RubyTags;
                Assert.AreEqual(1, rubyTags.Count);
                Assert.AreEqual("かぜ", rubyTags[0].Text);
            });
        }

        [Test]
        public void TestRemove()
        {
            var removedTag = new RubyTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "かぜ",
            };

            PrepareHitObject(new Lyric
            {
                Text = "風",
                Language = new CultureInfo(17),
                RubyTags = new List<RubyTag>
                {
                    removedTag
                }
            });

            TriggerHandlerChanged(c => c.Remove(removedTag));

            AssertSelectedHitObject(h =>
            {
                Assert.IsEmpty(h.RubyTags);
            });
        }

        [Test]
        public void TestRemoveAll()
        {
            var removedTag = new RubyTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "か",
            };

            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Language = new CultureInfo(17),
                RubyTags = new List<RubyTag>
                {
                    removedTag,
                    new()
                    {
                        StartIndex = 1,
                        EndIndex = 2,
                        Text = "ら",
                    }
                }
            });

            TriggerHandlerChanged(c => c.RemoveAll(new[] { removedTag }));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(1, h.RubyTags.Count);
            });
        }

        [Test]
        public void TestSetIndex()
        {
            var targetTag = new RubyTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "か",
            };

            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Language = new CultureInfo(17),
                RubyTags = new List<RubyTag>
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
            var targetTag = new RubyTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "か",
            };

            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Language = new CultureInfo(17),
                RubyTags = new List<RubyTag>
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
            var targetTag = new RubyTag
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = "か",
            };

            PrepareHitObject(new Lyric
            {
                Text = "カラオケ",
                Language = new CultureInfo(17),
                RubyTags = new List<RubyTag>
                {
                    targetTag
                }
            });

            TriggerHandlerChanged(c => c.SetText(targetTag, "からおけ"));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual("からおけ", targetTag.Text);
            });
        }
    }
}
