// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics.Sprites.Processor;

public partial class TestSceneLyricFirstDisplayProcessor : TestSceneDisplayProcessor
{
    protected override LyricDisplayType DisplayType => LyricDisplayType.Lyric;

    #region Happy path

    [Test]
    public void TestTextChanged()
    {
        Initialize();
        TriggerChange(lyric =>
        {
            lyric.Text = "カラオケ";
        });

        AssertTopTextChanged(Array.Empty<PositionText>());
        AssertCenterTextChanged("カラオケ");
        AssertBottomTextChanged(Array.Empty<PositionText>());
        AssertTimeTagsChanged(new Dictionary<double, TextIndex>());
    }

    [Test]
    public void TestRubiesChanged()
    {
        Initialize(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerChange(lyric =>
        {
            lyric.RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" });
        });

        AssertTopTextChanged(new PositionText[]
        {
            new("か", 0, 0),
            new("ら", 1, 1),
            new("お", 2, 2),
            new("け", 3, 3),
        });
        AssertCenterTextNotChanged();
        AssertBottomTextNotChanged();
        AssertTimeTagsNotChanged();
    }

    [Test]
    public void TestTimeTagsChanged()
    {
        Initialize(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerChange(lyric =>
        {
            lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000#^ka", "[1,start]:2000#ra", "[2,start]:3000#^o", "[3,start]:4000#ke", "[3,end]:5000" });
        });

        AssertTopTextNotChanged();
        AssertCenterTextNotChanged();
        AssertBottomTextChanged(new PositionText[]
        {
            new("kara", 0, 1),
            new("oke", 2, 3),
        });
        AssertTimeTagsChanged(new Dictionary<double, TextIndex>
        {
            { 1000, new TextIndex(0) },
            { 2000, new TextIndex(1) },
            { 3000, new TextIndex(2) },
            { 4000, new TextIndex(3) },
            { 5000, new TextIndex(3, TextIndex.IndexState.End) },
        });
    }

    #endregion

    #region With empty lyric

    [Test]
    public void TestRubiesChangedWithEmptyText()
    {
        Initialize();

        TriggerChange(lyric =>
        {
            lyric.RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" });
        });

        // it's OK not to filter the ruby that out of range. karaoke/sprite text will not display that.
        AssertTopTextChanged(new PositionText[]
        {
            new("か", 0, 0),
            new("ら", 1, 1),
            new("お", 2, 2),
            new("け", 3, 3),
        });
        AssertCenterTextNotChanged();
        AssertBottomTextNotChanged();
        AssertTimeTagsNotChanged();
    }

    [Test]
    public void TestTimeTagsChangedWithEmptyText()
    {
        Initialize();

        TriggerChange(lyric =>
        {
            lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000#^ka", "[1,start]:2000#ra", "[2,start]:3000#o", "[3,start]:4000#ke", "[3,end]:5000" });
        });

        AssertTopTextNotChanged();
        AssertCenterTextNotChanged();
        // it's OK not to filter the romanisation that out of range. karaoke/sprite text will not display that.
        AssertBottomTextChanged(new PositionText[]
        {
            new("karaoke", 0, 3),
        });
        // it's OK not to filter the time-tag that out of range. karaoke/sprite text will not display that.
        AssertTimeTagsChanged(new Dictionary<double, TextIndex>
        {
            { 1000, new TextIndex(0) },
            { 2000, new TextIndex(1) },
            { 3000, new TextIndex(2) },
            { 4000, new TextIndex(3) },
            { 5000, new TextIndex(3, TextIndex.IndexState.End) },
        });
    }

    #endregion
}
