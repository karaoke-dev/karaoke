// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Judgements;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Default;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Notes;
using osu.Game.Rulesets.Karaoke.UI.Position;
using osu.Game.Rulesets.Karaoke.Utils;
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

        private readonly Bindable<bool> isHitting = new();

        private readonly IBindable<NotePositionCalculator> positionBindable = new Bindable<NotePositionCalculator>();

        public readonly IBindable<string> TextBindable = new Bindable<string>();
        public readonly IBindable<string> RubyTextBindable = new Bindable<string>();
        public readonly IBindable<int[]> SingersBindable = new Bindable<int[]>();
        public readonly IBindable<bool> DisplayBindable = new Bindable<bool>();
        public readonly IBindable<Tone> ToneBindable = new Bindable<Tone>();

        public DrawableNote()
            : this(null)
        {
        }

        public DrawableNote([CanBeNull] Note hitObject)
            : base(hitObject)
        {
            AddRangeInternal(new Drawable[]
            {
                new SkinnableDrawable(new KaraokeSkinComponent(KaraokeSkinComponents.Note), _ => new DefaultBodyPiece { RelativeSizeAxes = Axes.Both }),
                textPiece = new OsuSpriteText(),
            });

            // Comment it because i'm not sure will it be used in the future or not.
            /*
            AccentColour.BindValueChanged(colour =>
            {
                bodyPiece.AccentColour = colour.NewValue;
            }, true);
            */
        }

        [BackgroundDependencyLoader]
        private void load(INotePositionInfo notePositionInfo)
        {
            positionBindable.BindTo(notePositionInfo.Position);

            positionBindable.BindValueChanged(_ => updateNotePositionAndHeight());
            TextBindable.BindValueChanged(_ => { changeText(HitObject); });
            RubyTextBindable.BindValueChanged(_ => { changeText(HitObject); });
            SingersBindable.BindValueChanged(_ => { ApplySkin(CurrentSkin, false); });
            DisplayBindable.BindValueChanged(e => { (Result.Judgement as KaraokeNoteJudgement).Saitenable = e.NewValue; });
            ToneBindable.BindValueChanged(_ => updateNotePositionAndHeight());

            void updateNotePositionAndHeight()
            {
                Y = notePositionInfo.Calculator.YPositionAt(HitObject);
                Height = notePositionInfo.Calculator.ColumnHeight;
            }
        }

        protected override void OnApply()
        {
            base.OnApply();

            TextBindable.BindTo(HitObject.TextBindable);
            RubyTextBindable.BindTo(HitObject.RubyTextBindable);
            DisplayBindable.BindTo(HitObject.DisplayBindable);
            ToneBindable.BindTo(HitObject.ToneBindable);

            if (HitObject.ParentLyric != null)
            {
                HitObject.ParentLyricBindable.BindValueChanged(x =>
                {
                    SingersBindable.UnbindAll();
                    SingersBindable.BindTo(x.NewValue.SingersBindable);
                }, true);
            }
        }

        protected override void OnFree()
        {
            base.OnFree();

            TextBindable.UnbindFrom(HitObject.TextBindable);
            RubyTextBindable.UnbindFrom(HitObject.RubyTextBindable);
            DisplayBindable.UnbindFrom(HitObject.DisplayBindable);
            ToneBindable.UnbindFrom(HitObject.ToneBindable);

            if (HitObject.ParentLyric != null)
                SingersBindable.UnbindFrom(HitObject.ParentLyric?.SingersBindable);
        }

        protected override void ApplySkin(ISkinSource skin, bool allowFallback)
        {
            base.ApplySkin(skin, allowFallback);

            if (CurrentSkin == null)
                return;

            if (HitObject == null)
                return;

            var noteSkin = skin.GetConfig<KaraokeSkinLookup, NoteSkin>(new KaraokeSkinLookup(KaraokeSkinConfiguration.NoteStyle, HitObject.ParentLyric?.Singers))?.Value;
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
            // todo: should apply the setting.
            textPiece.Text = NoteUtils.DisplayText(note, true);
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

        public bool OnPressed(KeyBindingPressEvent<KaraokeSaitenAction> e)
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
                EndSing();
            }

            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<KaraokeSaitenAction> e)
        {
            // Make sure that the user started holding the key during the hold note
            if (!holdStartTime.HasValue)
                return;

            // User stop singing this note
            EndSing();
        }
    }
}
