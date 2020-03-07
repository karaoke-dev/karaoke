// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    public abstract class DrawableKaraokeScrollingHitObject : DrawableHitObject<KaraokeHitObject>
    {
        /// <summary>
        /// Whether this <see cref="DrawableKaraokeHitObject"/> should always remain alive.
        /// </summary>
        internal bool AlwaysAlive;

        protected readonly IBindable<ScrollingDirection> Direction = new Bindable<ScrollingDirection>();

        protected readonly IBindable<double> TimeRange = new Bindable<double>();

        protected DrawableKaraokeScrollingHitObject(KaraokeHitObject hitObject)
            : base(hitObject)
        {
        }

        [BackgroundDependencyLoader(true)]
        private void load([NotNull] IScrollingInfo scrollingInfo)
        {
            Direction.BindTo(scrollingInfo.Direction);
            Direction.BindValueChanged(OnDirectionChanged, true);

            TimeRange.BindTo(scrollingInfo.TimeRange);
            TimeRange.BindValueChanged(OnTimeRangeChanged, true);
        }

        protected override bool ShouldBeAlive => AlwaysAlive || base.ShouldBeAlive;

        protected virtual void OnDirectionChanged(ValueChangedEvent<ScrollingDirection> e)
        {
            Anchor = Origin = e.NewValue == ScrollingDirection.Left ? Anchor.CentreLeft : Anchor.CentreRight;
        }

        protected virtual void OnTimeRangeChanged(ValueChangedEvent<double> e)
        {
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            // Adjust life time
            LifetimeEnd = HitObject.GetEndTime() + TimeRange.Value;
        }
    }

    public abstract class DrawableKaraokeScrollingHitObject<TObject> : DrawableKaraokeScrollingHitObject
        where TObject : KaraokeHitObject
    {
        public new readonly TObject HitObject;

        protected DrawableKaraokeScrollingHitObject(TObject hitObject)
            : base(hitObject)
        {
            HitObject = hitObject;
        }
    }
}
