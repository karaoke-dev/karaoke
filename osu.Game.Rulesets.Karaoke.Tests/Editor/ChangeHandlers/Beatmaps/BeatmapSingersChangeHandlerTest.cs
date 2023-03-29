// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Beatmaps;

public partial class BeatmapSingersChangeHandlerTest : BaseChangeHandlerTest<BeatmapSingersChangeHandler>
{
    [Test]
    [Ignore("Not working because singer in karaoke beatmap only sync to change handler once.")]
    public void TestChangeOrder()
    {
        Singer firstSinger = null!;
        Singer secondSinger = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            firstSinger = karaokeBeatmap.SingerInfo.AddSinger(s => s.Order = 1);
            secondSinger = karaokeBeatmap.SingerInfo.AddSinger(s => s.Order = 2);
        });

        TriggerHandlerChanged(c =>
        {
            c.ChangeOrder(firstSinger, 2);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            Assert.AreEqual(1, firstSinger.ID);
            Assert.AreEqual(2, firstSinger.Order);

            Assert.AreEqual(2, secondSinger.ID);
            Assert.AreEqual(1, secondSinger.Order);
        });
    }

    [Test]
    [Ignore("It's hard to test this because it needs lots of dependencies.")]
    public void TestChangeSingerAvatar()
    {
    }

    [Test]
    [Ignore("Not working because singer in karaoke beatmap only sync to change handler once.")]
    public void TestAdd()
    {
        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            karaokeBeatmap.SingerInfo.AddSinger(s => s.Order = 1);
            karaokeBeatmap.SingerInfo.AddSinger(s => s.Order = 2);
        });

        TriggerHandlerChanged(c =>
        {
            c.Add();
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var singers = karaokeBeatmap.SingerInfo.Singers;
            Assert.AreEqual(3, singers.Count);

            var lastSinger = singers.Last();
            Assert.AreEqual(2, lastSinger.ID);
            Assert.AreEqual(3, lastSinger.Order);
        });
    }

    [Test]
    [Ignore("Not working because singer in karaoke beatmap only sync to change handler once.")]
    public void TestRemove()
    {
        Singer firstSinger = null!;
        Singer secondSinger = null!;

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            firstSinger = karaokeBeatmap.SingerInfo.AddSinger(s => s.Order = 1);
            secondSinger = karaokeBeatmap.SingerInfo.AddSinger(s => s.Order = 2);

            karaokeBeatmap.HitObjects = new List<KaraokeHitObject>
            {
                new Lyric
                {
                    SingerIds = { firstSinger.ID }
                }
            };
        });

        TriggerHandlerChanged(c =>
        {
            c.Remove(firstSinger);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var singers = karaokeBeatmap.SingerInfo.Singers;
            Assert.AreEqual(1, singers.Count);

            Assert.AreEqual(1, secondSinger.ID);
            Assert.AreEqual(1, secondSinger.Order);

            var lyrics = karaokeBeatmap.HitObjects.OfType<Lyric>().Where(x => x.SingerIds.Contains(firstSinger.ID));
            Assert.IsEmpty(lyrics);
        });
    }
}