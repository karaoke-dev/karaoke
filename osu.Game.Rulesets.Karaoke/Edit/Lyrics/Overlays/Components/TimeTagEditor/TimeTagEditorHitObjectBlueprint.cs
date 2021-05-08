// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Overlays.Components.TimeTagEditor
{
    public class TimeTagEditorHitObjectBlueprint : SelectionBlueprint<TimeTag>
    {
        private const float circle_size = 38;

        public Action<DragEvent> OnDragHandled;

        [UsedImplicitly]
        private readonly Bindable<double?> startTime;

        private readonly TimeTagPiece timeTagPiece;
        private readonly OsuSpriteText timeTagText;

        public TimeTagEditorHitObjectBlueprint(TimeTag item)
            : base(item)
        {
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.CentreLeft;

            startTime = item.TimeBindable.GetBoundCopy();
            startTime.BindValueChanged(e =>
            {
                // assign blueprint position in here.
                var time = e.NewValue;

                if (time != null)
                {
                    X = (float)time.Value;
                }
                else
                {
                    // todo : should get relative position.
                }
            }, true);

            RelativePositionAxes = Axes.X;

            RelativeSizeAxes = Axes.X;
            Height = circle_size;

            AddRangeInternal(new Drawable[]
            {
                timeTagPiece = new TimeTagPiece(item)
                {
                    Anchor = Anchor.CentreLeft,
                },
                timeTagText = new OsuSpriteText
                {
                    Text = "Demo",
                    Anchor = Anchor.BottomLeft,
                }
            });

            switch (item.Index.State)
            {
                case TextIndex.IndexState.Start:
                    timeTagPiece.Origin = Anchor.CentreLeft;
                    timeTagText.Origin = Anchor.TopLeft;
                    break;

                case TextIndex.IndexState.End:
                    timeTagPiece.Origin = Anchor.CentreRight;
                    timeTagText.Origin = Anchor.TopRight;
                    break;
            }
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            startTime.BindValueChanged(e =>
            {
                // assign blueprint position in here.
                var hasValue = e.NewValue.HasValue;

                switch (hasValue)
                {
                    case true:
                        timeTagPiece.Colour = colours.BlueLight;
                        break;

                    case false:
                        timeTagPiece.Colour = colours.Red;
                        break;
                }
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
            timeTagPiece.ReceivePositionalInputAt(screenSpacePos);

        public override Quad SelectionQuad => timeTagPiece.ScreenSpaceDrawQuad;

        public override Vector2 ScreenSpaceSelectionPoint => ScreenSpaceDrawQuad.TopLeft;

        public class TimeTagPiece : CompositeDrawable
        {
            protected readonly Box box;

            protected readonly RightTriangle triangle;

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
    }
}
