// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Edit
{
    [TestFixture]
    public class TestSceneInvalidLyricToolTip : OsuTestScene
    {
        private InvalidLyricToolTip toolTip;

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = toolTip = new InvalidLyricToolTip
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
            toolTip.Show();
        });

        [Test]
        public void TestValidLyric()
        {
            setTooltip("valid lyric");
        }

        [Test]
        public void TestTimeInvalidLyric()
        {
            setTooltip("overlapping time", new TestLyricTimeIssue(new[]
            {
                TimeInvalid.Overlapping,
            }));

            setTooltip("start time invalid", new TestLyricTimeIssue(new[]
            {
                TimeInvalid.StartTimeInvalid,
            }));

            setTooltip("end time invalid", new TestLyricTimeIssue(new[]
            {
                TimeInvalid.EndTimeInvalid,
            }));
        }

        [Test]
        public void TestRubyTagInvalidLyric()
        {
            setTooltip("ruby tag out of range", new TestRubyTagIssue(new Dictionary<RubyTagInvalid, RubyTag[]>
            {
                {
                    RubyTagInvalid.OutOfRange,
                    new[]
                    {
                        new RubyTag
                        {
                            StartIndex = 2,
                            EndIndex = 3,
                            Text = "Invalid ruby"
                        }
                    }
                },
            }));

            setTooltip("ruby tag out of range", new TestRubyTagIssue(new Dictionary<RubyTagInvalid, RubyTag[]>
            {
                {
                    RubyTagInvalid.Overlapping,
                    new[]
                    {
                        new RubyTag
                        {
                            StartIndex = 2,
                            EndIndex = 3,
                            Text = "Invalid ruby"
                        }
                    }
                }
            }));

            setTooltip("ruby tag text is empty", new TestRubyTagIssue(new Dictionary<RubyTagInvalid, RubyTag[]>
            {
                {
                    RubyTagInvalid.Overlapping,
                    new[]
                    {
                        new RubyTag
                        {
                            StartIndex = 2,
                            EndIndex = 3,
                            Text = null,
                        }
                    }
                }
            }));
        }

        [Test]
        public void TestRomajiTagInvalidLyric()
        {
            setTooltip("romaji tag out of range", new TestRomajiTagIssue(new Dictionary<RomajiTagInvalid, RomajiTag[]>
            {
                {
                    RomajiTagInvalid.OutOfRange,
                    new[]
                    {
                        new RomajiTag
                        {
                            StartIndex = 2,
                            EndIndex = 3,
                            Text = "Invalid romaji"
                        }
                    }
                },
            }));

            setTooltip("romaji tag out of range", new TestRomajiTagIssue(new Dictionary<RomajiTagInvalid, RomajiTag[]>
            {
                {
                    RomajiTagInvalid.Overlapping,
                    new[]
                    {
                        new RomajiTag
                        {
                            StartIndex = 2,
                            EndIndex = 3,
                            Text = "Invalid romaji"
                        }
                    }
                }
            }));

            setTooltip("romaji tag text is empty", new TestRomajiTagIssue(new Dictionary<RomajiTagInvalid, RomajiTag[]>
            {
                {
                    RomajiTagInvalid.Overlapping,
                    new[]
                    {
                        new RomajiTag
                        {
                            StartIndex = 2,
                            EndIndex = 3,
                            Text = null,
                        }
                    }
                }
            }));
        }

        [Test]
        public void TestTimeTagInvalidLyric()
        {
            setTooltip("time tag out of range", new TestTimeTagIssue(
                new Dictionary<TimeTagInvalid, TimeTag[]>
                {
                    {
                        TimeTagInvalid.OutOfRange,
                        new[]
                        {
                            new TimeTag(new TextIndex(2))
                        }
                    },
                }));

            setTooltip("time tag overlapping", new TestTimeTagIssue(new Dictionary<TimeTagInvalid, TimeTag[]>
            {
                {
                    TimeTagInvalid.Overlapping,
                    new[]
                    {
                        new TimeTag(new TextIndex(2))
                    }
                }
            }));

            setTooltip("time tag with no time", new TestTimeTagIssue(new Dictionary<TimeTagInvalid, TimeTag[]>
            {
                {
                    TimeTagInvalid.EmptyTime,
                    new[]
                    {
                        new TimeTag(new TextIndex(2))
                    }
                }
            }));

            setTooltip("missing start time-tag", new TestTimeTagIssue(null, true));
            setTooltip("missing end time-tag", new TestTimeTagIssue(null, false, true));
            setTooltip("missing start and end time-tag", new TestTimeTagIssue(null, true, true));
        }

        [Test]
        public void TestMultiInvalidLyric()
        {
            setTooltip("multi property is invalid", new TestLyricTimeIssue(new[]
            {
                TimeInvalid.Overlapping,
                TimeInvalid.StartTimeInvalid,
            }), new TestRubyTagIssue(new Dictionary<RubyTagInvalid, RubyTag[]>
            {
                {
                    RubyTagInvalid.Overlapping,
                    new[]
                    {
                        new RubyTag
                        {
                            StartIndex = 2,
                            EndIndex = 3,
                            Text = "か"
                        },
                        new RubyTag
                        {
                            StartIndex = 2,
                            EndIndex = 3,
                            Text = "ら"
                        }
                    }
                }
            }), new TestRomajiTagIssue(new Dictionary<RomajiTagInvalid, RomajiTag[]>
            {
                {
                    RomajiTagInvalid.OutOfRange,
                    new[]
                    {
                        new RomajiTag
                        {
                            StartIndex = 2,
                            EndIndex = 3,
                            Text = "ka"
                        },
                        new RomajiTag
                        {
                            StartIndex = 4,
                            EndIndex = 5,
                            Text = "ra"
                        },
                        new RomajiTag
                        {
                            StartIndex = 5,
                            EndIndex = 6,
                            Text = "o"
                        },
                        new RomajiTag
                        {
                            StartIndex = 6,
                            EndIndex = 7,
                            Text = "ke"
                        }
                    }
                },
            }), new TestTimeTagIssue(new Dictionary<TimeTagInvalid, TimeTag[]>
            {
                {
                    TimeTagInvalid.OutOfRange,
                    new[]
                    {
                        new TimeTag(new TextIndex(2))
                    }
                },
            }));
        }

        private void setTooltip(string testName, params Issue[] issues)
        {
            AddStep(testName, () =>
            {
                toolTip.SetContent(issues);
            });
        }

        internal class TestLyricTimeIssue : LyricTimeIssue
        {
            public TestLyricTimeIssue(TimeInvalid[] invalidLyricTime)
                : base(new Lyric(), null, invalidLyricTime)
            {
            }
        }

        internal class TestRubyTagIssue : RubyTagIssue
        {
            public TestRubyTagIssue(Dictionary<RubyTagInvalid, RubyTag[]> invalidRubyTags)
                : base(new Lyric(), null, invalidRubyTags)
            {
            }
        }

        internal class TestRomajiTagIssue : RomajiTagIssue
        {
            public TestRomajiTagIssue(Dictionary<RomajiTagInvalid, RomajiTag[]> invalidRomajiTags)
                : base(new Lyric(), null, invalidRomajiTags)
            {
            }
        }

        internal class TestTimeTagIssue : TimeTagIssue
        {
            public TestTimeTagIssue(Dictionary<TimeTagInvalid, TimeTag[]> invalidTimeTags, bool missingStartTimeTag = false, bool missingEndTimeTag = false)
                : base(new Lyric(), null, invalidTimeTags, missingStartTimeTag, missingEndTimeTag)
            {
            }
        }
    }
}
