// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList
{
    public class DrawablePreviewLyricListItem : DrawableLyricListItem
    {
        private FillFlowContainer content = null!;

        private readonly IBindable<TimeTagEditMode> bindableTimeTagEditMode = new Bindable<TimeTagEditMode>();

        public DrawablePreviewLyricListItem(Lyric item)
            : base(item)
        {
            bindableTimeTagEditMode.BindValueChanged(_ =>
            {
                // should remove extend when switch mode.
                removeExtend();
            });
        }

        protected override void OnModeChanged()
        {
            base.OnModeChanged();

            // should remove extend when switch mode.
            removeExtend();
        }

        protected override void OnCaretPositionChanged(ICaretPosition? caretPosition)
        {
            base.OnCaretPositionChanged(caretPosition);

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

        protected override CompositeDrawable CreateRowContent()
        {
            return content = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    new EditLyricRow(Model)
                    {
                        RelativeSizeAxes = Axes.X
                    }
                }
            };
        }

        public override float ExtendHeight => getExtend()?.ContentHeight ?? 0;

        protected override bool HighlightBackgroundWhenSelected(ICaretPosition caretPosition)
        {
            if (caretPosition?.Lyric != Model)
                return false;

            // should not show the background in the assign language mode.
            if (caretPosition is ClickingCaretPosition)
                return false;

            return true;
        }

        protected override Func<LyricEditorMode, Color4> GetBackgroundColour(BackgroundStyle style, LyricEditorColourProvider colourProvider) =>
            style switch
            {
                BackgroundStyle.Idle => colourProvider.Background5,
                BackgroundStyle.Hover => colourProvider.Background4,
                BackgroundStyle.Focus => colourProvider.Background3,
                _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
            };

        [BackgroundDependencyLoader]
        private void load(ITimeTagModeState timeTagModeState)
        {
            bindableTimeTagEditMode.BindTo(timeTagModeState.BindableEditMode);
        }
    }
}
