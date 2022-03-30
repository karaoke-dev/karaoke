// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.RecordingTimeTags;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Extends.TimeTags;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class DrawableLyricEditListItem : OsuRearrangeableListItem<Lyric>
    {
        private Box background;
        private FillFlowContainer content;

        [Resolved]
        private LyricEditorColourProvider colourProvider { get; set; }

        [Resolved]
        private ILyricCaretState lyricCaretState { get; set; }

        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();
        private readonly IBindable<TimeTagEditMode> bindableTimeTagEditMode = new Bindable<TimeTagEditMode>();
        private readonly IBindable<ICaretPosition> bindableHoverCaretPosition = new Bindable<ICaretPosition>();
        private readonly IBindable<ICaretPosition> bindableCaretPosition = new Bindable<ICaretPosition>();

        public DrawableLyricEditListItem(Lyric item)
            : base(item)
        {
            bindableMode.BindValueChanged(e =>
            {
                // Only draggable in edit mode.
                ShowDragHandle.Value = e.NewValue == LyricEditorMode.Manage;

                // should remove extend when switch mode.
                removeExtend();
            }, true);

            bindableTimeTagEditMode.BindValueChanged(_ =>
            {
                // should remove extend when switch mode.
                removeExtend();
            });

            bindableHoverCaretPosition.BindValueChanged(_ =>
            {
                updateBackgroundColour();
            });

            bindableCaretPosition.BindValueChanged(e =>
            {
                updateBackgroundColour();

                // should wait until time-tag edit mode value updated.
                Schedule(() =>
                {
                    // should remove the extend area if caret position does not focus to current lyric row.
                    if (e.NewValue?.Lyric != Model)
                    {
                        removeExtend();
                        return;
                    }

                    // show not create again if contains same extend.
                    var existExtend = getExtend();
                    if (existExtend != null)
                        return;

                    // show extra extend if hover to current lyric.
                    var editExtend = createExtend(bindableMode.Value, bindableTimeTagEditMode.Value, Model);
                    if (editExtend == null)
                        return;

                    editExtend.RelativeSizeAxes = Axes.X;
                    content.Add(editExtend);
                    editExtend.Show();
                });

                static EditRowExtend createExtend(LyricEditorMode mode, TimeTagEditMode timeTagEditMode, Lyric lyric) =>
                    mode switch
                    {
                        LyricEditorMode.EditNote => new NoteRowExtend(lyric),
                        LyricEditorMode.EditTimeTag when timeTagEditMode == TimeTagEditMode.Recording => new RecordingTimeTagRowExtend(lyric),
                        LyricEditorMode.EditTimeTag when timeTagEditMode == TimeTagEditMode.Adjust => new TimeTagRowExtend(lyric),
                        _ => null
                    };
            });

            void removeExtend()
            {
                var existExtend = getExtend();
                if (existExtend == null)
                    return;

                // todo : might remove component until Extend effect end.
                content.Remove(existExtend);
            }
        }

        private EditRowExtend getExtend()
        {
            return content?.Children.OfType<EditRowExtend>().FirstOrDefault();
        }

        public float ExtendHeight => getExtend()?.ContentHeight ?? 0;

        protected override Drawable CreateContent()
        {
            return new Container
            {
                Masking = true,
                CornerRadius = 5,
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Children = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.7f
                    },
                    content = new FillFlowContainer
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
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, ITimeTagModeState timeTagModeState)
        {
            bindableMode.BindTo(state.BindableMode);
            bindableTimeTagEditMode.BindTo(timeTagModeState.BindableEditMode);
            bindableHoverCaretPosition.BindTo(lyricCaretState.BindableHoverCaretPosition);
            bindableCaretPosition.BindTo(lyricCaretState.BindableCaretPosition);

            updateBackgroundColour();
        }

        protected override bool OnDragStart(DragStartEvent e)
        {
            if (!base.OnDragStart(e))
                return false;

            updateBackgroundColour();

            // should mark object as selecting while dragging.
            lyricCaretState.MoveCaretToTargetPosition(Model);

            return true;
        }

        protected override void OnDragEnd(DragEndEvent e)
        {
            updateBackgroundColour();

            base.OnDragEnd(e);
        }

        private void updateBackgroundColour()
        {
            background.Colour = getColour();

            Color4 getColour()
            {
                var mode = bindableMode.Value;

                if (bindableCaretPosition.Value?.Lyric == Model)
                    return colourProvider.Background3(mode);

                if (bindableHoverCaretPosition.Value?.Lyric == Model)
                    return colourProvider.Background4(mode);

                return colourProvider.Background6(mode);
            }
        }
    }
}
