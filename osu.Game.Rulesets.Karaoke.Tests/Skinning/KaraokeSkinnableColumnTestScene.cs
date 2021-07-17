// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Tests.Skinning
{
    /// <summary>
    /// A test scene for skinnable karaoke components.
    /// </summary>
    public abstract class KaraokeSkinnableColumnTestScene : KaraokeSkinnableTestScene
    {
        protected const double START_TIME = 1000000000;
        protected const double DURATION = 1000000000;

        public const int COLUMNS = 9;

        [Cached(Type = typeof(IScrollingInfo))]
        private readonly TestScrollingInfo scrollingInfo = new();

        [Cached(Type = typeof(INotePositionInfo))]
        private readonly PreviewNotePositionInfo notePositionInfo = new();

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

        [BackgroundDependencyLoader]
        private void load(RulesetConfigCache configCache)
        {
            // Cache ruleset config manager and session because karaoke input manager need it.
            var config = (KaraokeRulesetConfigManager)configCache.GetConfigFor(Ruleset.Value.CreateInstance());
            var session = new KaraokeSessionStatics(config, null);

            Dependencies.Cache(config);
            Dependencies.Cache(session);
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
            public readonly Bindable<ScrollingDirection> Direction = new();

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

        private class PreviewNotePositionInfo : INotePositionInfo
        {
            public IBindable<NotePositionCalculator> Position { get; } = new Bindable<NotePositionCalculator>(new NotePositionCalculator(COLUMNS, DefaultColumnBackground.COLUMN_HEIGHT, ScrollingNotePlayfield.COLUMN_SPACING));

            public NotePositionCalculator Calculator => Position.Value;
        }
    }
}
