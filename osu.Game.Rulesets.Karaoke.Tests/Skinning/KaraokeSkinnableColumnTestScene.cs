// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osuTK.Graphics;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    /// <summary>
    /// A test scene for skinnable karaoke components.
    /// </summary>
    public abstract class KaraokeSkinnableColumnTestScene : KaraokeSkinnableTestScene
    {
        protected const double START_TIME = 1000000000;

        public const int COLUMN_NUMBER = 9;

        [Cached(Type = typeof(IScrollingInfo))]
        private readonly TestScrollingInfo scrollingInfo = new TestScrollingInfo();

        [Cached(Type = typeof(IPositionCalculator))]
        private readonly PositionCalculator positionCalculator = new PositionCalculator(COLUMN_NUMBER);

        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(KaraokeRuleset),
            typeof(KaraokeLegacySkinTransformer),
            typeof(KaraokeSettingsSubsection)
        };

        protected override Ruleset CreateRulesetForSkinProvider() => new KaraokeRuleset();

        protected KaraokeSkinnableColumnTestScene()
        {
            scrollingInfo.Direction.Value = ScrollingDirection.Left;

            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.SlateGray.Opacity(0.2f),
                Depth = 1
            });
        }

        [Test]
        public void TestScrollingDown()
        {
            AddStep("change direction to left", () => scrollingInfo.Direction.Value = ScrollingDirection.Left);
        }

        [Test]
        public void TestScrollingUp()
        {
            AddStep("change direction to right", () => scrollingInfo.Direction.Value = ScrollingDirection.Right);
        }

        private class TestScrollingInfo : IScrollingInfo
        {
            public readonly Bindable<ScrollingDirection> Direction = new Bindable<ScrollingDirection>();

            IBindable<ScrollingDirection> IScrollingInfo.Direction => Direction;
            IBindable<double> IScrollingInfo.TimeRange { get; } = new Bindable<double>(1000);
            IScrollAlgorithm IScrollingInfo.Algorithm { get; } = new ZeroScrollAlgorithm();
        }

        private class ZeroScrollAlgorithm : IScrollAlgorithm
        {
            public double GetDisplayStartTime(double originTime, float offset, double timeRange, float scrollLength)
                => double.MinValue;

            public float GetLength(double startTime, double endTime, double timeRange, float scrollLength)
                => scrollLength;

            public float PositionAt(double time, double currentTime, double timeRange, float scrollLength)
                => (float)((time - START_TIME) / timeRange) * scrollLength;

            public double TimeAt(float position, double currentTime, double timeRange, float scrollLength)
                => 0;

            public void Reset()
            {
            }
        }
    }
}
