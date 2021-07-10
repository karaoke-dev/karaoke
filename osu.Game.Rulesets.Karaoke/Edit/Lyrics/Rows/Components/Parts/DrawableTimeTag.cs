// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components.Parts
{
    public class DrawableTimeTag : CompositeDrawable, IHasCustomTooltip
    {
        /// <summary>
        /// Height of major bar line triangles.
        /// </summary>
        private const float triangle_width = 6;

        private Bindable<LyricEditorMode> bindableMode;
        private Bindable<RecordingMovingCaretMode> bindableRecordingMovingCaretMode;

        private readonly Lyric lyric;
        private readonly TimeTag timeTag;

        public DrawableTimeTag(Lyric lyric, TimeTag timeTag)
        {
            this.lyric = lyric;
            this.timeTag = timeTag;

            Size = new Vector2(triangle_width);

            var index = timeTag.Index;
            InternalChild = new RightTriangle
            {
                Name = "Time tag triangle",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.Centre,
                Size = new Vector2(triangle_width),
                Scale = new Vector2(index.State == TextIndex.IndexState.Start ? 1 : -1, 1)
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, ILyricEditorState state, LyricCaretState lyricCaretState)
        {
            InternalChild.Colour = colours.GetTimeTagColour(timeTag);

            bindableMode = state.BindableMode.GetBoundCopy();
            bindableRecordingMovingCaretMode = state.BindableRecordingMovingCaretMode.GetBoundCopy();

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

            void updateStyle()
            {
                if (bindableMode.Value != LyricEditorMode.RecordTimeTag)
                {
                    InternalChild.Show();
                    return;
                }

                if (!lyricCaretState.CaretPositionMovable(new TimeTagCaretPosition(lyric, timeTag)))
                {
                    InternalChild.Alpha = 0.3f;
                }
                else
                {
                    InternalChild.Show();
                }
            }
        }

        public object TooltipContent => timeTag;

        public ITooltip GetCustomTooltip() => new TimeTagTooltip();
    }
}
