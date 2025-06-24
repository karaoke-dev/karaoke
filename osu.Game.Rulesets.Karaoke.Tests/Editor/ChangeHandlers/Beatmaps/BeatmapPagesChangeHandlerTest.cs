// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Beatmaps;
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
        PrepareHitObject(() => TestCaseTagHelper.ParseLyric("[1000,3000]:karaoke"), false);

        TriggerHandlerChanged(c => c.AutoGenerate());

        AssertKaraokeBeatmap(karaokeBeatmap =>
        {
            var pages = karaokeBeatmap.PageInfo.SortedPages;

            Assert.That(pages.Count, Is.EqualTo(2));
            Assert.That(pages[0].Time, Is.EqualTo(1000));
            Assert.That(pages[1].Time, Is.EqualTo(3000));
        });
    }

    [Test]
    public void TestGeneratePageWithInvalidCase()
    {
        // there's no time-info inside.
        PrepareHitObject(() => new Lyric(), false);

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
            Assert.That(pages.Count, Is.EqualTo(1));
            Assert.That(pages[0], Is.EqualTo(page));
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
            Assert.That(pages.Count, Is.EqualTo(1));
            Assert.That(pages[0], Is.EqualTo(secondPage));
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
            Assert.That(pages.Count, Is.EqualTo(1));
            Assert.That(pages[0], Is.EqualTo(secondPage));
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
            Assert.That(firstPage.Time, Is.EqualTo(1000));
            Assert.That(secondPage.Time, Is.EqualTo(0));
        });
    }

    protected override void SetUpKaraokeBeatmap(Action<KaraokeBeatmap> action)
    {
        base.SetUpKaraokeBeatmap(karaokeBeatmap =>
        {
            karaokeBeatmap.PageInfo = new PageInfo();

            action(karaokeBeatmap);
        });
    }
}
