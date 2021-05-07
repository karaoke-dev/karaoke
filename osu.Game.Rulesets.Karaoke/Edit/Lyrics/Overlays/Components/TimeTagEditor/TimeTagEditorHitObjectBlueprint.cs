// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Overlays.Components.TimeTagEditor
{
    public class TimeTagEditorHitObjectBlueprint : SelectionBlueprint<TimeTag>
    {
        private const float circle_size = 38;

        public Action<DragEvent> OnDragHandled;

        private readonly ExtendableCircle circle;

        public TimeTagEditorHitObjectBlueprint(TimeTag item)
            : base(item)
        {
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.CentreLeft;

            AddRangeInternal(new Drawable[]
            {
                circle = new ExtendableCircle
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                },
            });
        }

        /// <summary>
        /// A circle with externalised end caps so it can take up the full width of a relative width area.
        /// </summary>
        public class ExtendableCircle : CompositeDrawable
        {
            protected readonly Circle Content;

            public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => Content.ReceivePositionalInputAt(screenSpacePos);

            public override Quad ScreenSpaceDrawQuad => Content.ScreenSpaceDrawQuad;

            public ExtendableCircle()
            {
                Padding = new MarginPadding { Horizontal = -circle_size / 2f };
                InternalChild = Content = new Circle
                {
                    BorderColour = OsuColour.Gray(0.75f),
                    BorderThickness = 4,
                    Masking = true,
                    RelativeSizeAxes = Axes.Both,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Type = EdgeEffectType.Shadow,
                        Radius = 5,
                        Colour = Color4.Black.Opacity(0.4f)
                    }
                };
            }
        }
    }
}
