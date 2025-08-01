// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

public partial class LyricSingerChangeHandlerTest : LyricPropertyChangeHandlerTest<LyricSingerChangeHandler>
{
    [Test]
    public void TestAdd()
    {
        Singer singer = null!;
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            singer = karaokeBeatmap.SingerInfo.AddSinger(s =>
            {
                s.Name = "Singer1";
            });
        });
        PrepareHitObject(() => new Lyric());

        TriggerHandlerChanged(c => c.Add(singer));

        AssertSelectedHitObject(h =>
        {
            var singers = h.SingerIds;
            Assert.That(singers.Count, Is.EqualTo(1));
            Assert.That(singers.FirstOrDefault(), Is.EqualTo(singer.ID));
        });
    }

    [Test]
    public void TestAddRange()
    {
        Singer singer = null!;
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            singer = karaokeBeatmap.SingerInfo.AddSinger(s =>
            {
                s.Name = "Singer1";
            });
        });
        PrepareHitObject(() => new Lyric());

        TriggerHandlerChanged(c => c.AddRange(new[] { singer }));

        AssertSelectedHitObject(h =>
        {
            var singers = h.SingerIds;
            Assert.That(singers.Count, Is.EqualTo(1));
            Assert.That(singers.FirstOrDefault(), Is.EqualTo(singer.ID));
        });
    }

    [Test]
    public void TestRemove()
    {
        Singer singer = null!;
        Singer anotherSinger = null!;
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            singer = karaokeBeatmap.SingerInfo.AddSinger(s =>
            {
                s.Name = "Singer1";
            });

            anotherSinger = karaokeBeatmap.SingerInfo.AddSinger(s =>
            {
                s.Name = "Another singer";
            });
        });
        PrepareHitObject(() => new Lyric
        {
            SingerIds = new[]
            {
                singer.ID,
                anotherSinger.ID,
            },
        });

        TriggerHandlerChanged(c => c.Remove(singer));

        AssertSelectedHitObject(h =>
        {
            var singers = h.SingerIds;

            // should not contains removed singer.
            Assert.That(singers.Contains(singer.ID), Is.False);
            // should only contain remain singer。
            Assert.That(singers.Count, Is.EqualTo(1));
            Assert.That(singers.Contains(anotherSinger.ID), Is.True);
        });
    }

    [Test]
    public void TestRemoveRange()
    {
        Singer singer = null!;
        Singer anotherSinger = null!;
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            singer = karaokeBeatmap.SingerInfo.AddSinger(s =>
            {
                s.Name = "Singer1";
            });

            anotherSinger = karaokeBeatmap.SingerInfo.AddSinger(s =>
            {
                s.Name = "Another singer";
            });
        });
        PrepareHitObject(() => new Lyric
        {
            SingerIds = new[]
            {
                singer.ID,
                anotherSinger.ID,
            },
        });

        TriggerHandlerChanged(c => c.RemoveRange(new[] { singer }));

        AssertSelectedHitObject(h =>
        {
            var singers = h.SingerIds;

            // should not contains removed singer.
            Assert.That(singers.Contains(singer.ID), Is.False);
            // should only contain remain singer。
            Assert.That(singers.Count, Is.EqualTo(1));
            Assert.That(singers.Contains(anotherSinger.ID), Is.True);
        });
    }

    [Test]
    public void TestClear()
    {
        Singer singer = null!;
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            singer = karaokeBeatmap.SingerInfo.AddSinger(s =>
            {
                s.Name = "Singer1";
            });
        });
        PrepareHitObject(() => new Lyric
        {
            SingerIds = new[]
            {
                singer.ID,
            },
        });

        TriggerHandlerChanged(c => c.Clear());

        AssertSelectedHitObject(h =>
        {
            Assert.That(h.SingerIds, Is.Empty);
        });
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestWithReferenceLyric(bool syncSinger)
    {
        Singer singer = null!;
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            singer = karaokeBeatmap.SingerInfo.AddSinger(s =>
            {
                s.Name = "Singer1";
            });
        });
        PrepareLyricWithSyncConfig(new Lyric(), new SyncLyricConfig
        {
            SyncSingerProperty = syncSinger,
        });

        if (syncSinger)
        {
            TriggerHandlerChangedWithException<ChangeForbiddenException>(c => c.Add(singer));
        }
        else
        {
            TriggerHandlerChanged(c => c.Add(singer));
        }
    }
}
