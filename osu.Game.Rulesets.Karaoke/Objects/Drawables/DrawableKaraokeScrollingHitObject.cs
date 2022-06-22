// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    public abstract class DrawableKaraokeScrollingHitObject : DrawableHitObject<KaraokeHitObject>
    {
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

        private double computedLifetimeStart;

        public override double LifetimeStart
        {
            get => base.LifetimeStart;
            set
            {
                computedLifetimeStart = value;

                if (!AlwaysAlive)
                    base.LifetimeStart = value;
            }
        }

        private double computedLifetimeEnd;

        public override double LifetimeEnd
        {
            get => base.LifetimeEnd;
            set
            {
                computedLifetimeEnd = value;

                if (!AlwaysAlive)
                    base.LifetimeEnd = value;
            }
        }

        private bool alwaysAlive;

        /// <summary>
        /// Whether this <see cref="DrawableKaraokeHitObject"/> should always remain alive.
        /// </summary>
        internal bool AlwaysAlive
        {
            get => alwaysAlive;
            set
            {
                if (alwaysAlive == value)
                    return;

                alwaysAlive = value;

                if (value)
                {
                    // Set the base lifetimes directly, to avoid mangling the computed lifetimes
                    base.LifetimeStart = double.MinValue;
                    base.LifetimeEnd = double.MaxValue;
                }
                else
                {
                    LifetimeStart = computedLifetimeStart;
                    LifetimeEnd = computedLifetimeEnd;
                }
            }
        }

        protected virtual void OnDirectionChanged(ValueChangedEvent<ScrollingDirection> e)
        {
            Anchor = Origin = e.NewValue == ScrollingDirection.Left ? Anchor.CentreLeft : Anchor.CentreRight;
        }

        protected virtual void OnTimeRangeChanged(ValueChangedEvent<double> e)
        {
        }
    }

    public abstract class DrawableKaraokeScrollingHitObject<TObject> : DrawableKaraokeScrollingHitObject
        where TObject : KaraokeHitObject
    {
        public new TObject HitObject => base.HitObject as TObject;

        protected DrawableKaraokeScrollingHitObject(TObject hitObject)
            : base(hitObject)
        {
        }
    }
}
