// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

public partial class LyricTimeTagsChangeHandlerTest : LyricPropertyChangeHandlerTest<LyricTimeTagsChangeHandler>
{
    [Test]
    public void TestSetTimeTagTime()
    {
        var timeTag = new TimeTag(new TextIndex(), 1000);
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                timeTag,
            },
        });

        TriggerHandlerChanged(c => c.SetTimeTagTime(timeTag, 2000));

        AssertSelectedHitObject(_ =>
        {
            Assert.That(timeTag.Time, Is.EqualTo(2000));
        });
    }

    [Test]
    public void TestSetTimeTagFirstSyllable()
    {
        var timeTag = new TimeTag(new TextIndex(), 1000);
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                timeTag,
            },
        });

        TriggerHandlerChanged(c => c.SetTimeTagFirstSyllable(timeTag, true));

        AssertSelectedHitObject(_ =>
        {
            Assert.That(timeTag.FirstSyllable, Is.True);
        });
    }

    [Test]
    public void TestSetTimeTagRomanisedSyllable()
    {
        var timeTag = new TimeTag(new TextIndex(), 1000);
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                timeTag,
            },
        });

        TriggerHandlerChanged(c => c.SetTimeTagRomanisedSyllable(timeTag, "karaoke"));

        AssertSelectedHitObject(_ =>
        {
            Assert.That(timeTag.RomanisedSyllable, Is.EqualTo("karaoke"));
        });

        TriggerHandlerChanged(c => c.SetTimeTagRomanisedSyllable(timeTag, "  "));

        AssertSelectedHitObject(_ =>
        {
            Assert.That(timeTag.RomanisedSyllable, Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public void TestShiftingTimeTagTime()
    {
        var timeTag = new TimeTag(new TextIndex());
        var timeTagWithTime = new TimeTag(new TextIndex(), 1000);
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                timeTag,
                timeTagWithTime,
            },
        });

        TriggerHandlerChanged(c => c.ShiftingTimeTagTime(new[] { timeTag, timeTagWithTime }, 2000));

        // use this temp way to trigger transaction count increase.
        AddStep("Prepare testing beatmap", () =>
        {
            var editorBeatmap = Dependencies.Get<EditorBeatmap>();
            editorBeatmap.PerformOnSelection(h =>
            {
                editorBeatmap.Update(h);
            });
        });

        AssertSelectedHitObject(_ =>
        {
            Assert.That(timeTag.Time, Is.Null);
            Assert.That(timeTagWithTime.Time, Is.EqualTo(3000));
        });
    }

    [Test]
    public void TestClearTimeTagTime()
    {
        var timeTag = new TimeTag(new TextIndex(), 1000);
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                timeTag,
            },
        });

        TriggerHandlerChanged(c => c.ClearTimeTagTime(timeTag));

        AssertSelectedHitObject(_ =>
        {
            Assert.That(timeTag.Time, Is.Null);
        });
    }

    [Test]
    public void TestClearAllTimeTagTime()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex()), // without time.
                new TimeTag(new TextIndex(), 1000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 3000),
            },
        });

        TriggerHandlerChanged(c => c.ClearAllTimeTagTime());

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.TimeTags.All(x => x.Time == null));
        });
    }

    [Test]
    public void TestAdd()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChanged(c => c.Add(new TimeTag(new TextIndex(), 1000)));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.TimeTags.Count, Is.EqualTo(1));
            Assert.That(h.TimeTags[0].Time, Is.EqualTo(1000));
        });
    }

    [Test]
    public void TestAddRange()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChanged(c => c.AddRange(new[] { new TimeTag(new TextIndex(), 1000) }));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.TimeTags.Count, Is.EqualTo(1));
            Assert.That(h.TimeTags[0].Time, Is.EqualTo(1000));
        });
    }

    [Test]
    public void TestRemove()
    {
        var removedTag = new TimeTag(new TextIndex(), 1000);

        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                removedTag,
            },
        });

        TriggerHandlerChanged(c => c.Remove(removedTag));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.TimeTags, Is.Empty);
        });
    }

    [Test]
    public void TestRemoveRange()
    {
        var removedTag = new TimeTag(new TextIndex(), 1000);

        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                removedTag,
            },
        });

        TriggerHandlerChanged(c => c.RemoveRange(new[] { removedTag }));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.TimeTags, Is.Empty);
        });
    }

    [Test]
    public void TestAddByPosition()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChanged(c => c.AddByPosition(new TextIndex(3, TextIndex.IndexState.End)));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.TimeTags.Count, Is.EqualTo(1));

            var actualTimeTag = h.TimeTags[0];
            Assert.That(actualTimeTag.Index, Is.EqualTo(new TextIndex(3, TextIndex.IndexState.End)));
            Assert.That(actualTimeTag.Time, Is.Null);
        });
    }

    [Test]
    public void TestRemoveByPosition()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End)),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 5000),
            },
        });

        TriggerHandlerChanged(c => c.RemoveByPosition(new TextIndex(3, TextIndex.IndexState.End)));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.TimeTags.Count, Is.EqualTo(2));

            // should delete the min time of the time-tag
            var actualTimeTag = h.TimeTags[0];
            Assert.That(actualTimeTag.Index, Is.EqualTo(new TextIndex(3, TextIndex.IndexState.End)));
            Assert.That(actualTimeTag.Time, Is.EqualTo(4000));
        });
    }

    [Test]
    public void TestRemoveByPositionCase2()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 5000),
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End), 4000),
            },
        });

        TriggerHandlerChanged(c => c.RemoveByPosition(new TextIndex(3, TextIndex.IndexState.End)));

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.TimeTags.Count, Is.EqualTo(1));

            // should delete the min time of the time-tag
            var actualTimeTag = h.TimeTags[0];
            Assert.That(actualTimeTag.Index, Is.EqualTo(new TextIndex(3, TextIndex.IndexState.End)));
            Assert.That(actualTimeTag.Time, Is.EqualTo(5000));
        });
    }

    [TestCase(ShiftingDirection.Left, ShiftingType.Index, 1)]
    [TestCase(ShiftingDirection.Left, ShiftingType.State, 2)]
    [TestCase(ShiftingDirection.Right, ShiftingType.State, 2)]
    [TestCase(ShiftingDirection.Right, ShiftingType.Index, 3)]
    public void TestShifting(ShiftingDirection direction, ShiftingType type, int expectedIndex)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(1)),
                new TimeTag(new TextIndex(1, TextIndex.IndexState.End)),
                new TimeTag(new TextIndex(2), 4000), // target.
                new TimeTag(new TextIndex(2, TextIndex.IndexState.End)),
                new TimeTag(new TextIndex(3)),
            },
        });

        TriggerHandlerChanged(c =>
        {
            var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
            var targetTimeTag = lyric.TimeTags[2];
            var actualTimeTag = c.Shifting(targetTimeTag, direction, type);

            Assert.That(lyric.TimeTags.IndexOf(actualTimeTag), Is.EqualTo(expectedIndex));

            // the property should be the same
            Assert.That(actualTimeTag.Time, Is.EqualTo(targetTimeTag.Time));
        });
    }

    [TestCase(ShiftingDirection.Left, ShiftingType.Index, 0)]
    [TestCase(ShiftingDirection.Left, ShiftingType.State, 0)]
    [TestCase(ShiftingDirection.Right, ShiftingType.State, 0)]
    [TestCase(ShiftingDirection.Right, ShiftingType.Index, 0)]
    public void TestShiftingToFirst(ShiftingDirection direction, ShiftingType type, int expectedIndex)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(1)), // target.
                new TimeTag(new TextIndex(3)),
            },
        });

        TriggerHandlerChanged(c =>
        {
            var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
            var targetTimeTag = lyric.TimeTags[0];
            var actualTimeTag = c.Shifting(targetTimeTag, direction, type);

            Assert.That(lyric.TimeTags.IndexOf(actualTimeTag), Is.EqualTo(expectedIndex));

            // the property should be the same
            Assert.That(actualTimeTag.Time, Is.EqualTo(targetTimeTag.Time));
        });
    }

    [TestCase(ShiftingDirection.Left, ShiftingType.Index, 1)]
    [TestCase(ShiftingDirection.Left, ShiftingType.State, 1)]
    [TestCase(ShiftingDirection.Right, ShiftingType.State, 1)]
    [TestCase(ShiftingDirection.Right, ShiftingType.Index, 1)]
    public void TestShiftingToLast(ShiftingDirection direction, ShiftingType type, int expectedIndex)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(0)),
                new TimeTag(new TextIndex(2)), // target.
            },
        });

        TriggerHandlerChanged(c =>
        {
            var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
            var targetTimeTag = lyric.TimeTags[1];
            var actualTimeTag = c.Shifting(targetTimeTag, direction, type);

            Assert.That(lyric.TimeTags.IndexOf(actualTimeTag), Is.EqualTo(expectedIndex));

            // the property should be the same
            Assert.That(actualTimeTag.Time, Is.EqualTo(targetTimeTag.Time));
        });
    }

    [TestCase(ShiftingDirection.Left, ShiftingType.Index, 1)]
    [TestCase(ShiftingDirection.Left, ShiftingType.State, 1)]
    [TestCase(ShiftingDirection.Right, ShiftingType.State, 1)]
    [TestCase(ShiftingDirection.Right, ShiftingType.Index, 1)]
    public void TestShiftingWithNoDuplicatedTimeTag(ShiftingDirection direction, ShiftingType type, int expectedIndex)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(0)),
                new TimeTag(new TextIndex(2), 4000), // target.
                new TimeTag(new TextIndex(3, TextIndex.IndexState.End)),
            },
        });

        TriggerHandlerChanged(c =>
        {
            var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
            var targetTimeTag = lyric.TimeTags[1];
            var actualTimeTag = c.Shifting(targetTimeTag, direction, type);

            Assert.That(lyric.TimeTags.IndexOf(actualTimeTag), Is.EqualTo(expectedIndex));

            // the property should be the same
            Assert.That(actualTimeTag.Time, Is.EqualTo(targetTimeTag.Time));
        });
    }

    [TestCase(ShiftingDirection.Left, ShiftingType.Index, 0)]
    [TestCase(ShiftingDirection.Left, ShiftingType.State, 0)]
    [TestCase(ShiftingDirection.Right, ShiftingType.State, 0)]
    [TestCase(ShiftingDirection.Right, ShiftingType.Index, 0)]
    public void TestShiftingWithOneTimeTag(ShiftingDirection direction, ShiftingType type, int expectedIndex)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(2), 4000), // target.
            },
        });

        TriggerHandlerChanged(c =>
        {
            var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
            var targetTimeTag = lyric.TimeTags[0];
            var actualTimeTag = c.Shifting(targetTimeTag, direction, type);

            Assert.That(lyric.TimeTags.IndexOf(actualTimeTag), Is.EqualTo(expectedIndex));

            // the property should be the same
            Assert.That(actualTimeTag.Time, Is.EqualTo(targetTimeTag.Time));
        });
    }

    [Ignore("Will be implement if it will increase better UX.")]
    [TestCase(ShiftingDirection.Left, ShiftingType.State, 1)]
    [TestCase(ShiftingDirection.Left, ShiftingType.Index, 1)]
    [TestCase(ShiftingDirection.Right, ShiftingType.State, 3)]
    [TestCase(ShiftingDirection.Right, ShiftingType.Index, 3)]
    public void TestShiftingWithSameTimeTag(ShiftingDirection direction, ShiftingType type, int expectedIndex)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(2), 3000),
                new TimeTag(new TextIndex(2), 4000), // target.
                new TimeTag(new TextIndex(2), 5000),
            },
        });

        TriggerHandlerChanged(c =>
        {
            var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
            var targetTimeTag = lyric.TimeTags[1];
            var actualTimeTag = c.Shifting(targetTimeTag, direction, type);

            Assert.That(lyric.TimeTags.IndexOf(actualTimeTag), Is.EqualTo(expectedIndex));

            // the property should be the same
            Assert.That(actualTimeTag.Time, Is.EqualTo(targetTimeTag.Time));
        });
    }

    [TestCase(TextIndex.IndexState.Start, ShiftingDirection.Left, ShiftingType.Index)]
    [TestCase(TextIndex.IndexState.Start, ShiftingDirection.Left, ShiftingType.State)]
    [TestCase(TextIndex.IndexState.Start, ShiftingDirection.Right, ShiftingType.Index)]
    [TestCase(TextIndex.IndexState.End, ShiftingDirection.Left, ShiftingType.Index)]
    [TestCase(TextIndex.IndexState.End, ShiftingDirection.Right, ShiftingType.State)]
    [TestCase(TextIndex.IndexState.End, ShiftingDirection.Right, ShiftingType.Index)]
    public void TestShiftingException(TextIndex.IndexState state, ShiftingDirection direction, ShiftingType type)
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "-",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(0, state), 5000), // target.
            },
        });

        // will have exception because the time-tag cannot move right.
        TriggerHandlerChanged(c =>
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var lyric = Dependencies.Get<EditorBeatmap>().HitObjects.OfType<Lyric>().First();
                var targetTimeTag = lyric.TimeTags[0];
                c.Shifting(targetTimeTag, direction, type);
            });
        });
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestWithReferenceLyric(bool syncTimeTag)
    {
        var lyric = PrepareLyricWithSyncConfig(new Lyric
        {
            Text = "カラオケ",
            TimeTags = new[]
            {
                new TimeTag(new TextIndex(), 1000),
            },
        }, new SyncLyricConfig
        {
            SyncTimeTagProperty = syncTimeTag,
        });

        // should add the time-tag by hand because it does not sync from thr referenced lyric.
        if (!syncTimeTag)
        {
            lyric.TimeTags = new[]
            {
                new TimeTag(new TextIndex(), 2000),
            };
        }

        var timeTag = lyric.TimeTags.First();

        if (syncTimeTag)
        {
            TriggerHandlerChangedWithException<ChangeForbiddenException>(c => c.SetTimeTagTime(timeTag, 2000));
        }
        else
        {
            TriggerHandlerChanged(c => c.SetTimeTagTime(timeTag, 2000));
        }
    }
}
