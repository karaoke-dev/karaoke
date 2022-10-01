// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList
{
    public abstract class DrawableLyricListItem : OsuRearrangeableListItem<Lyric>
    {
        public const float HANDLER_WIDTH = 22;

        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; }

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

        protected DrawableLyricListItem(Lyric item)
            : base(item)
        {
            bindableMode.BindValueChanged(e =>
            {
                // Only draggable in edit mode.
                ShowDragHandle.Value = e.NewValue == LyricEditorMode.Texting;
            }, true);

            DragActive.BindValueChanged(e =>
            {
                // should mark object as selecting while dragging.
                lyricCaretState.MoveCaretToTargetPosition(Model);
            });
        }

        protected sealed override Drawable CreateContent() => CreateEditRow(Model);

        protected abstract Row CreateEditRow(Lyric lyric);

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            bindableMode.BindTo(state.BindableMode);
        }
    }
}
