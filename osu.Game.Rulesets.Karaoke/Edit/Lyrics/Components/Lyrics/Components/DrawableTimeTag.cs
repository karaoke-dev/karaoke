// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics.Components
{
    public class DrawableTimeTag : CompositeDrawable
    {
        /// <summary>
        /// Height of major bar line triangles.
        /// </summary>
        private const float triangle_width = 6;

        [Resolved]
        private ILyricEditorState state { get; set; }

        private readonly Bindable<Mode> bindableMode = new Bindable<Mode>();
        private readonly Bindable<RecordingMovingCursorMode> bindableRecordingMovingCursorMode = new Bindable<RecordingMovingCursorMode>();

        private readonly TimeTag timeTag;

        public DrawableTimeTag(TimeTag timeTag)
        {
            this.timeTag = timeTag;
            AutoSizeAxes = Axes.Both;

            bindableMode.BindValueChanged(x =>
            {
                updateStyle();
            });

            bindableRecordingMovingCursorMode.BindValueChanged(x =>
            {
                updateStyle();
            });

            InternalChild = new RightTriangle
            {
                Name = "Time tag triangle",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                Size = new Vector2(triangle_width),
                Scale = new Vector2(timeTag.Index.State == TextIndex.IndexState.Start ? 1 : -1, 1)
            };
        }

        private void updateStyle()
        {
            if (isTrigger(bindableMode.Value) && !state.RecordingCursorMovable(timeTag))
            {
                InternalChild.Alpha = 0.3f;
            }
            else
            {
                InternalChild.Show();
            }
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            InternalChild.Colour = timeTag.Time.HasValue ? colours.Yellow : colours.Gray7;

            bindableMode.BindTo(state.BindableMode);
            bindableRecordingMovingCursorMode.BindTo(state.BindableRecordingMovingCursorMode);
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (!isTrigger(bindableMode.Value))
                return false;

            return state?.MoveHoverRecordCursorToTargetPosition(timeTag) ?? false;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!isTrigger(bindableMode.Value))
                return;

            state?.ClearHoverRecordCursorPosition();
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (!isTrigger(bindableMode.Value))
                return false;

            return state.MoveRecordCursorToTargetPosition(timeTag);
        }

        protected override void Dispose(bool isDisposing)
        {
            bindableMode.UnbindAll();
            bindableRecordingMovingCursorMode.UnbindAll();

            base.Dispose(isDisposing);
        }

        private bool isTrigger(Mode mode)
            => mode == Mode.RecordMode;
    }
}
