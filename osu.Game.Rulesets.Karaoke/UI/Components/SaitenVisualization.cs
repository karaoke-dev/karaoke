// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Caching;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Lines;
using osu.Framework.Graphics.Performance;
using osu.Framework.Layout;
using osu.Framework.Threading;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.Components
{
    public class SaitenVisualization : LifetimeManagementContainer
    {
        private const float safe_lifetime_end_multiplier = 1;

        private readonly IBindable<double> timeRange = new BindableDouble();
        private readonly IBindable<ScrollingDirection> direction = new Bindable<ScrollingDirection>();

        [Resolved]
        private IScrollingInfo scrollingInfo { get; set; }

        [Resolved]
        private IPositionCalculator calculator { get; set; }

        private readonly LayoutValue initialStateCache = new LayoutValue(Invalidation.RequiredParentSizeToFit | Invalidation.DrawInfo);

        private readonly IDictionary<SaitenPath, List<KaraokeReplayFrame>> frames = new Dictionary<SaitenPath, List<KaraokeReplayFrame>>();
        private readonly IDictionary<SaitenPath, Cached> pathInitialStateCache = new Dictionary<SaitenPath, Cached>();

        protected IEnumerable<SaitenPath> Paths => InternalChildren.Cast<SaitenPath>();
        protected IEnumerable<SaitenPath> AlivePaths => AliveInternalChildren.Cast<SaitenPath>();

        protected virtual void Add(Path hitObject) => AddInternal(hitObject);
        protected virtual bool Remove(Path hitObject) => RemoveInternal(hitObject);

        public SaitenVisualization()
        {
            AddLayout(initialStateCache);
        }

        public ColourInfo LineColour
        {
            get => Colour;
            set => Colour = value;
        }

        public new bool Masking
        {
            get => base.Masking;
            set => base.Masking = value;
        }

        private float pathRadius = 2;

        public float PathRadius
        {
            get => pathRadius;
            set
            {
                if (pathRadius == value)
                    return;

                pathRadius = value;

                foreach (var path in Paths)
                {
                    path.PathRadius = pathRadius;
                }
            }
        }

        protected double MaxAvailableTime => frames.LastOrDefault().Value?.LastOrDefault()?.Time ?? 0;

        public SaitenOrientatePosition OrientatePosition { get; set; } = SaitenOrientatePosition.Start;

        [BackgroundDependencyLoader]
        private void load()
        {
            direction.BindTo(scrollingInfo.Direction);
            timeRange.BindTo(scrollingInfo.TimeRange);

            direction.ValueChanged += _ => initialStateCache.Invalidate();
            timeRange.ValueChanged += _ => initialStateCache.Invalidate();
        }

        private bool createNew = true;

        public void Add(KaraokeReplayFrame frame)
        {
            // Start time should be largest and cannot be removed.
            var startTime = frame.Time;
            if (startTime <= MaxAvailableTime)
                throw new ArgumentOutOfRangeException($"{nameof(startTime)} out of range.");

            if (!frame.Sound)
            {
                // Next replay frame will create new path
                createNew = true;
                return;
            }

            if (createNew)
            {
                var path = new SaitenPath(startTime)
                {
                    PathRadius = PathRadius
                };
                frames.Add(path, new List<KaraokeReplayFrame> { frame });
                pathInitialStateCache.Add(path, new Cached());

                Add(path);

                createNew = false;
            }
            else
            {
                frames.LastOrDefault().Value.Add(frame);
            }
        }

        public void Clear()
        {
            frames.Clear();
            ClearInternal();
        }

        private float scrollLength;

        protected override void Update()
        {
            base.Update();

            scrollLength = DrawSize.X;

            // If change the speed or direction, mark all the cache is invalid and re-calculate life time
            if (!initialStateCache.IsValid)
            {
                // Reset scroll info
                scrollingInfo.Algorithm.Reset();

                foreach (var cached in pathInitialStateCache.Values)
                    cached.Invalidate();

                foreach (var path in Paths)
                    computeLifetime(path);

                // Mark all the state is valid
                initialStateCache.Validate();
            }

            // Re-calculate alive path
            AlivePaths.ForEach(computePath);
        }

        protected void MarkAsInvalid(SaitenPath path) => pathInitialStateCache[path].Invalidate();

        private void computeLifetime(SaitenPath path)
        {
            var startTime = path.StartTime;
            var endTime = frames[path].LastOrDefault()?.Time;
            if (endTime == null)
                return;

            float originAdjustment = 0.0f;

            switch (direction.Value)
            {
                case ScrollingDirection.Left:
                    originAdjustment = path.OriginPosition.X;
                    break;

                case ScrollingDirection.Right:
                    originAdjustment = path.DrawWidth - path.OriginPosition.X;
                    break;
            }

            path.LifetimeStart = scrollingInfo.Algorithm.GetDisplayStartTime(startTime, originAdjustment, timeRange.Value, scrollLength);
            path.LifetimeEnd = scrollingInfo.Algorithm.TimeAt(scrollLength * safe_lifetime_end_multiplier, endTime.Value, timeRange.Value, scrollLength);
        }

        // Cant use AddOnce() since the delegate is re-constructed every invocation
        private void computePath(SaitenPath path) => path.Schedule(() =>
        {
            var startTime = path.StartTime;
            if (pathInitialStateCache[path].IsValid)
                return;

            pathInitialStateCache?[path].Validate();

            // Calculate path
            var frameList = frames[path];
            if (frameList.Count <= 1)
                return;

            path.ClearVertices();

            bool left = direction.Value == ScrollingDirection.Left;
            path.Anchor = path.Origin = left ? Anchor.TopLeft : Anchor.TopRight;

            var centerPosition = calculator.CenterPosition();
            var scaleDistance = calculator.Distance();

            foreach (var frame in frameList)
            {
                var x = scrollingInfo.Algorithm.GetLength(startTime, frame.Time, timeRange.Value, scrollLength);
                path.AddVertex(new Vector2(left ? x : -x, frame.Scale * scaleDistance - centerPosition));
            }
        });

        protected override void UpdateAfterChildrenLife()
        {
            base.UpdateAfterChildrenLife();

            // We need to calculate hit object positions as soon as possible after lifetimes so that hitobjects get the final say in their positions
            foreach (var path in AlivePaths)
                updatePosition(path, Time.Current);
        }

        protected override void OnChildLifetimeBoundaryCrossed(LifetimeBoundaryCrossedEvent e)
        {
            // Recalculate path if appear
            if (e.Kind == LifetimeBoundaryKind.Start && e.Child is SaitenPath path)
                computePath(path);

            base.OnChildLifetimeBoundaryCrossed(e);
        }

        private void updatePosition(SaitenPath path, double currentTime)
        {
            double startTime = path.StartTime;
            var multiple = direction.Value == ScrollingDirection.Left ? 1 : -1;
            var x = scrollingInfo.Algorithm.PositionAt(startTime, currentTime, timeRange.Value, scrollLength);
            var offset = OrientatePosition == SaitenOrientatePosition.End ? scrollLength : 0;
            path.X = (x + offset) * multiple;
        }

        public class SaitenPath : Path
        {
            public override bool RemoveWhenNotAlive => false;

            public double StartTime { get; }

            public SaitenPath(double startTime)
            {
                StartTime = startTime;
            }

            /// <summary>
            /// Schedules an <see cref="Action"/> to this <see cref="SaitenPath"/>.
            /// </summary>
            protected internal new ScheduledDelegate Schedule(Action action) => base.Schedule(action);
        }

        public enum SaitenOrientatePosition
        {
            Start,

            End
        }
    }
}
