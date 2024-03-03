// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Extensions.EnumExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites.Processor;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Helper;
using osu.Game.Tests.Visual;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Graphics.Sprites.Processor;

/// <summary>
/// Test should be focus on:
/// 1. Test one or more property changed, all related property should be changed.
/// 2. Test if the property is being triggered.
/// </summary>
public abstract partial class TestSceneDisplayProcessor : OsuGridTestScene
{
    // check value.
    private IReadOnlyList<PositionText>? topText;
    private string? centerText;
    private IReadOnlyList<PositionText>? bottomText;
    private IReadOnlyDictionary<double, TextIndex>? timeTags;

    // check changed count
    private int topTextChangeCount;
    private int centerTextChangeCount;
    private int bottomTextChangeCount;
    private int timeTagsChangeCount;

    private Lyric? lyric;
    private BaseDisplayProcessor? testProcessor;

    private readonly ManualClock manualClock = new();

    protected TestSceneDisplayProcessor()
        : base(2, 1)
    {
        AddSliderStep("Adjust clock time", 0, 5000, 2500, time =>
        {
            manualClock.CurrentTime = time;
        });
    }

    #region Override properties.

    protected abstract BaseDisplayProcessor CreateProcessor(Lyric lyric, LyricDisplayProperty displayProperty);

    #endregion

    #region Tests

    [Test]
    public void TestDisplayProperty([Values] LyricDisplayProperty property)
    {
        Initialize(property);

        // should not have any change.
        AssertTopTextNotChanged();
        AssertCenterTextNotChanged();
        AssertBottomTextNotChanged();
        AssertTimeTagsNotChanged();

        TriggerChange(lyric =>
        {
            switch (testProcessor)
            {
                // will trigger all properties changed if the center text is changed.
                case LyricFirstDisplayProcessor:
                    lyric.Text = "カラオケ";
                    break;

                case RomanizedSyllableFirstDisplayProcessor:
                    lyric.TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:1000#^ka", "[1,start]:2000#ra", "[2,start]:3000#o", "[3,start]:4000#ke", "[3,end]:5000" });
                    break;

                default:
                    throw new InvalidOperationException();
            }
        });

        if (property.HasFlagFast(LyricDisplayProperty.TopText))
        {
            AssertTopTextChanged();
        }
        else
        {
            AssertTopTextNotChanged();
        }

        AssertCenterTextChanged();

        if (property.HasFlagFast(LyricDisplayProperty.BottomText))
        {
            AssertBottomTextChanged();
        }
        else
        {
            AssertBottomTextNotChanged();
        }
    }

    #endregion

    #region Tools

    protected void Initialize(LyricDisplayProperty displayProperty = LyricDisplayProperty.Both)
    {
        Initialize(() => new Lyric(), displayProperty);
    }

    protected void Initialize(Func<Lyric> createLyricFunc, LyricDisplayProperty displayProperty = LyricDisplayProperty.Both)
    {
        AddStep("Initialize", () =>
        {
            // reset the value
            topText = default;
            centerText = default;
            bottomText = default;
            timeTags = default;

            // reset the count
            topTextChangeCount = 0;
            centerTextChangeCount = 0;
            bottomTextChangeCount = 0;
            timeTagsChangeCount = 0;

            // create processor
            lyric = createLyricFunc();
            testProcessor = CreateProcessor(lyric, displayProperty);

            // bind event
            testProcessor.TopTextChanged += texts =>
            {
                topText = texts;
                topTextChangeCount++;
            };
            testProcessor.CenterTextChanged += text =>
            {
                centerText = text;
                centerTextChangeCount++;
            };
            testProcessor.BottomTextChanged += texts =>
            {
                bottomText = texts;
                bottomTextChangeCount++;
            };
            testProcessor.TimeTagsChanged += tags =>
            {
                timeTags = tags;
                timeTagsChangeCount++;
            };

            // create the drawable for preview.
            createSampleSpriteText(lyric);
        });
    }

    private void createSampleSpriteText(Lyric lyric)
    {
        Cell(0).Child = createProvider("karaoke sprite text", new DrawableKaraokeSpriteText(lyric)
        {
            LeftTextColour = Color4.Green,
            RightTextColour = Color4.Red,
            Clock = new FramedClock(manualClock),
        });
        Cell(1).Child = createProvider("lyric sprite text", new DrawableLyricSpriteText(lyric));
        return;

        static Drawable createProvider(string name, Drawable sample) =>
            new Container
            {
                RelativeSizeAxes = Axes.Both,
                BorderColour = Color4.White,
                BorderThickness = 3,
                Masking = true,

                Children = new[]
                {
                    new Box
                    {
                        AlwaysPresent = true,
                        Alpha = 0,
                        RelativeSizeAxes = Axes.Both,
                    },
                    new OsuSpriteText
                    {
                        Text = name,
                        Scale = new Vector2(1.5f),
                        Padding = new MarginPadding(5),
                    },
                    sample.With(x =>
                    {
                        x.Anchor = Anchor.Centre;
                        x.Origin = Anchor.Centre;
                        x.Scale = new Vector2(2);
                    }),
                },
            };
    }

    protected void TriggerChange(Action<Lyric> targetLyric)
    {
        AddStep("Change property", () =>
        {
            if (lyric == null)
                throw new InvalidOperationException("Test lyric should not be null.");

            if (testProcessor == null)
                throw new InvalidOperationException("Process should not be null.");

            targetLyric(lyric);
        });
    }

    protected void AssertTopTextChanged(IEnumerable<PositionText> expectedValue)
    {
        AssertTopTextChanged();
        AddAssert("Check top text value", () => topText?.SequenceEqual(expectedValue) ?? false);
    }

    protected void AssertTopTextChanged()
    {
        AddAssert("Top text should trigger only once", () => topTextChangeCount == 1);
    }

    protected void AssertTopTextNotChanged()
    {
        AddAssert("Top text should not triggered", () => topTextChangeCount == 0);
    }

    protected void AssertCenterTextChanged(string expectedValue)
    {
        AssertCenterTextChanged();
        AddAssert("Check center text value", () => centerText == expectedValue);
    }

    protected void AssertCenterTextChanged()
    {
        AddAssert("Center text should trigger only once", () => centerTextChangeCount == 1);
    }

    protected void AssertCenterTextNotChanged()
    {
        AddAssert("Center text should not triggered", () => centerTextChangeCount == 0);
    }

    protected void AssertBottomTextChanged(IEnumerable<PositionText> expectedValue)
    {
        AssertBottomTextChanged();
        AddAssert("Check bottom text value", () => bottomText?.SequenceEqual(expectedValue) ?? false);
    }

    protected void AssertBottomTextChanged()
    {
        AddAssert("Bottom text should trigger only once", () => bottomTextChangeCount == 1);
    }

    protected void AssertBottomTextNotChanged()
    {
        AddAssert("Bottom text should not triggered", () => bottomTextChangeCount == 0);
    }

    protected void AssertTimeTagsChanged(Dictionary<double, TextIndex> expectedValue)
    {
        AssertTimeTagsChanged();
        AddAssert("Check time tags value", () => timeTags?.SequenceEqual(expectedValue) ?? false);
    }

    protected void AssertTimeTagsChanged()
    {
        AddAssert("Time-tag should trigger only once", () => timeTagsChangeCount == 1);
    }

    protected void AssertTimeTagsNotChanged()
    {
        AddAssert("Time-tag should not triggered", () => timeTagsChangeCount == 0);
    }

    #endregion
}
