// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Parts
{
    public class DrawableTimeTag : CompositeDrawable, IHasCustomTooltip
    {
        /// <summary>
        /// Height of major bar line triangles.
        /// </summary>
        private const float triangle_width = 6;

        [Resolved]
        private LyricCaretState lyricCaretState { get; set; }

        [Resolved]
        private EditorClock editorClock { get; set; }

        private readonly Bindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly Bindable<RecordingMovingCaretMode> bindableRecordingMovingCaretMode = new Bindable<RecordingMovingCaretMode>();

        private readonly TimeTagCaretPosition timeTagCaretPosition;

        public DrawableTimeTag(TimeTagCaretPosition timeTagCaretPosition)
        {
            this.timeTagCaretPosition = timeTagCaretPosition;
            Size = new Vector2(triangle_width);

            bindableMode.BindValueChanged(x =>
            {
                // should wait until caret position algorithm loaded.
                Schedule(updateStyle);
            });

            bindableRecordingMovingCaretMode.BindValueChanged(x =>
            {
                // should wait until caret position algorithm loaded.
                Schedule(updateStyle);
            });

            var index = timeTagCaretPosition.TimeTag.Index;
            InternalChild = new RightTriangle
            {
                Name = "Time tag triangle",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                Size = new Vector2(triangle_width),
                Scale = new Vector2(index.State == TextIndex.IndexState.Start ? 1 : -1, 1)
            };
        }

        private void updateStyle()
        {
            if (isTrigger(bindableMode.Value) && !lyricCaretState.CaretPositionMovable(timeTagCaretPosition))
            {
                InternalChild.Alpha = 0.3f;
            }
            else
            {
                InternalChild.Show();
            }
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, ILyricEditorState state)
        {
            var time = timeTagCaretPosition.TimeTag.Time;
            InternalChild.Colour = time.HasValue ? colours.Yellow : colours.Gray7;

            bindableMode.BindTo(state.BindableMode);
            bindableRecordingMovingCaretMode.BindTo(state.BindableRecordingMovingCaretMode);
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (!isTrigger(bindableMode.Value))
                return false;

            return lyricCaretState?.MoveHoverCaretToTargetPosition(timeTagCaretPosition) ?? false;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!isTrigger(bindableMode.Value))
                return;

            lyricCaretState?.ClearHoverCaretPosition();
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            // navigation to target time
            // todo : might apply config to allow this behavior in target place.
            var time = timeTagCaretPosition.TimeTag.Time;
            if (time != null)
                editorClock.SeekSmoothlyTo(time.Value);

            if (!isTrigger(bindableMode.Value))
                return false;

            return lyricCaretState.MoveCaretToTargetPosition(timeTagCaretPosition);
        }

        protected override void Dispose(bool isDisposing)
        {
            bindableMode.UnbindAll();
            bindableRecordingMovingCaretMode.UnbindAll();

            base.Dispose(isDisposing);
        }

        private bool isTrigger(LyricEditorMode mode)
            => mode == LyricEditorMode.RecordTimeTag;

        public object TooltipContent => timeTagCaretPosition.TimeTag;

        public ITooltip GetCustomTooltip() => new TimeTagTooltip();
    }
}
