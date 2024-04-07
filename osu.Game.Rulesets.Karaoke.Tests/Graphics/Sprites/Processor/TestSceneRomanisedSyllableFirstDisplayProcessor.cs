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

public partial class TestSceneRomanisedSyllableFirstDisplayProcessor : TestSceneDisplayProcessor
{
    protected override LyricDisplayType DisplayType => LyricDisplayType.RomanisedSyllable;

    #region Happy path

    [Test]
    public void TestTextChanged()
    {
        Initialize(() => new Lyric
        {
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000#^ka", "[1,start]:2000#ra", "[2,start]:3000#o", "[3,start]:4000#ke", "[3,end]:5000" }),
        });
        TriggerChange(lyric =>
        {
            lyric.Text = "カラオケ";
        });

        AssertTopTextNotChanged();
        AssertCenterTextNotChanged();
        // todo: implementation needed.
        AssertBottomTextChanged(Array.Empty<PositionText>());
        AssertTimeTagsNotChanged();
    }

    [Test]
    public void TestRubiesChanged()
    {
        Initialize(() => new Lyric
        {
            Text = "カラオケ",
            TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000#^ka", "[1,start]:2000#ra", "[2,start]:3000#o", "[3,start]:4000#ke", "[3,end]:5000" }),
        });

        TriggerChange(lyric =>
        {
            lyric.RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" });
        });

        // todo: implementation needed.
        AssertTopTextChanged(Array.Empty<PositionText>());
        AssertCenterTextNotChanged();
        AssertBottomTextNotChanged();
        AssertTimeTagsNotChanged();
    }

    [Test]
    public void TestTimeTagsChanged()
    {
        Initialize();

        TriggerChange(lyric =>
        {
            lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000#^ka", "[1,start]:2000#ra", "[2,start]:3000#o", "[3,start]:4000#ke", "[3,end]:5000" });
        });

        AssertTopTextChanged(Array.Empty<PositionText>());
        AssertCenterTextChanged("karaoke");
        // todo: implementation needed.
        AssertBottomTextChanged(Array.Empty<PositionText>());
        // todo: implementation needed.
        AssertTimeTagsChanged(new Dictionary<double, TextIndex>());
    }

    #endregion

    #region With empty time-tags

    [Test]
    public void TestTextChangedWithNoTimeTags()
    {
        Initialize();
        TriggerChange(lyric =>
        {
            lyric.Text = "カラオケ";
        });

        AssertTopTextNotChanged();
        AssertCenterTextNotChanged();
        // todo: implementation needed.
        AssertBottomTextChanged(Array.Empty<PositionText>());
        AssertTimeTagsNotChanged();
    }

    [Test]
    public void TestRubiesChangedWithNoTimeTags()
    {
        Initialize(() => new Lyric
        {
            Text = "カラオケ",
        });

        TriggerChange(lyric =>
        {
            lyric.RubyTags = TestCaseTagHelper.ParseRubyTags(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" });
        });

        AssertTopTextChanged(Array.Empty<PositionText>());
        AssertCenterTextNotChanged();
        AssertBottomTextNotChanged();
        AssertTimeTagsNotChanged();
    }

    #endregion
}
