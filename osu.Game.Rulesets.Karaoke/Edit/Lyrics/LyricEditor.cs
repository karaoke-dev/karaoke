// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Layouts;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Rulesets.Timing;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    [Cached(typeof(ILyricEditorState))]
    public class LyricEditor : Container, ILyricEditorState, IKeyBindingHandler<KaraokeEditAction>
    {
        [Resolved(canBeNull: true)]
        private ILyricsChangeHandler lyricsChangeHandler { get; set; }

        [Resolved(canBeNull: true)]
        private ILyricTextChangeHandler lyricTextChangeHandler { get; set; }

        [Resolved(canBeNull: true)]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

        [Resolved]
        private EditorClock editorClock { get; set; }

        [Resolved]
        private KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager { get; set; }

        [Cached]
        private readonly LyricEditorColourProvider colourProvider = new();

        [Cached(typeof(ILyricSelectionState))]
        private readonly LyricSelectionState lyricSelectionState;

        [Cached(typeof(ILyricCaretState))]
        private readonly LyricCaretState lyricCaretState;

        [Cached(typeof(IBlueprintSelectionState))]
        private readonly BlueprintSelectionState blueprintSelectionState;

        [Cached(typeof(IScrollingInfo))]
        private readonly LocalScrollingInfo scrollingInfo = new();

        [Cached]
        private readonly BindableBeatDivisor beatDivisor = new();

        private readonly Bindable<LyricEditorMode> bindableMode = new();

        public IBindable<LyricEditorMode> BindableMode => bindableMode;

        private readonly Bindable<float> bindableFontSize = new();
        private readonly Bindable<MovingTimeTagCaretMode> bindableCreateMovingCaretMode = new();
        private readonly Bindable<MovingTimeTagCaretMode> bindableRecordingMovingCaretMode = new();
        private readonly BindableList<Lyric> bindableLyrics = new();

        private readonly GridContainer gridContainer;
        private readonly GridContainer lyricEditorGridContainer;
        private readonly Container leftSideExtendArea;
        private readonly Container rightSideExtendArea;
        private readonly KaraokeLyricEditorSkin skin;
        private readonly DrawableLyricEditList container;

        private const int spacing = 10;

        public LyricEditor()
        {
            AddInternal(lyricSelectionState = new LyricSelectionState());
            AddInternal(lyricCaretState = new LyricCaretState());
            AddInternal(blueprintSelectionState = new BlueprintSelectionState());

            Add(gridContainer = new GridContainer
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
                                    new SkinProvidingContainer(skin = new KaraokeLyricEditorSkin(null))
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Child = container = new DrawableLyricEditList
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                        }
                                    },
                                },
                                Array.Empty<Drawable>(),
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
            });

            container.Items.BindTo(bindableLyrics);
            container.OnOrderChanged += (x, nowOrder) =>
            {
                lyricsChangeHandler?.ChangeOrder(nowOrder);
            };

            lyricCaretState.MoveCaret(MovingCaretAction.First);

            BindableMode.BindValueChanged(e =>
            {
                // should wait until beatmap has been loaded.
                Schedule(() =>
                {
                    initialCaretPositionAlgorithm();
                    lyricCaretState.ResetPosition(e.NewValue);
                });

                // display add new lyric only with edit mode.
                container.DisplayBottomDrawable = e.NewValue == LyricEditorMode.Manage;

                // should control grid container spacing and place some component.
                initializeExtendArea();

                // cancel selecting if switch mode.
                lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Cancel);
            }, true);

            bindableFontSize.BindValueChanged(e =>
            {
                skin.FontSize = e.NewValue;
            });

            bindableCreateMovingCaretMode.BindValueChanged(_ =>
            {
                initialCaretPositionAlgorithm();

                lyricCaretState.ResetPosition(Mode);
            });

            bindableRecordingMovingCaretMode.BindValueChanged(_ =>
            {
                initialCaretPositionAlgorithm();

                lyricCaretState.ResetPosition(Mode);
            });

            lyricSelectionState.Selecting.BindValueChanged(_ =>
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
            float width = extendArea?.ExtendWidth ?? 0;

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
                    throw new ArgumentOutOfRangeException(nameof(extendArea.Direction));
            }

            EditExtend getExtendArea() =>
                Mode switch
                {
                    LyricEditorMode.Language => new LanguageExtend(),
                    LyricEditorMode.EditRuby => new RubyTagExtend(),
                    LyricEditorMode.EditRomaji => new RomajiTagExtend(),
                    LyricEditorMode.CreateTimeTag => new TimeTagExtend(),
                    LyricEditorMode.RecordTimeTag => new TimeTagExtend(),
                    LyricEditorMode.AdjustTimeTag => new TimeTagExtend(),
                    LyricEditorMode.EditNote => new NoteExtend(),
                    LyricEditorMode.Singer => new SingerExtend(),
                    LyricEditorMode.Layout => new LayoutExtend(),
                    _ => null
                };

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
            bool show = lyricSelectionState.Selecting.Value;
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
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.LyricEditorFontSize, bindableFontSize);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.CreateTimeTagMovingCaretMode, bindableCreateMovingCaretMode);
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.RecordingTimeTagMovingCaretMode, bindableRecordingMovingCaretMode);

            // set-up divisor.
            beatDivisor.Value = beatmap.BeatmapInfo.BeatDivisor;

            // load lyric in here
            var lyrics = OrderUtils.Sorted(beatmap.HitObjects.OfType<Lyric>());
            bindableLyrics.AddRange(lyrics);

            // need to check is there any lyric added or removed.
            beatmap.HitObjectAdded += e =>
            {
                if (e is not Lyric lyric)
                    return;

                var previousLyric = bindableLyrics.LastOrDefault(x => x.Order < lyric.Order);

                if (previousLyric != null)
                {
                    int insertIndex = bindableLyrics.IndexOf(previousLyric) + 1;
                    bindableLyrics.Insert(insertIndex, lyric);
                }
                else
                {
                    // insert to first.
                    bindableLyrics.Insert(0, lyric);
                }

                initialCaretPositionAlgorithm();
            };
            beatmap.HitObjectRemoved += e =>
            {
                if (e is not Lyric lyric)
                    return;

                bindableLyrics.Remove(lyric);
                initialCaretPositionAlgorithm();
            };

            initialCaretPositionAlgorithm();
        }

        private void initialCaretPositionAlgorithm()
        {
            var state = Mode;
            var recordingMovingCaretMode = Mode == LyricEditorMode.RecordTimeTag
                ? bindableRecordingMovingCaretMode.Value
                : bindableCreateMovingCaretMode.Value;

            lyricCaretState.ChangePositionAlgorithm(state, recordingMovingCaretMode);
        }

        public bool OnPressed(KeyBindingPressEvent<KaraokeEditAction> e)
        {
            var action = e.Action;
            bool isMoving = HandleMovingEvent(action);
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

                case LyricEditorMode.EditNote:
                case LyricEditorMode.Layout:
                case LyricEditorMode.Singer:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException(nameof(Mode));
            }
        }

        public void OnReleased(KeyBindingReleaseEvent<KaraokeEditAction> e)
        {
        }

        protected bool HandleMovingEvent(KaraokeEditAction action) =>
            action switch
            {
                KaraokeEditAction.Up => lyricCaretState.MoveCaret(MovingCaretAction.Up),
                KaraokeEditAction.Down => lyricCaretState.MoveCaret(MovingCaretAction.Down),
                KaraokeEditAction.Left => lyricCaretState.MoveCaret(MovingCaretAction.Left),
                KaraokeEditAction.Right => lyricCaretState.MoveCaret(MovingCaretAction.Right),
                KaraokeEditAction.First => lyricCaretState.MoveCaret(MovingCaretAction.First),
                KaraokeEditAction.Last => lyricCaretState.MoveCaret(MovingCaretAction.Last),
                _ => false
            };

        protected bool HandleSetTimeEvent(KaraokeEditAction action)
        {
            if (lyricTimeTagsChangeHandler == null)
                return false;

            var caretPosition = lyricCaretState.BindableCaretPosition.Value;
            if (caretPosition is not TimeTagCaretPosition timeTagCaretPosition)
                throw new NotSupportedException(nameof(caretPosition));

            var currentTimeTag = timeTagCaretPosition.TimeTag;

            switch (action)
            {
                case KaraokeEditAction.ClearTime:
                    lyricTimeTagsChangeHandler.ClearTimeTagTime(currentTimeTag);
                    return true;

                case KaraokeEditAction.SetTime:
                    double currentTime = editorClock.CurrentTime;
                    lyricTimeTagsChangeHandler.SetTimeTagTime(currentTimeTag, currentTime);

                    if (lyricEditorConfigManager.Get<bool>(KaraokeRulesetLyricEditorSetting.RecordingAutoMoveToNextTimeTag))
                        lyricCaretState.MoveCaret(MovingCaretAction.Right);

                    return true;

                default:
                    return false;
            }
        }

        protected bool HandleCreateOrDeleterTimeTagEvent(KaraokeEditAction action)
        {
            if (lyricTimeTagsChangeHandler == null)
                return false;

            if (lyricCaretState.BindableCaretPosition.Value is not TimeTagIndexCaretPosition position)
                throw new NotSupportedException(nameof(position));

            var index = position.Index;

            switch (action)
            {
                case KaraokeEditAction.Create:
                    lyricTimeTagsChangeHandler.AddByPosition(index);
                    return true;

                case KaraokeEditAction.Remove:
                    lyricTimeTagsChangeHandler.RemoveByPosition(index);
                    return true;

                default:
                    return false;
            }
        }

        public LyricEditorMode Mode
        {
            get => bindableMode.Value;
            set => bindableMode.Value = value;
        }

        public virtual void NavigateToFix(LyricEditorMode mode)
        {
            switch (mode)
            {
                case LyricEditorMode.Typing:
                case LyricEditorMode.Language:
                case LyricEditorMode.AdjustTimeTag:
                    Mode = mode;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }
        }

        private class LocalScrollingInfo : IScrollingInfo
        {
            public IBindable<ScrollingDirection> Direction { get; } = new Bindable<ScrollingDirection>(ScrollingDirection.Left);

            public IBindable<double> TimeRange { get; } = new BindableDouble(5000)
            {
                MinValue = 1000,
                MaxValue = 10000
            };

            public IScrollAlgorithm Algorithm { get; } = new SequentialScrollAlgorithm(new List<MultiplierControlPoint>());
        }
    }
}
