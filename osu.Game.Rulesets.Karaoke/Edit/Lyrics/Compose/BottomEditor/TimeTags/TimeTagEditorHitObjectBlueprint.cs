// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Edit.Components.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.BottomEditor.TimeTags
{
    public class TimeTagEditorHitObjectBlueprint : SelectionBlueprint<TimeTag>, IHasCustomTooltip<TimeTag>
    {
        [UsedImplicitly]
        private readonly Bindable<double?> startTime;

        private readonly TimeTagPiece timeTagPiece;
        private readonly TimeTagWithNoTimePiece timeTagWithNoTimePiece;
        private readonly OsuSpriteText timeTagText;

        public TimeTagEditorHitObjectBlueprint(TimeTag item)
            : base(item)
        {
            startTime = item.TimeBindable.GetBoundCopy();

            Anchor = Anchor.CentreLeft;
            Origin = Anchor.CentreLeft;

            RelativePositionAxes = Axes.X;
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;

            AddRangeInternal(new Drawable[]
            {
                timeTagPiece = new TimeTagPiece(item)
                {
                    Anchor = Anchor.CentreLeft,
                },
                timeTagWithNoTimePiece = new TimeTagWithNoTimePiece(item)
                {
                    Anchor = Anchor.BottomLeft,
                },
                timeTagText = new OsuSpriteText
                {
                    Text = "Demo",
                    Anchor = Anchor.BottomLeft,
                    Y = 10,
                }
            });

            switch (item.Index.State)
            {
                case TextIndex.IndexState.Start:
                    timeTagPiece.Origin = Anchor.CentreLeft;
                    timeTagWithNoTimePiece.Origin = Anchor.BottomLeft;
                    timeTagText.Origin = Anchor.TopLeft;
                    break;

                case TextIndex.IndexState.End:
                    timeTagPiece.Origin = Anchor.CentreRight;
                    timeTagWithNoTimePiece.Origin = Anchor.BottomRight;
                    timeTagText.Origin = Anchor.TopRight;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(item.Index.State));
            }
        }

        [BackgroundDependencyLoader]
        private void load(EditorClock clock, TimeTagEditor timeline, OsuColour colours)
        {
            // todo : should be able to let user able to select show from ruby or main text.
            timeTagText.Text = LyricUtils.GetTimeTagDisplayRubyText(timeline.HitObject, Item);

            timeTagPiece.Clock = clock;
            timeTagPiece.Colour = colours.BlueLight;

            timeTagWithNoTimePiece.Colour = colours.Red;
            startTime.BindValueChanged(_ =>
            {
                bool hasValue = hasTime();

                // update show time-tag style.
                switch (hasValue)
                {
                    case true:
                        timeTagPiece.Show();
                        timeTagWithNoTimePiece.Hide();
                        break;

                    case false:
                        timeTagPiece.Hide();
                        timeTagWithNoTimePiece.Show();
                        break;
                }

                // should wait until all time-tag time has been modified.
                Schedule(() =>
                {
                    double previewTime = timeline.GetPreviewTime(Item);

                    // adjust position.
                    X = (float)previewTime;

                    // make tickle effect.
                    timeTagPiece.ClearTransforms();

                    using (timeTagPiece.BeginAbsoluteSequence(previewTime))
                    {
                        timeTagPiece.Colour = colours.BlueLight;
                        timeTagPiece.FlashColour(colours.PurpleDark, 750, Easing.OutQuint);
                    }
                });
            }, true);
        }

        protected override void OnSelected()
        {
            // base logic hides selected blueprints when not selected, but timeline doesn't do that.
        }

        protected override void OnDeselected()
        {
            // base logic hides selected blueprints when not selected, but timeline doesn't do that.
        }

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) =>
            hasTime() ? timeTagPiece.ReceivePositionalInputAt(screenSpacePos) : timeTagWithNoTimePiece.ReceivePositionalInputAt(screenSpacePos);

        public override Quad SelectionQuad =>
            hasTime() ? timeTagPiece.ScreenSpaceDrawQuad : timeTagWithNoTimePiece.ScreenSpaceDrawQuad;

        public override Vector2 ScreenSpaceSelectionPoint => ScreenSpaceDrawQuad.TopLeft;

        public ITooltip<TimeTag> GetCustomTooltip() => new TimeTagTooltip();

        public TimeTag TooltipContent => Item;

        private bool hasTime() => startTime.Value.HasValue;

        public class TimeTagPiece : CompositeDrawable
        {
            public TimeTagPiece(TimeTag timeTag)
            {
                RelativeSizeAxes = Axes.Y;
                Width = 10;

                var textIndex = timeTag.Index;
                InternalChildren = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = 1.5f,
                        Anchor = TextIndexUtils.GetValueByState(textIndex, Anchor.CentreLeft, Anchor.CentreRight),
                        Origin = TextIndexUtils.GetValueByState(textIndex, Anchor.CentreLeft, Anchor.CentreRight),
                    },
                    new DrawableTextIndex
                    {
                        Size = new Vector2(10),
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        State = textIndex.State
                    }
                };
            }

            public override bool RemoveCompletedTransforms => false;
        }

        public class TimeTagWithNoTimePiece : CompositeDrawable
        {
            public TimeTagWithNoTimePiece(TimeTag timeTag)
            {
                AutoSizeAxes = Axes.Y;
                Width = 10;

                var state = timeTag.Index.State;
                InternalChildren = new Drawable[]
                {
                    new DrawableTextIndex
                    {
                        Size = new Vector2(10),
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        State = state
                    }
                };
            }
        }
    }
}
