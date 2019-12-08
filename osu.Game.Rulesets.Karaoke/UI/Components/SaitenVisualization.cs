// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Caching;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Lines;
using osu.Framework.Threading;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Replays;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.UI.Scrolling;
using osuTK;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private readonly Cached initialStateCache = new Cached();
        private readonly Cached addStateCache = new Cached();

        private readonly NotePlayfield notePlayfield;

        private readonly IDictionary<double, SaitenPath> paths = new Dictionary<double, SaitenPath>();
        private readonly IDictionary<double, List<KaraokeReplayFrame>> frames = new Dictionary<double, List<KaraokeReplayFrame>>();
        private readonly IDictionary<double, Cached> pathInitialStateCache = new Dictionary<double, Cached>();

        public IEnumerable<SaitenPath> Objects => InternalChildren.Cast<SaitenPath>();
        public IEnumerable<SaitenPath> AliveObjects => AliveInternalChildren.Cast<SaitenPath>();

        public virtual void Add(Path hitObject) => AddInternal(hitObject);
        public virtual bool Remove(Path hitObject) => RemoveInternal(hitObject);

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

                foreach (var path in Objects)
                {
                    path.PathRadius = pathRadius;
                }
            }
        }

        protected double MaxAvailableTime => frames.LastOrDefault().Value?.LastOrDefault()?.Time ?? 0;

        public SaitenOrientatePosition OrientatePosition { get; set; } = SaitenOrientatePosition.Start;

        public SaitenVisualization(NotePlayfield playfield)
        {
            notePlayfield = playfield;
        }

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
            // start time should be largest and cannot be removed.
            var startTime = frame.Time;
            if (startTime <= MaxAvailableTime)
                throw new ArgumentOutOfRangeException();

            if (!frame.Sound)
            {
                createNew = true;
                return;
            }

            addStateCache.Invalidate();

            if (createNew)
            {
                var path = new SaitenPath
                {
                    PathRadius = PathRadius
                };
                paths.Add(startTime, path);
                frames.Add(startTime, new List<KaraokeReplayFrame> { frame });
                pathInitialStateCache.Add(startTime, new Cached());

                Add(path);

                createNew = false;
            }
            else
            {
                frames.LastOrDefault().Value.Add(frame);
            }
        }

        public override bool Invalidate(Invalidation invalidation = Invalidation.All, Drawable source = null, bool shallPropagate = true)
        {
            if ((invalidation & (Invalidation.RequiredParentSizeToFit | Invalidation.DrawInfo)) > 0)
                initialStateCache.Invalidate();

            return base.Invalidate(invalidation, source, shallPropagate);
        }

        private float scrollLength;

        protected override void Update()
        {
            base.Update();

            if (initialStateCache.IsValid && addStateCache.IsValid)
                return;

            scrollLength = DrawSize.X;

            // If change the speed or direction, mark all the cache is invalid
            if (!initialStateCache.IsValid)
            {
                foreach (var cached in pathInitialStateCache.Values)
                    cached.Invalidate();

                // Reset scroll info
                scrollingInfo.Algorithm.Reset();
            }

            // If addStateCache is invalid, means last path add new vertix
            if (!addStateCache.IsValid)
                pathInitialStateCache?.LastOrDefault().Value?.Invalidate();

            // Re-calculate path if invalid
            var invalidPaths = paths.Where(x => !pathInitialStateCache[x.Key].IsValid);

            foreach (var invalidPath in invalidPaths)
            {
                var startTime = invalidPath.Key;
                var path = invalidPath.Value;

                computeLifetime(startTime, path);
                computePath(startTime, path);

                pathInitialStateCache?[startTime].Validate();
            }

            // Mark all the state is valid
            initialStateCache.Validate();
            addStateCache.Validate();
        }

        private void computeLifetime(double startTime, SaitenPath path)
        {
            var endTime = frames[startTime].LastOrDefault()?.Time;
            if (endTime == null)
                return;

            path.LifetimeStart = scrollingInfo.Algorithm.GetDisplayStartTime(startTime, timeRange.Value);
            path.LifetimeEnd = scrollingInfo.Algorithm.TimeAt(scrollLength * safe_lifetime_end_multiplier, endTime.Value, timeRange.Value, scrollLength);
        }

        // Cant use AddOnce() since the delegate is re-constructed every invocation
        private void computePath(double startTime, SaitenPath path) => path.Schedule(() =>
        {
            var frameList = frames[startTime];
            if (frameList.Count <= 1)
                return;

            path.ClearVertices();

            bool left = direction.Value == ScrollingDirection.Left;
            path.Anchor = path.Origin = left ? Anchor.TopLeft : Anchor.TopRight;

            var currentTime = Time.Current;
            
            var centerPosition = calculator.CenterPosition();
            var scaleDistance = calculator.Distance();

            foreach (var frame in frameList)
            {
                var x = scrollingInfo.Algorithm.PositionAt(frame.Time - startTime, currentTime, timeRange.Value, scrollLength);
                path.AddVertex(new Vector2(left ? x : -x, frame.Scale * scaleDistance - centerPosition));
            }
        });

        protected override void UpdateAfterChildrenLife()
        {
            base.UpdateAfterChildrenLife();

            // We need to calculate hitobject positions as soon as possible after lifetimes so that hitobjects get the final say in their positions
            foreach (var path in AliveObjects)
            {
                var startTime = paths.FirstOrDefault(x => x.Value == path).Key;
                updatePosition(path, startTime, Time.Current);
            }
        }

        private void updatePosition(SaitenPath path, double startTime, double currentTime)
        {
            bool left = direction.Value == ScrollingDirection.Left;
            var x = scrollingInfo.Algorithm.PositionAt(startTime, currentTime, timeRange.Value, scrollLength) * (left ? 1 : -1);
            var offset = (OrientatePosition == SaitenOrientatePosition.End ? scrollLength : 0) * (left ? 1 : -1);
            path.X = x + offset;
        }

        public class SaitenPath : Path
        {
            public override bool RemoveWhenNotAlive => false;

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
