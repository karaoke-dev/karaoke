// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Layouts;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;
using osuTK.Input;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    [Cached(typeof(ILyricEditorState))]
    public partial class LyricEditor : Container, ILyricEditorState, IKeyBindingHandler<KaraokeEditAction>
    {
        [Resolved(canBeNull: true)]
        private LyricManager lyricManager { get; set; }

        [Resolved]
        private EditorClock editorClock { get; set; }

        [Cached]
        private readonly LyricEditorColourProvider colourProvider = new LyricEditorColourProvider();

        public Bindable<LyricEditorMode> BindableMode { get; } = new Bindable<LyricEditorMode>();

        public Bindable<RecordingMovingCaretMode> BindableRecordingMovingCaretMode { get; } = new Bindable<RecordingMovingCaretMode>();

        public BindableBool BindableAutoFocusEditLyric { get; } = new BindableBool();

        public BindableInt BindableAutoFocusEditLyricSkipRows { get; } = new BindableInt();

        public BindableList<Lyric> BindableLyrics { get; } = new BindableList<Lyric>();

        public Bindable<ICaretPosition> BindableHoverCaretPosition { get; } = new Bindable<ICaretPosition>();

        public Bindable<ICaretPosition> BindableCaretPosition { get; } = new Bindable<ICaretPosition>();

        public BindableList<TimeTag> SelectedTimeTags { get; } = new BindableList<TimeTag>();

        public BindableList<RubyTag> SelectedRubyTags { get; } = new BindableList<RubyTag>();

        public BindableList<RomajiTag> SelectedRomajiTags { get; } = new BindableList<RomajiTag>();

        public BindableBool Selecting { get; } = new BindableBool();

        public BindableList<Lyric> SelectedLyrics { get; } = new BindableList<Lyric>();

        public Action<LyricEditorSelectingAction> Action { get; set; }

        private readonly GridContainer gridContainer;
        private readonly GridContainer lyricEditorGridContainer;
        private readonly Container leftSideExtendArea;
        private readonly Container rightSideExtendArea;
        private readonly KaraokeLyricEditorSkin skin;
        private readonly DrawableLyricEditList container;

        private const int spacing = 10;

        public LyricEditor()
        {
            Child = gridContainer = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                Content = new[]
                {
                    new Drawable[]
                    {
                        leftSideExtendArea = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                        new Box(),
                        lyricEditorGridContainer = new GridContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Content = new[]
                            {
                                new Drawable[]
                                {
                                    new SkinProvidingContainer(skin = new KaraokeLyricEditorSkin())
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Child = container = new DrawableLyricEditList
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                        }
                                    },
                                },
                                new Drawable[]
                                {
                                },
                                new Drawable[]
                                {
                                    new ApplySelectingArea(),
                                }
                            }
                        },
                        new Box(),
                        rightSideExtendArea = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                    }
                }
            };

            container.Items.BindTo(BindableLyrics);
            if (lyricManager != null)
                container.OnOrderChanged += lyricManager.ChangeLyricOrder;

            MoveCaret(MovingCaretAction.First);

            BindableMode.BindValueChanged(e =>
            {
                initialCaretPositionAlgorithm();

                // display add new lyric only with edit mode.
                container.DisplayBottomDrawable = e.NewValue == LyricEditorMode.Manage;

                // should wait until beatmap has been loaded.
                Schedule(() => ResetPosition(e.NewValue));

                // should control grid container spacing and place some component.
                initializeExtendArea();

                // cancel selecting if switch mode.
                EndSelecting(LyricEditorSelectingAction.Cancel);
            }, true);

            BindableRecordingMovingCaretMode.BindValueChanged(e =>
            {
                initialCaretPositionAlgorithm();

                ResetPosition(Mode);
            });

            Selecting.BindValueChanged(e =>
            {
                initializeApplySelectingArea();
            }, true);
        }

        private void initializeExtendArea()
        {
            var extendArea = getExtendArea();
            if (extendArea != null && checkDuplicatedWithExistExtend(extendArea))
                return;

            leftSideExtendArea.Clear();
            rightSideExtendArea.Clear();

            var direction = extendArea?.Direction;
            var width = extendArea?.ExtendWidth ?? 0;

            gridContainer.ColumnDimensions = new[]
            {
                new Dimension(GridSizeMode.Absolute, direction == ExtendDirection.Left ? width : 0),
                new Dimension(GridSizeMode.Absolute, direction == ExtendDirection.Left ? spacing : 0),
                new Dimension(),
                new Dimension(GridSizeMode.Absolute, direction == ExtendDirection.Right ? spacing : 0),
                new Dimension(GridSizeMode.Absolute, direction == ExtendDirection.Right ? width : 0),
            };

            if (extendArea == null)
                return;

            switch (extendArea.Direction)
            {
                case ExtendDirection.Left:
                    leftSideExtendArea.Add(extendArea);
                    break;

                case ExtendDirection.Right:
                    rightSideExtendArea.Add(extendArea);
                    break;

                default:
                    throw new IndexOutOfRangeException(nameof(extendArea.Direction));
            }

            EditExtend getExtendArea()
            {
                switch (Mode)
                {
                    case LyricEditorMode.Language:
                        return new LanguageExtend();

                    case LyricEditorMode.EditRuby:
                        return new RubyTagExtend();

                    case LyricEditorMode.EditRomaji:
                        return new RomajiTagExtend();

                    case LyricEditorMode.CreateTimeTag:
                    case LyricEditorMode.RecordTimeTag:
                    case LyricEditorMode.AdjustTimeTag:
                        return new TimeTagExtend();

                    case LyricEditorMode.CreateNote:
                    case LyricEditorMode.CreateNotePosition:
                    case LyricEditorMode.AdjustNote:
                        return new NoteExtend();

                    case LyricEditorMode.Singer:
                        return new SingerExtend();

                    case LyricEditorMode.Layout:
                        return new LayoutExtend();

                    default:
                        return null;
                }
            }

            bool checkDuplicatedWithExistExtend(EditExtend extend)
            {
                var type = extendArea.GetType();
                if (leftSideExtendArea.Children?.FirstOrDefault()?.GetType() == type)
                    return true;

                if (rightSideExtendArea.Children?.FirstOrDefault()?.GetType() == type)
                    return true;

                return false;
            }
        }

        private void initializeApplySelectingArea()
        {
            var show = Selecting.Value;
            lyricEditorGridContainer.RowDimensions = new[]
            {
                new Dimension(),
                new Dimension(GridSizeMode.Absolute, show ? spacing : 0),
                new Dimension(GridSizeMode.AutoSize),
            };
        }

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap)
        {
            // load lyric in here
            var lyrics = OrderUtils.Sorted(beatmap.HitObjects.OfType<Lyric>());
            BindableLyrics.AddRange(lyrics);

            // need to check is there any lyric added or removed.
            beatmap.HitObjectAdded += e =>
            {
                if (e is Lyric lyric)
                {
                    var previousLyric = BindableLyrics.LastOrDefault(x => x.Order < lyric.Order);

                    if (previousLyric != null)
                    {
                        var insertIndex = BindableLyrics.IndexOf(previousLyric) + 1;
                        BindableLyrics.Insert(insertIndex, lyric);
                    }
                    else
                    {
                        // insert to first.
                        BindableLyrics.Insert(0, lyric);
                    }

                    initialCaretPositionAlgorithm();
                }
            };
            beatmap.HitObjectRemoved += e =>
            {
                if (e is Lyric lyric)
                {
                    BindableLyrics.Remove(lyric);
                    initialCaretPositionAlgorithm();
                }
            };

            initialCaretPositionAlgorithm();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (lyricManager == null)
                return false;

            if (Mode != LyricEditorMode.Typing)
                return false;

            var caretPosition = BindableCaretPosition.Value;
            if (!(caretPosition is TextCaretPosition textCaretPosition))
                throw new NotSupportedException(nameof(caretPosition));

            var lyric = textCaretPosition.Lyric;
            var index = textCaretPosition.Index;

            switch (e.Key)
            {
                case Key.BackSpace:
                    // delete single character.
                    var deletedSuccess = lyricManager.DeleteLyricText(lyric, index);
                    if (deletedSuccess)
                        MoveCaret(MovingCaretAction.Left);
                    return deletedSuccess;

                default:
                    return false;
            }
        }

        public bool OnPressed(KaraokeEditAction action)
        {
            if (lyricManager == null)
                return false;

            var isMoving = HandleMovingEvent(action);
            if (isMoving)
                return true;

            switch (Mode)
            {
                case LyricEditorMode.View:
                case LyricEditorMode.Manage:
                case LyricEditorMode.Typing: // will handle in OnKeyDown
                case LyricEditorMode.Language:
                case LyricEditorMode.EditRuby:
                case LyricEditorMode.EditRomaji:
                    return false;

                case LyricEditorMode.CreateTimeTag:
                    return HandleCreateOrDeleterTimeTagEvent(action);

                case LyricEditorMode.RecordTimeTag:
                    return HandleSetTimeEvent(action);

                case LyricEditorMode.AdjustTimeTag:
                    return false;

                case LyricEditorMode.CreateNote:
                case LyricEditorMode.CreateNotePosition:
                case LyricEditorMode.AdjustNote:
                case LyricEditorMode.Layout:
                case LyricEditorMode.Singer:
                    return false;

                default:
                    throw new IndexOutOfRangeException(nameof(Mode));
            }
        }

        public void OnReleased(KaraokeEditAction action)
        {
        }

        protected bool HandleMovingEvent(KaraokeEditAction action)
        {
            // moving caret action
            switch (action)
            {
                case KaraokeEditAction.Up:
                    return MoveCaret(MovingCaretAction.Up);

                case KaraokeEditAction.Down:
                    return MoveCaret(MovingCaretAction.Down);

                case KaraokeEditAction.Left:
                    return MoveCaret(MovingCaretAction.Left);

                case KaraokeEditAction.Right:
                    return MoveCaret(MovingCaretAction.Right);

                case KaraokeEditAction.First:
                    return MoveCaret(MovingCaretAction.First);

                case KaraokeEditAction.Last:
                    return MoveCaret(MovingCaretAction.Last);

                default:
                    return false;
            }
        }

        protected bool HandleSetTimeEvent(KaraokeEditAction action)
        {
            if (lyricManager == null)
                return false;

            var caretPosition = BindableCaretPosition.Value;
            if (!(caretPosition is TimeTagCaretPosition timeTagCaretPosition))
                throw new NotSupportedException(nameof(caretPosition));

            var currentTimeTag = timeTagCaretPosition.TimeTag;

            switch (action)
            {
                case KaraokeEditAction.ClearTime:
                    return lyricManager.ClearTimeTagTime(currentTimeTag);

                case KaraokeEditAction.SetTime:
                    var currentTime = editorClock.CurrentTime;
                    var setTimeSuccess = lyricManager.SetTimeTagTime(currentTimeTag, currentTime);
                    if (setTimeSuccess)
                        MoveCaret(MovingCaretAction.Right);
                    return setTimeSuccess;

                default:
                    return false;
            }
        }

        protected bool HandleCreateOrDeleterTimeTagEvent(KaraokeEditAction action)
        {
            if (lyricManager == null)
                return false;

            if (!(BindableCaretPosition.Value is TimeTagIndexCaretPosition position))
                throw new NotSupportedException(nameof(position));

            var lyric = position.Lyric;
            var index = position.Index;

            switch (action)
            {
                case KaraokeEditAction.Create:
                    return lyricManager.AddTimeTagByPosition(lyric, index);

                case KaraokeEditAction.Remove:
                    return lyricManager.RemoveTimeTagByPosition(lyric, index);

                default:
                    return false;
            }
        }

        public float FontSize
        {
            get => skin.FontSize;
            set => skin.FontSize = value;
        }

        public LyricEditorMode Mode
        {
            get => BindableMode.Value;
            set => BindableMode.Value = value;
        }

        public RecordingMovingCaretMode RecordingMovingCaretMode
        {
            get => BindableRecordingMovingCaretMode.Value;
            set => BindableRecordingMovingCaretMode.Value = value;
        }

        public bool AutoFocusEditLyric
        {
            get => BindableAutoFocusEditLyric.Value;
            set => BindableAutoFocusEditLyric.Value = value;
        }

        public int AutoFocusEditLyricSkipRows
        {
            get => BindableAutoFocusEditLyricSkipRows.Value;
            set => BindableAutoFocusEditLyricSkipRows.Value = value;
        }
    }
}
