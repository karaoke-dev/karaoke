// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osu.Game.Tests.Visual;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    /// <summary>
    /// A test scene for skinnable karaoke components.
    /// </summary>
    public abstract class KaraokeSkinnableTestScene : SkinnableTestScene
    {
        public const int COLUMN_NUMBER = 9;

        [Cached(Type = typeof(IScrollingInfo))]
        private readonly TestScrollingInfo scrollingInfo = new TestScrollingInfo();

        [Cached(Type = typeof(IPositionCalculator))]
        private readonly PositionCalculator positionCalculator = new PositionCalculator(COLUMN_NUMBER);

        protected KaraokeSkinnableTestScene()
        {
            scrollingInfo.Direction.Value = ScrollingDirection.Down;

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
            IScrollAlgorithm IScrollingInfo.Algorithm { get; } = new ConstantScrollAlgorithm();
        }
    }
}
