// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Framework.Timing;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Timing;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osu.Game.Screens.Edit;
using osu.Game.Screens.Edit.Compose;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Overlays
{
    public abstract class OverlayHitObjectComposer : HitObjectComposer, IPlacementHandler
    {
        [Cached(Type = typeof(IScrollingInfo))]
        private readonly LocalScrollingInfo scrollingInfo;

        [Resolved]
        protected EditorClock EditorClock { get; private set; }

        [Resolved]
        protected EditorBeatmap EditorBeatmap { get; private set; }

        [Resolved]
        protected IBeatSnapProvider BeatSnapProvider { get; private set; }

        private Playfield playfield;

        protected ComposeBlueprintContainer BlueprintContainer { get; private set; }

        private InputManager inputManager;

        protected OverlayHitObjectComposer()
        {
            scrollingInfo = new LocalScrollingInfo();
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChild = new Container
            {
                Name = "Content",
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    // layers below playfield
                    playfield = CreatePlayfield(),
                    // layers above playfield
                    BlueprintContainer = CreateBlueprintContainer()
                }
            };

            // todo : set stop clock and navigation to target time.
            playfield.Clock = new StopClock(CurrentTime);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            inputManager = GetContainingInputManager();
        }

        protected abstract double CurrentTime { get; }

        /// <summary>
        /// Creates a Playfield.
        /// </summary>
        /// <returns>The Playfield.</returns>
        protected abstract Playfield CreatePlayfield();

        /// <summary>
        /// Construct a relevant blueprint container. This will manage hitobject selection/placement input handling and display logic.
        /// </summary>
        protected virtual ComposeBlueprintContainer CreateBlueprintContainer()
            => new ComposeBlueprintContainer(this);

        public override Playfield Playfield => playfield;

        public override IEnumerable<DrawableHitObject> HitObjects
            => Playfield.AllHitObjects;

        public override bool CursorInPlacementArea => Playfield.ReceivePositionalInputAt(inputManager.CurrentState.Mouse.Position);

        #region IPlacementHandler

        public abstract void BeginPlacement(HitObject hitObject);

        public abstract void EndPlacement(HitObject hitObject, bool commit);

        public abstract void Delete(HitObject hitObject);

        #endregion

        #region IPositionSnapProvider

        /// <summary>
        /// Retrieve the relevant <see cref="Playfield"/> at a specified screen-space position.
        /// In cases where a ruleset doesn't require custom logic (due to nested playfields, for example)
        /// this will return the ruleset main playfield.
        /// </summary>
        /// <param name="screenSpacePosition">The screen-space position to query.</param>
        /// <returns>The most relevant <see cref="Playfield"/>.</returns>
        protected virtual Playfield PlayfieldAtScreenSpacePosition(Vector2 screenSpacePosition) => Playfield;

        public override SnapResult SnapScreenSpacePositionToValidTime(Vector2 screenSpacePosition)
        {
            var playfield = PlayfieldAtScreenSpacePosition(screenSpacePosition);
            double? targetTime = null;

            if (playfield is ScrollingPlayfield scrollingPlayfield)
            {
                targetTime = scrollingPlayfield.TimeAtScreenSpacePosition(screenSpacePosition);

                // apply beat snapping
                targetTime = BeatSnapProvider.SnapTime(targetTime.Value);

                // convert back to screen space
                screenSpacePosition = scrollingPlayfield.ScreenSpacePositionAtTime(targetTime.Value);
            }

            return new SnapResult(screenSpacePosition, targetTime, playfield);
        }

        public override float GetBeatSnapDistanceAt(double referenceTime)
        {
            DifficultyControlPoint difficultyPoint = EditorBeatmap.ControlPointInfo.DifficultyPointAt(referenceTime);
            return (float)(100 * EditorBeatmap.BeatmapInfo.BaseDifficulty.SliderMultiplier * difficultyPoint.SpeedMultiplier / BeatSnapProvider.BeatDivisor);
        }

        public override float DurationToDistance(double referenceTime, double duration)
        {
            double beatLength = BeatSnapProvider.GetBeatLengthAtTime(referenceTime);
            return (float)(duration / beatLength * GetBeatSnapDistanceAt(referenceTime));
        }

        public override double DistanceToDuration(double referenceTime, float distance)
        {
            double beatLength = BeatSnapProvider.GetBeatLengthAtTime(referenceTime);
            return distance / GetBeatSnapDistanceAt(referenceTime) * beatLength;
        }

        public override double GetSnappedDurationFromDistance(double referenceTime, float distance)
            => BeatSnapProvider.SnapTime(referenceTime + DistanceToDuration(referenceTime, distance), referenceTime) - referenceTime;

        public override float GetSnappedDistanceFromDistance(double referenceTime, float distance)
        {
            double actualDuration = referenceTime + DistanceToDuration(referenceTime, distance);

            double snappedEndTime = BeatSnapProvider.SnapTime(actualDuration, referenceTime);

            double beatLength = BeatSnapProvider.GetBeatLengthAtTime(referenceTime);

            // we don't want to exceed the actual duration and snap to a point in the future.
            // as we are snapping to beat length via SnapTime (which will round-to-nearest), check for snapping in the forward direction and reverse it.
            if (snappedEndTime > actualDuration + 1)
                snappedEndTime -= beatLength;

            return DurationToDistance(referenceTime, snappedEndTime - referenceTime);
        }

        #endregion

        private class LocalScrollingInfo : IScrollingInfo
        {
            public IBindable<ScrollingDirection> Direction { get; } = new Bindable<ScrollingDirection>(ScrollingDirection.Left);

            public IBindable<double> TimeRange { get; } = new BindableDouble(1500);

            public IScrollAlgorithm Algorithm { get; set; } = new SequentialScrollAlgorithm(new List<MultiplierControlPoint>());
        }

        private class StopClock : IFrameBasedClock
        {
            public StopClock(double targetTime)
            {
                CurrentTime = targetTime;
            }

            public double ElapsedFrameTime => 0;

            public double FramesPerSecond => 0;

            public FrameTimeInfo TimeInfo => new FrameTimeInfo { Current = CurrentTime, Elapsed = ElapsedFrameTime };

            public double CurrentTime { get; private set; }

            public double Rate => 0;

            public bool IsRunning => false;

            public void ProcessFrame()
            {
            }
        }
    }
}
