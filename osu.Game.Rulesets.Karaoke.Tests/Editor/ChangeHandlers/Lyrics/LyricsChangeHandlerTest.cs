// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

public partial class LyricsChangeHandlerTest : BaseHitObjectChangeHandlerTest<LyricsChangeHandler, Lyric>
{
    [Test]
    public void TestSplit()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerHandlerChanged(c => c.Split(2));

        AssertHitObjects(objects =>
        {
            var lyrics = objects.ToArray();
            var firstLyric = lyrics.First();
            var secondLyric = lyrics.Last();

            // test property in the first lyric.
            Assert.That(firstLyric.Text, Is.EqualTo("カラ"));
            Assert.That(firstLyric.Order, Is.EqualTo(0));

            // test property in the second lyric.
            Assert.That(secondLyric.Text, Is.EqualTo("オケ"));
            Assert.That(secondLyric.Order, Is.EqualTo(1));
        });
    }

    [Test]
    public void TestCombine()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラ",
            Order = 1,
        }, false);

        PrepareHitObject(() => new Lyric
        {
            Text = "オケ",
            Order = 2,
        });

        PrepareHitObject(() => new Lyric
        {
            Text = "karaoke",
            Order = 3,
        }, false);

        TriggerHandlerChanged(c => c.Combine());

        AssertHitObjects(objects =>
        {
            var lyrics = objects.ToArray();
            Assert.That(lyrics.Length, Is.EqualTo(2));

            var combinedLyric = lyrics.First(x => x.Text == "カラオケ");
            Assert.That(combinedLyric.Order, Is.EqualTo(1));

            var notAffectedLyric = lyrics.First(x => x.Text == "karaoke");
            Assert.That(notAffectedLyric.Order, Is.EqualTo(2));
        });
    }

    [Test]
    public void TestCreateAtPosition()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Order = 1,
        });

        PrepareHitObject(() => new Lyric
        {
            Text = "karaoke",
            Order = 2,
        }, false);

        TriggerHandlerChanged(c => c.CreateAtPosition());

        AssertHitObjects(hitObjects =>
        {
            var lyrics = hitObjects.ToArray();
            Assert.That(lyrics.Length, Is.EqualTo(3));

            var firstLyric = lyrics.First(x => x.Text == "カラオケ");
            Assert.That(firstLyric.Order, Is.EqualTo(1));

            var secondLyric = lyrics.First(x => x.Text == "New lyric");
            Assert.That(secondLyric.Order, Is.EqualTo(2));

            var thirdLyric = lyrics.First(x => x.Text == "karaoke");
            Assert.That(thirdLyric.Order, Is.EqualTo(3));
        });
    }

    [Test]
    public void TestCreateAtLast()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Order = 1,
        });

        PrepareHitObject(() => new Lyric
        {
            Text = "karaoke",
            Order = 2,
        }, false);

        TriggerHandlerChanged(c => c.CreateAtLast());

        AssertHitObjects(hitObjects =>
        {
            var lyrics = hitObjects.ToArray();
            Assert.That(lyrics.Length, Is.EqualTo(3));

            var firstLyric = lyrics.First(x => x.Text == "カラオケ");
            Assert.That(firstLyric.Order, Is.EqualTo(1));

            var secondLyric = lyrics.First(x => x.Text == "karaoke");
            Assert.That(secondLyric.Order, Is.EqualTo(2));

            var thirdLyric = lyrics.First(x => x.Text == "New lyric");
            Assert.That(thirdLyric.Order, Is.EqualTo(3));
        });
    }

    [Test]
    public void TestCreateAtLastWithEmptyBeatmap()
    {
        TriggerHandlerChanged(c => c.CreateAtLast());

        AssertHitObjects(hitObjects =>
        {
            var lyrics = hitObjects.ToArray();
            Assert.That(lyrics.Length, Is.EqualTo(1));

            var addedLyric = lyrics.First(x => x.Text == "New lyric");
            Assert.That(addedLyric.Order, Is.EqualTo(1));
        });
    }

    [Test]
    public void TestAddBelowToSelection()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Order = 1,
        });

        PrepareHitObject(() => new Lyric
        {
            Text = "Last lyric",
            Order = 2,
        }, false);

        TriggerHandlerChanged(c => c.AddBelowToSelection(new Lyric
        {
            Text = "New lyric",
        }));

        AssertHitObjects(hitObjects =>
        {
            var lyrics = hitObjects.ToArray();
            Assert.That(lyrics.Length, Is.EqualTo(3));

            var addedLyric = lyrics.First(x => x.Text == "New lyric");
            Assert.That(addedLyric.Order, Is.EqualTo(2));
        });
    }

    [Test]
    public void TestAddRangeBelowToSelection()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Order = 1,
        });

        PrepareHitObject(() => new Lyric
        {
            Text = "Last lyric",
            Order = 2,
        }, false);

        TriggerHandlerChanged(c => c.AddRangeBelowToSelection(new[]
        {
            new Lyric
            {
                Text = "New lyric",
            },
        }));

        AssertHitObjects(hitObjects =>
        {
            var lyrics = hitObjects.ToArray();
            Assert.That(lyrics.Length, Is.EqualTo(3));

            var addedLyric = lyrics.First(x => x.Text == "New lyric");
            Assert.That(addedLyric.Order, Is.EqualTo(2));
        });
    }

    [Test]
    public void TestRemove()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Order = 1,
        });

        PrepareHitObject(() => new Lyric
        {
            Text = "karaoke",
            Order = 2,
        }, false);

        TriggerHandlerChanged(c => c.Remove());

        AssertHitObjects(hitObjects =>
        {
            var lyrics = hitObjects.ToArray();
            Assert.That(lyrics.Length, Is.EqualTo(1));

            var secondLyric = lyrics.First(x => x.Text == "karaoke");
            Assert.That(secondLyric.Order, Is.EqualTo(1));
        });
    }

    [Test]
    public void TestChangeOrder()
    {
        PrepareHitObject(() => new Lyric
        {
            Text = "カラオケ",
            Order = 1,
        });

        PrepareHitObject(() => new Lyric
        {
            Text = "karaoke",
            Order = 2,
        }, false);

        // move the "カラオケ" lyric next of the "karaoke" lyric.
        TriggerHandlerChanged(c => c.ChangeOrder(1));

        AssertHitObjects(hitObjects =>
        {
            var lyrics = hitObjects.ToArray();
            Assert.That(lyrics.Length, Is.EqualTo(2));

            var firstLyric = lyrics.First(x => x.Text == "karaoke");
            Assert.That(firstLyric.Order, Is.EqualTo(1));

            var secondLyric = lyrics.First(x => x.Text == "カラオケ");
            Assert.That(secondLyric.Order, Is.EqualTo(2));
        });
    }
}
