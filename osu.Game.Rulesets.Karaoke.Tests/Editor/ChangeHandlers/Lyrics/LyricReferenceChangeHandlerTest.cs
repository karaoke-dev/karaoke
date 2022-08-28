// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Properties;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Lyrics
{
    public class LyricReferenceChangeHandlerTest : LyricPropertyChangeHandlerTest<LyricReferenceChangeHandler>
    {
        [Test]
        public void TestUpdateReferenceLyric()
        {
            var lyric = new Lyric
            {
                Text = "Referenced lyric"
            };

            PrepareHitObject(lyric, false);

            PrepareHitObject(new Lyric
            {
                Text = "I need the reference lyric."
            });

            TriggerHandlerChanged(c => c.UpdateReferenceLyric(lyric));

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(lyric, h.ReferenceLyric);
                Assert.IsTrue(h.ReferenceLyricConfig is ReferenceLyricConfig);
            });
        }

        [Test]
        public void TestSwitchToReferenceLyricConfig()
        {
            var lyric = new Lyric
            {
                Text = "Referenced lyric"
            };

            PrepareHitObject(new Lyric
            {
                Text = "Lyric",
                ReferenceLyric = lyric
            });

            TriggerHandlerChanged(c => c.SwitchToReferenceLyricConfig());

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(lyric, h.ReferenceLyric);
                Assert.IsTrue(h.ReferenceLyricConfig is ReferenceLyricConfig);
            });
        }

        [Test]
        public void TestSwitchToSyncLyricConfig()
        {
            var lyric = new Lyric
            {
                Text = "Referenced lyric"
            };

            PrepareHitObject(new Lyric
            {
                Text = "Lyric",
                ReferenceLyric = lyric
            });

            TriggerHandlerChanged(c => c.SwitchToSyncLyricConfig());

            AssertSelectedHitObject(h =>
            {
                Assert.AreEqual(lyric, h.ReferenceLyric);
                Assert.IsTrue(h.ReferenceLyricConfig is SyncLyricConfig);
            });
        }

        [Test]
        public void TestAdjustLyricConfig()
        {
            var lyric = new Lyric
            {
                Text = "Referenced lyric"
            };

            PrepareHitObject(new Lyric
            {
                Text = "Lyric",
                ReferenceLyric = lyric,
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
}
