// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics.Sprites;

public class DisplayLyricProcessorTest
{
    // check changed count
    private int topTextChangeCount;
    private int centerTextChangeCount;
    private int bottomTextChangeCount;
    private int timeTagsChangeCount;

    private Lyric? lyric;
    private DisplayLyricProcessor? testProcessor;

    [SetUp]
    public void Setup()
    {
        // create processor
        lyric = new Lyric();
        testProcessor = new DisplayLyricProcessor(lyric);

        // bind event
        testProcessor.TopTextChanged += _ =>
        {
            topTextChangeCount++;
        };
        testProcessor.CenterTextChanged += _ =>
        {
            centerTextChangeCount++;
        };
        testProcessor.BottomTextChanged += _ =>
        {
            bottomTextChangeCount++;
        };
        testProcessor.TimeTagsChanged += _ =>
        {
            timeTagsChangeCount++;
        };

        // should trigger update all first after bind event.
        testProcessor.UpdateAll();

        // check the changed count. should be updated.
        Assert.AreEqual(topTextChangeCount, 1);
        Assert.AreEqual(centerTextChangeCount, 1);
        Assert.AreEqual(bottomTextChangeCount, 1);
        Assert.AreEqual(timeTagsChangeCount, 1);

        // reset the count to 0 for the remaining test.
        topTextChangeCount = 0;
        centerTextChangeCount = 0;
        bottomTextChangeCount = 0;
        timeTagsChangeCount = 0;
    }

    [Test]
    public void TestTextChanged()
    {
        // change the property.
        lyric!.Text = "karaoke";

        // check the changed count
        Assert.AreEqual(topTextChangeCount, 1);
        Assert.AreEqual(centerTextChangeCount, 1);
        Assert.AreEqual(bottomTextChangeCount, 1);
        Assert.AreEqual(timeTagsChangeCount, 1);
    }

    [Test]
    public void TestSwitchDisplayType()
    {
        // change the display type.
        testProcessor!.DisplayType = LyricDisplayType.RomanisedSyllable;

        // check the changed count
        Assert.AreEqual(topTextChangeCount, 1);
        Assert.AreEqual(centerTextChangeCount, 1);
        Assert.AreEqual(bottomTextChangeCount, 1);
        Assert.AreEqual(timeTagsChangeCount, 1);
    }

    [Test]
    public void TestSwitchDisplayProperty()
    {
        // change the display property.
        testProcessor!.DisplayProperty = LyricDisplayProperty.TopText;

        // check the changed count
        Assert.AreEqual(topTextChangeCount, 1);
        Assert.AreEqual(centerTextChangeCount, 1);
        Assert.AreEqual(bottomTextChangeCount, 1);
        Assert.AreEqual(timeTagsChangeCount, 1);
    }

    [TearDown]
    public void TearDown()
    {
        // reset the count to 0 for the next test.
        topTextChangeCount = 0;
        centerTextChangeCount = 0;
        bottomTextChangeCount = 0;
        timeTagsChangeCount = 0;
    }
}
