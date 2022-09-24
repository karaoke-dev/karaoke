// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Extends;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Extends.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Extends.RecordingTimeTags;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList.Rows.Extends.TimeTags;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList
{
    public class DrawablePreviewLyricListItem : DrawableLyricListItem
    {
        private FillFlowContainer content = null!;

        private readonly IBindable<TimeTagEditMode> bindableTimeTagEditMode = new Bindable<TimeTagEditMode>();
        private readonly IBindable<ICaretPosition> bindableCaretPosition = new Bindable<ICaretPosition>();

        public DrawablePreviewLyricListItem(Lyric item)
            : base(item)
        {
            bindableTimeTagEditMode.BindValueChanged(_ =>
            {
                // should remove extend when switch mode.
                removeExtend();
            });

            bindableCaretPosition.BindValueChanged(e =>
            {
                onCaretPositionChanged(e.NewValue);
            });
        }

        protected override void OnModeChanged()
        {
            base.OnModeChanged();

            // should remove extend when switch mode.
            removeExtend();
        }

        private void onCaretPositionChanged(ICaretPosition? caretPosition)
        {
            // should wait until time-tag edit mode value updated.
            Schedule(() =>
            {
                // should remove the extend area if caret position does not focus to current lyric row.
                if (caretPosition?.Lyric != Model)
                {
                    removeExtend();
                    return;
                }

                // show not create again if contains same extend.
                var existExtend = getExtend();
                if (existExtend != null)
                    return;

                // show extra extend if hover to current lyric.
                var editExtend = createExtend(EditorMode, bindableTimeTagEditMode.Value, Model);
                if (editExtend == null)
                    return;

                editExtend.RelativeSizeAxes = Axes.X;
                content.Add(editExtend);
                editExtend.Show();
            });

            static EditRowExtend? createExtend(LyricEditorMode mode, TimeTagEditMode timeTagEditMode, Lyric lyric) =>
                mode switch
                {
                    LyricEditorMode.EditNote => new NoteRowExtend(lyric),
                    LyricEditorMode.EditTimeTag when timeTagEditMode == TimeTagEditMode.Recording => new RecordingTimeTagRowExtend(lyric),
                    LyricEditorMode.EditTimeTag when timeTagEditMode == TimeTagEditMode.Adjust => new TimeTagRowExtend(lyric),
                    _ => null
                };
        }

        private void removeExtend()
        {
            var existExtend = getExtend();
            if (existExtend == null)
                return;

            // todo : might remove component until Extend effect end.
            content.Remove(existExtend, true);
        }

        private EditRowExtend? getExtend()
        {
            return content?.Children.OfType<EditRowExtend>().FirstOrDefault();
        }

        protected override Drawable CreateContent()
        {
            return content = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Children = new[]
                {
                    base.CreateContent()
                }
            };
        }

        protected override Row CreateEditRow(Lyric lyric)
            => new EditLyricPreviewRow(lyric);

        public override float ExtendHeight => getExtend()?.ContentHeight ?? 0;

        [BackgroundDependencyLoader]
        private void load(ITimeTagModeState timeTagModeState)
        {
            bindableTimeTagEditMode.BindTo(timeTagModeState.BindableEditMode);
        }
    }
}
