// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Objects.Drawables
{
    /// <summary>
    /// Visualises a <see cref="Note"/> hit object.
    /// </summary>
    public class DrawableNote : DrawableKaraokeScrollingHitObject<Note>, IKeyBindingHandler<KaraokeSaitenAction>
    {
        private readonly OsuSpriteText textPiece;

        /// <summary>
        /// Time at which the user started holding this hold note. Null if the user is not holding this hold note.
        /// </summary>
        private double? holdStartTime;

        public IBindable<bool> IsHitting => isHitting;

        private readonly Bindable<bool> isHitting = new Bindable<bool>();

        public IBindable<bool> Display => HitObject.DisplayBindable;

        public IBindable<int> StyleIndex => HitObject.StyleIndexBindable;

        public DrawableNote(Note note)
            : base(note)
        {
            Height = DefaultColumnBackground.COLUMN_HEIGHT;

            AddRangeInternal(new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Name = "Background layer",
                    Children = new Drawable[]
                    {
                        new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.NoteBackgroundHead), _ => null)
                        {
                            RelativeSizeAxes = Axes.Y,
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.Centre
                        },
                        new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.NoteBackgroundBody), _ => new DefaultBackgroundBodyPiece{ RelativeSizeAxes = Axes.Both})
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre
                        },
                        new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.NoteBackgroundTail), _ => null)
                        {
                            RelativeSizeAxes = Axes.Y,
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.Centre
                        }
                    }
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Name = "Border layer",
                    Children = new Drawable[]
                    {
                        new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.NoteBorderHead), _ => null)
                        {
                            RelativeSizeAxes = Axes.Y,
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.Centre
                        },
                        new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.NoteBorderBody), _ => new DefaultBorderBodyPiece{ RelativeSizeAxes = Axes.Both})
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre
                        },
                        new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.NoteBorderTail), _ => null)
                        {
                            RelativeSizeAxes = Axes.Y,
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.Centre
                        }
                    }
                },
                textPiece = new OsuSpriteText(),
            });

            // Comment it because i'm not sure will it be used in the future or not.
            /*
            AccentColour.BindValueChanged(colour =>
            {
                bodyPiece.AccentColour = colour.NewValue;
            }, true);
            */

            note.TextBindable.BindValueChanged(_ => { changeText(note); }, true);

            note.AlternativeTextBindable.BindValueChanged(_ => { changeText(note); }, true);

            note.StyleIndexBindable.BindValueChanged(index => { ApplySkin(CurrentSkin, false); }, true);
        }

        protected override void ApplySkin(ISkinSource skin, bool allowFallback)
        {
            base.ApplySkin(skin, allowFallback);

            if (CurrentSkin == null)
                return;

            var noteSkin = skin.GetConfig<KaraokeSkinLookup, NoteSkin>(new KaraokeSkinLookup(KaraokeSkinConfiguration.NoteStyle, HitObject.StyleIndex))?.Value;
            if (noteSkin == null)
                return;

            textPiece.Colour = noteSkin.TextColor;
        }

        protected override void OnDirectionChanged(ValueChangedEvent<ScrollingDirection> e)
        {
            base.OnDirectionChanged(e);

            textPiece.Anchor = textPiece.Origin = e.NewValue == ScrollingDirection.Left ? Anchor.CentreLeft : Anchor.CentreRight;
        }

        protected override void OnTimeRangeChanged(ValueChangedEvent<double> e)
        {
            base.OnTimeRangeChanged(e);

            var paddingSize = 5 + 7 * 1000 / (float)e.NewValue;
            textPiece.Padding = new MarginPadding { Left = paddingSize, Right = paddingSize };
        }

        private void changeText(Note note)
        {
            textPiece.Text = note.AlternativeText ?? note.Text;
        }

        protected void BeginSing()
        {
            holdStartTime = Time.Current;
            isHitting.Value = true;
        }

        protected void EndSing()
        {
            holdStartTime = null;
            isHitting.Value = false;

            UpdateResult(true);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            Debug.Assert(HitObject.HitWindows != null);

            if (!userTriggered)
            {
                if (!HitObject.HitWindows.CanBeHit(timeOffset))
                    ApplyResult(r => r.Type = HitResult.Miss);
                return;
            }

            var result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None)
                return;

            ApplyResult(r => r.Type = result);
        }

        public bool OnPressed(KaraokeSaitenAction action)
        {
            // Make sure the action happened within the body of the hold note
            if (Time.Current < HitObject.StartTime && holdStartTime == null || Time.Current > HitObject.EndTime && holdStartTime == null)
                return false;

            if (holdStartTime == null)
            {
                // User start singing this note
                BeginSing();
            }
            else if (Time.Current > HitObject.EndTime || Time.Current < HitObject.StartTime)
            {
                // User stop singing this note
                OnReleased(action);
            }

            return false;
        }

        public void OnReleased(KaraokeSaitenAction action)
        {
            // Make sure that the user started holding the key during the hold note
            if (!holdStartTime.HasValue)
                return;

            // User stop singing this note
            EndSing();
        }
    }
}
