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
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics.Parts
{
    public class DrawableTimeTag : CompositeDrawable, IHasCustomTooltip
    {
        /// <summary>
        /// Height of major bar line triangles.
        /// </summary>
        private const float triangle_width = 6;

        [Resolved]
        private ILyricEditorState state { get; set; }

        [Resolved]
        private EditorClock editorClock { get; set; }

        private readonly Bindable<Mode> bindableMode = new Bindable<Mode>();
        private readonly Bindable<RecordingMovingCaretMode> bindableRecordingMovingCaretMode = new Bindable<RecordingMovingCaretMode>();

        private readonly TimeTag timeTag;
        private readonly Lyric lyric;

        public DrawableTimeTag(TimeTag timeTag, Lyric lyric)
        {
            this.timeTag = timeTag;
            this.lyric = lyric;
            AutoSizeAxes = Axes.Both;

            bindableMode.BindValueChanged(x =>
            {
                updateStyle();
            });

            bindableRecordingMovingCaretMode.BindValueChanged(x =>
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
            if (isTrigger(bindableMode.Value) && !state.CaretMovable(new TimeTagCaretPosition(lyric, timeTag)))
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
            bindableRecordingMovingCaretMode.BindTo(state.BindableRecordingMovingCaretMode);
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (!isTrigger(bindableMode.Value))
                return false;

            return state?.MoveHoverCaretToTargetPosition(new TimeTagCaretPosition(lyric, timeTag)) ?? false;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!isTrigger(bindableMode.Value))
                return;

            state?.ClearHoverCaretPosition();
            base.OnHoverLost(e);
        }

        protected override bool OnClick(ClickEvent e)
        {
            // navigation to target time
            // todo : might apply config to allow this behavior in target place.
            var time = timeTag.Time;
            if (time != null)
                editorClock.SeekSmoothlyTo(time.Value);

            if (!isTrigger(bindableMode.Value))
                return false;

            return state.MoveCaretToTargetPosition(new TimeTagCaretPosition(lyric, timeTag));
        }

        protected override void Dispose(bool isDisposing)
        {
            bindableMode.UnbindAll();
            bindableRecordingMovingCaretMode.UnbindAll();

            base.Dispose(isDisposing);
        }

        private bool isTrigger(Mode mode)
            => mode == Mode.RecordMode;

        public object TooltipContent => timeTag;

        public ITooltip GetCustomTooltip() => new TimeTagTooltip();
    }
}
