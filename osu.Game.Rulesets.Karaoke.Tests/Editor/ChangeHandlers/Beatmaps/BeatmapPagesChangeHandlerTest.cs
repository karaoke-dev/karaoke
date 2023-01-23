// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers.Beatmaps;

public partial class BeatmapPagesChangeHandlerTest : BaseChangeHandlerTest<BeatmapPagesChangeHandler>
{
    protected override bool IncludeAutoGenerator => true;

    [Test]
    public void TestGeneratePage()
    {
        PrepareHitObject(TestCaseTagHelper.ParseLyric("[1000,3000]:karaoke"), false);

        TriggerHandlerChanged(c => c.AutoGenerate());

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var pages = karaokeBeatmap.PageInfo.SortedPages;

            Assert.AreEqual(2, pages.Count);

            Assert.AreEqual(1000, pages[0].Time);
            Assert.AreEqual(3000, pages[1].Time);
        });
    }

    [Test]
    public void TestGeneratePageWithInvalidCase()
    {
        // there's no time-info inside.
        PrepareHitObject(new Lyric(), false);

        TriggerHandlerChangedWithException<GeneratorNotSupportedException>(c => c.AutoGenerate());
    }

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
