// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics;

[TestFixture]
public partial class TestSceneSingerToolTip : OsuTestScene
{
    private SingerToolTip toolTip = null!;

    [SetUp]
    public void SetUp() => Schedule(() =>
    {
        Child = toolTip = new SingerToolTip
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
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

        setTooltip("Test singer with romanisation", singer =>
        {
            singer.Name = "Singer with romanisation";
            singer.Romanisation = "Hatsune Miku";
        });

        setTooltip("Test singer with large context", singer =>
        {
            singer.Name = "Singer with romanisation large large large large large large large large large";
            singer.Romanisation = "Hatsune Miku large large large large large large large large large";
            singer.EnglishName = "Hatsune Miku large large large large large large large large large";
            singer.Description =
                "International superstar vocaloid Hatsune Miku on Sept 9 assumed her new position as Coronavirus Countermeasure Supporter in the Office for Novel Coronavirus Disease Control of the Japanese government’s Cabinet Secretariat.";
        });
    }

    private void setTooltip(string testName, Action<Singer> callBack)
    {
        AddStep(testName, () =>
        {
            var singer = new Singer();
            callBack(singer);
            toolTip.SetContent(singer);
        });
    }
}
