// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Beatmaps;

public class BeatmapPagesChangeHandlerTest : BaseChangeHandlerTest<BeatmapPagesChangeHandler>
{
    [Test]
    public void TestAdd()
    {
        Page page = new Page();

        TriggerHandlerChanged(c =>
        {
            c.Add(page);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var pages = karaokeBeatmap.PageInfo.Pages;
            Assert.AreEqual(1, pages.Count);
            Assert.AreEqual(page, pages[0]);
        });
    }

    [Test]
    public void TestRemove()
    {
        Page firstPage = new Page { Time = 1000 };
        Page secondPage = new Page { Time = 2000 };

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var pages = karaokeBeatmap.PageInfo.Pages;
            pages.Add(firstPage);
            pages.Add(secondPage);
        });

        TriggerHandlerChanged(c =>
        {
            c.Remove(firstPage);
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var pages = karaokeBeatmap.PageInfo.Pages;
            Assert.AreEqual(1, pages.Count);

            Assert.AreEqual(secondPage, pages[0]);
        });
    }

    [Test]
    public void TestRemoveRange()
    {
        Page firstPage = new Page { Time = 1000 };
        Page secondPage = new Page { Time = 2000 };

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var pages = karaokeBeatmap.PageInfo.Pages;
            pages.Add(firstPage);
            pages.Add(secondPage);
        });

        TriggerHandlerChanged(c =>
        {
            c.RemoveRange(new[] { firstPage });
        });

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var pages = karaokeBeatmap.PageInfo.Pages;
            Assert.AreEqual(1, pages.Count);

            Assert.AreEqual(secondPage, pages[0]);
        });
    }

    [Test]
    public void TestShiftingPageTime()
    {
        Page firstPage = new Page();
        Page secondPage = new Page();

        SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            var pages = karaokeBeatmap.PageInfo.Pages;
            pages.Add(firstPage);
            pages.Add(secondPage);
        });

        TriggerHandlerChanged(c =>
        {
            c.ShiftingPageTime(new[] { firstPage }, 1000);
        });

        AssertKaraokeBeatmap(_ =>
        {
            Assert.AreEqual(1000, firstPage.Time);
            Assert.AreEqual(0, secondPage.Time);
        });
    }
}
