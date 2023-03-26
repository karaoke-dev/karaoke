// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics;

public partial class LyricReferenceChangeHandlerTest : LyricPropertyChangeHandlerTest<LyricReferenceChangeHandler>
{
    [Test]
    public void TestUpdateReferenceLyric()
    {
        var referencedLyric = new Lyric
        {
            Text = "Referenced lyric"
        };

        PrepareHitObject(referencedLyric, false);

        PrepareHitObject(new Lyric
        {
            Text = "I need the reference lyric."
        });

        TriggerHandlerChanged(c => c.UpdateReferenceLyric(referencedLyric));

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual(referencedLyric, h.ReferenceLyric);
            Assert.IsTrue(h.ReferenceLyricConfig is ReferenceLyricConfig);
        });
    }

    [Test]
    public void TestSwitchToReferenceLyricConfig()
    {
        var referencedLyric = new Lyric
        {
            Text = "Referenced lyric"
        };

        PrepareHitObject(new Lyric
        {
            Text = "Lyric",
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric
        });

        TriggerHandlerChanged(c => c.SwitchToReferenceLyricConfig());

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual(referencedLyric, h.ReferenceLyric);
            Assert.IsTrue(h.ReferenceLyricConfig is ReferenceLyricConfig);
        });
    }

    [Test]
    public void TestSwitchToSyncLyricConfig()
    {
        var referencedLyric = new Lyric
        {
            Text = "Referenced lyric"
        };

        PrepareHitObject(new Lyric
        {
            Text = "Lyric",
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric
        });

        TriggerHandlerChanged(c => c.SwitchToSyncLyricConfig());

        AssertSelectedHitObject(h =>
        {
            Assert.AreEqual(referencedLyric, h.ReferenceLyric);
            Assert.IsTrue(h.ReferenceLyricConfig is SyncLyricConfig);
        });
    }

    [Test]
    public void TestAdjustLyricConfig()
    {
        var referencedLyric = new Lyric
        {
            Text = "Referenced lyric"
        };

        PrepareHitObject(new Lyric
        {
            Text = "Lyric",
            ReferenceLyricId = referencedLyric.ID,
            ReferenceLyric = referencedLyric,
            ReferenceLyricConfig = new SyncLyricConfig(),
        });

        TriggerHandlerChanged(c => c.AdjustLyricConfig<SyncLyricConfig>(x =>
        {
            x.OffsetTime = 100;
            x.SyncSingerProperty = false;
        }));

        AssertSelectedHitObject(h =>
        {
            var config = (h.ReferenceLyricConfig as SyncLyricConfig)!;
            Assert.AreEqual(100, config.OffsetTime);
            Assert.AreEqual(false, config.SyncSingerProperty);
        });
    }

    [Test]
    public void TestWithReferenceLyric()
    {
        var lyric = new Lyric
        {
            Text = "Referenced lyric"
        };

        PrepareHitObject(lyric, false);
        PrepareLyricWithSyncConfig(new Lyric());

        // should not block the reference language change.
        TriggerHandlerChanged(c => c.UpdateReferenceLyric(lyric));
        TriggerHandlerChanged(c => c.SwitchToReferenceLyricConfig());
        TriggerHandlerChanged(c => c.SwitchToSyncLyricConfig());
        TriggerHandlerChanged(c => c.AdjustLyricConfig<SyncLyricConfig>(syncLyricConfig => syncLyricConfig.OffsetTime = 1000));
    }
}
