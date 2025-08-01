// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

public partial class LyricRubyTagsChangeHandlerTest : LyricPropertyChangeHandlerTest<LyricRubyTagsChangeHandler>
{
    [Test]
    public void TestAdd()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "風",
            Language = new CultureInfo(17),
        });

        TriggerHandlerChanged(c => c.Add(new RubyTag
        {
            StartIndex = 0,
            EndIndex = 0,
            Text = "かぜ",
        }));

        AssertSelectedHitObject(h =>
        {
            var rubyTags = h.RubyTags;
            Assert.That(rubyTags.Count, Is.EqualTo(1));
            Assert.That(rubyTags[0].Text, Is.EqualTo("かぜ"));
        });
    }

    [Test]
    public void TestAddRange()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "風",
            Language = new CultureInfo(17),
        });

        TriggerHandlerChanged(c => c.AddRange(new[]
        {
            new RubyTag
            {
                StartIndex = 0,
                EndIndex = 0,
                Text = "かぜ",
            },
        }));

        AssertSelectedHitObject(h =>
        {
            var rubyTags = h.RubyTags;
            Assert.That(rubyTags.Count, Is.EqualTo(1));
            Assert.That(rubyTags[0].Text, Is.EqualTo("かぜ"));
        });
    }

    [Test]
    public void TestRemove()
    {
        var removedTag = new RubyTag
        {
            StartIndex = 0,
            EndIndex = 0,
            Text = "かぜ",
        };

        PrepareHitObject(() => new Lyric
        {
            Text = "風",
            Language = new CultureInfo(17),
            RubyTags = new List<RubyTag>
            {
                removedTag,
            },
        });

        TriggerHandlerChanged(c => c.Remove(removedTag));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.RubyTags, Is.Empty);
        });
    }

    [Test]
    public void TestRemoveRange()
    {
        var removedTag = new RubyTag
        {
            StartIndex = 0,
            EndIndex = 0,
            Text = "か",
        };

        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Language = new CultureInfo(17),
            RubyTags = new List<RubyTag>
            {
                removedTag,
                new()
                {
                    StartIndex = 1,
                    EndIndex = 1,
                    Text = "ら",
                },
            },
        });

        TriggerHandlerChanged(c => c.RemoveRange(new[] { removedTag }));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.RubyTags.Count, Is.EqualTo(1));
        });
    }

    [Test]
    public void TestSetIndex()
    {
        var targetTag = new RubyTag
        {
            StartIndex = 0,
            EndIndex = 0,
            Text = "か",
        };

        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Language = new CultureInfo(17),
            RubyTags = new List<RubyTag>
            {
                targetTag,
            },
        });

        TriggerHandlerChanged(c => c.SetIndex(targetTag, 1, 2));

        AssertSelectedHitObject(h =>
        {
            Assert.That(targetTag.StartIndex, Is.EqualTo(1));
            Assert.That(targetTag.EndIndex, Is.EqualTo(2));
        });
    }

    [Test]
    public void TestShiftingIndex()
    {
        var targetTag = new RubyTag
        {
            StartIndex = 0,
            EndIndex = 0,
            Text = "か",
        };

        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Language = new CultureInfo(17),
            RubyTags = new List<RubyTag>
            {
                targetTag,
            },
        });

        TriggerHandlerChanged(c => c.ShiftingIndex(new[] { targetTag }, 1));

        AssertSelectedHitObject(h =>
        {
            Assert.That(targetTag.StartIndex, Is.EqualTo(1));
            Assert.That(targetTag.EndIndex, Is.EqualTo(1));
        });
    }

    [Test]
    public void TestSetText()
    {
        var targetTag = new RubyTag
        {
            StartIndex = 0,
            EndIndex = 0,
            Text = "か",
        };

        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Language = new CultureInfo(17),
            RubyTags = new List<RubyTag>
            {
                targetTag,
            },
        });

        TriggerHandlerChanged(c => c.SetText(targetTag, "からおけ"));

        AssertSelectedHitObject(h =>
        {
            Assert.That(targetTag.Text, Is.EqualTo("からおけ"));
        });
    }

    [Test]
    public void TestWithReferenceLyric()
    {
        PrepareLyricWithSyncConfig(new Lyric
        {
            Text = "風",
            Language = new CultureInfo(17),
        });

        TriggerHandlerChangedWithException<ChangeForbiddenException>(c => c.Add(new RubyTag
        {
            StartIndex = 0,
            EndIndex = 0,
            Text = "かぜ",
        }));
    }
}
