// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics
{
    [TestFixture]
    public class TestSceneSingerToolTip : OsuTestScene
    {
        private SingerToolTip toolTip;

        [SetUp]
        public void SetUp() => Schedule(() =>
        {
            Child = toolTip = new SingerToolTip
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };
            toolTip.Show();
        });

        [Test]
        public void TestDisplayToolTip()
        {
            setTooltip("Test normal singer", singer => { singer.Name = "Normal singer"; });

            setTooltip("Test singer with description", singer =>
            {
                singer.Name = "Singer with description";
                singer.Description = "International superstar vocaloid Hatsune Miku.";
            });

            setTooltip("Test singer with large description", singer =>
            {
                singer.Name = "Singer with large description";
                singer.Description =
                    "International superstar vocaloid Hatsune Miku on Sept 9 assumed her new position as Coronavirus Countermeasure Supporter in the Office for Novel Coronavirus Disease Control of the Japanese government’s Cabinet Secretariat.";
            });

            setTooltip("Test singer with english name", singer =>
            {
                singer.Name = "Singer with English name";
                singer.EnglishName = "Hatsune Miku";
            });

            setTooltip("Test singer with romaji name", singer =>
            {
                singer.Name = "Singer with Romaji name";
                singer.RomajiName = "Hatsune Miku";
            });

            setTooltip("Test singer with large context", singer =>
            {
                singer.Name = "Singer with Romaji name large large large large large large large large large";
                singer.RomajiName = "Hatsune Miku large large large large large large large large large";
                singer.EnglishName = "Hatsune Miku large large large large large large large large large";
                singer.Description =
                    "International superstar vocaloid Hatsune Miku on Sept 9 assumed her new position as Coronavirus Countermeasure Supporter in the Office for Novel Coronavirus Disease Control of the Japanese government’s Cabinet Secretariat.";
            });
        }

        private void setTooltip(string testName, Action<Singer> callBack)
        {
            AddStep(testName, () =>
            {
                var singer = new Singer(1);
                callBack?.Invoke(singer);
                toolTip.SetContent(singer);
            });
        }
    }
}
