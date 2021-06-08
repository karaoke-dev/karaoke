// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

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
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.TimeTags
{
    public class TimeTagEditorHitObjectBlueprint : SelectionBlueprint<TimeTag>, IHasCustomTooltip
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
            }
        }

        [BackgroundDependencyLoader]
        private void load(TimeTagEditor timeline, OsuColour colours)
        {
            timeTagText.Text = LyricUtils.GetTimeTagIndexDisplayText(timeline.HitObject, Item.Index);
            timeTagPiece.Colour = colours.BlueLight;
            timeTagWithNoTimePiece.Colour = colours.Red;
            startTime.BindValueChanged(e =>
            {
                adjustStyle();
                adjustPosition();
            }, true);

            // adjust style if time changed.
            void adjustStyle()
            {
                var hasValue = hasTime();

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
            }

            // assign blueprint position in here.
            void adjustPosition()
            {
                var time = startTime.Value;

                if (time != null)
                {
                    PreviewTime = (float)time.Value;
                }
                else
                {
                    var timeTags = timeline.HitObject.TimeTags;

                    const float preempt_time = 200;
                    var previousTimeTagWithTime = timeTags.GetPreviousMatch(Item, x => x.Time.HasValue);
                    var nextTimeTagWithTime = timeTags.GetNextMatch(Item, x => x.Time.HasValue);

                    if (previousTimeTagWithTime?.Time != null)
                    {
                        PreviewTime = (float)previousTimeTagWithTime.Time.Value + preempt_time;
                    }
                    else if (nextTimeTagWithTime?.Time != null)
                    {
                        PreviewTime = (float)nextTimeTagWithTime.Time.Value - preempt_time;
                    }
                    else
                    {
                        // will goes in here if all time-tag are no time.
                        var index = timeTags.IndexOf(Item);
                        PreviewTime = (float)timeline.StartTime + index * preempt_time;
                    }
                }
            }
        }

        private double previewTime;

        public double PreviewTime
        {
            get => previewTime;
            set
            {
                if(previewTime == value)
                    return;

                previewTime = value;

                // update position also.
                X = (float)previewTime;
            }
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

        public object TooltipContent => Item;

        public ITooltip GetCustomTooltip() => new TimeTagTooltip();

        private bool hasTime() => startTime.Value.HasValue;

        public class TimeTagPiece : CompositeDrawable
        {
            private readonly Box box;

            private readonly RightTriangle triangle;

            public TimeTagPiece(TimeTag timeTag)
            {
                RelativeSizeAxes = Axes.Y;
                Width = 10;
                InternalChildren = new Drawable[]
                {
                    box = new Box
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = 1.5f,
                    },
                    triangle = new RightTriangle
                    {
                        Size = new Vector2(10),
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre
                    }
                };

                switch (timeTag.Index.State)
                {
                    case TextIndex.IndexState.Start:
                        triangle.Scale = new Vector2(1);
                        box.Anchor = Anchor.CentreLeft;
                        box.Origin = Anchor.CentreLeft;
                        break;

                    case TextIndex.IndexState.End:
                        triangle.Scale = new Vector2(-1, 1);
                        box.Anchor = Anchor.CentreRight;
                        box.Origin = Anchor.CentreRight;
                        break;
                }
            }
        }

        public class TimeTagWithNoTimePiece : CompositeDrawable
        {
            private readonly RightTriangle triangle;

            public TimeTagWithNoTimePiece(TimeTag timeTag)
            {
                AutoSizeAxes = Axes.Y;
                Width = 10;
                InternalChildren = new Drawable[]
                {
                    triangle = new RightTriangle
                    {
                        Size = new Vector2(10),
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre
                    }
                };

                switch (timeTag.Index.State)
                {
                    case TextIndex.IndexState.Start:
                        triangle.Scale = new Vector2(1);
                        break;

                    case TextIndex.IndexState.End:
                        triangle.Scale = new Vector2(-1, 1);
                        break;
                }
            }
        }
    }
}
