// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics;
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
            setTooltip("valid lyric", new LyricCheckReport());
        }

        [Test]
        public void TestTimeInvalidLyric()
        {
            setTooltip("overlapping time", new LyricCheckReport
            {
                TimeInvalid = new[]
                {
                    TimeInvalid.Overlapping,
                }
            });

            setTooltip("start time invalid", new LyricCheckReport
            {
                TimeInvalid = new[]
                {
                    TimeInvalid.StartTimeInvalid,
                }
            });

            setTooltip("end time invalid", new LyricCheckReport
            {
                TimeInvalid = new[]
                {
                    TimeInvalid.EndTimeInvalid,
                }
            });
        }

        [Test]
        public void TestTimeTagInvalidLyric()
        {
            setTooltip("time tag out of range", new LyricCheckReport
            {
                InvalidTimeTags = new Dictionary<TimeTagInvalid, TimeTag[]>
                {
                    {
                        TimeTagInvalid.OutOfRange,
                        new []
                        {
                            new TimeTag(new TextIndex(2, TextIndex.IndexState.Start))
                        }
                    },
                }
            });

            setTooltip("time tag out of range", new LyricCheckReport
            {
                InvalidTimeTags = new Dictionary<TimeTagInvalid, TimeTag[]>
                {
                    {
                        TimeTagInvalid.Overlapping,
                        new []
                        {
                            new TimeTag(new TextIndex(2, TextIndex.IndexState.Start))
                        }
                    }
                }
            });
        }

        [Test]
        public void TestRubyTagInvalidLyric()
        {
            setTooltip("ruby tag out of range", new LyricCheckReport
            {
                InvalidRubyTags = new Dictionary<RubyTagInvalid, RubyTag[]>
                {
                    {
                        RubyTagInvalid.OutOfRange,
                        new []
                        {
                            new RubyTag
                            {
                                StartIndex = 2,
                                EndIndex = 3,
                                Text = "Invalid ruby"
                            }
                        }
                    },
                }
            });

            setTooltip("ruby tag out of range", new LyricCheckReport
            {
                InvalidRubyTags = new Dictionary<RubyTagInvalid, RubyTag[]>
                {
                    {
                        RubyTagInvalid.Overlapping,
                        new []
                        {
                            new RubyTag
                            {
                                StartIndex = 2,
                                EndIndex = 3,
                                Text = "Invalid ruby"
                            }
                        }
                    }
                }
            });
        }

        [Test]
        public void TestRomajiTagInvalidLyric()
        {
            setTooltip("romaji tag out of range", new LyricCheckReport
            {
                InvalidRomajiTags = new Dictionary<RomajiTagInvalid, RomajiTag[]>
                {
                    {
                        RomajiTagInvalid.OutOfRange,
                        new []
                        {
                            new RomajiTag
                            {
                                StartIndex = 2,
                                EndIndex = 3,
                                Text = "Invalid romaji"
                            }
                        }
                    },
                }
            });

            setTooltip("romaji tag out of range", new LyricCheckReport
            {
                InvalidRomajiTags = new Dictionary<RomajiTagInvalid, RomajiTag[]>
                {
                    {
                        RomajiTagInvalid.Overlapping,
                        new []
                        {
                            new RomajiTag
                            {
                                StartIndex = 2,
                                EndIndex = 3,
                                Text = "Invalid romaji"
                            }
                        }
                    }
                }
            });
        }

        [Test]
        public void TestMultiInvalidLyric()
        {
            setTooltip("multi property is invalid", new LyricCheckReport
            {
                TimeInvalid = new[]
                {
                    TimeInvalid.Overlapping,
                    TimeInvalid.StartTimeInvalid,
                },
                InvalidTimeTags = new Dictionary<TimeTagInvalid, TimeTag[]>
                {
                    {
                        TimeTagInvalid.OutOfRange,
                        new []
                        {
                            new TimeTag(new TextIndex(2, TextIndex.IndexState.Start))
                        }
                    },
                },
                InvalidRubyTags = new Dictionary<RubyTagInvalid, RubyTag[]>
                {
                    {
                        RubyTagInvalid.Overlapping,
                        new []
                        {
                            new RubyTag
                            {
                                StartIndex = 2,
                                EndIndex = 3,
                                Text = "Invalid ruby"
                            }
                        }
                    }
                },
                InvalidRomajiTags = new Dictionary<RomajiTagInvalid, RomajiTag[]>
                {
                    {
                        RomajiTagInvalid.OutOfRange,
                        new []
                        {
                            new RomajiTag
                            {
                                StartIndex = 2,
                                EndIndex = 3,
                                Text = "Invalid romaji"
                            }
                        }
                    },
                }
            });
        }

        private void setTooltip(string testName, LyricCheckReport timeTag)
        {
            AddStep(testName, () =>
            {
                toolTip.SetContent(timeTag);
            });
        }
    }
}
