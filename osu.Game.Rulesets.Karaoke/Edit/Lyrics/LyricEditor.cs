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
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Languages;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Manage;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
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

        [Resolved]
        private KaraokeRulesetLyricEditorConfigManager lyricEditorConfigManager { get; set; }

        [Cached]
        private readonly LyricEditorColourProvider colourProvider = new();

        [Cached(typeof(ILyricSelectionState))]
        private readonly LyricSelectionState lyricSelectionState;

        [Cached(typeof(ILyricCaretState))]
        private readonly LyricCaretState lyricCaretState;

        [Cached(typeof(IManageModeState))]
        private readonly ManageModeState manageModeState;

        [Cached(typeof(ILanguageModeState))]
        private readonly LanguageModeState languageModeState;

        [Cached(typeof(IEditRubyModeState))]
        private readonly EditRubyModeState editRubyModeState;

        [Cached(typeof(IEditRomajiModeState))]
        private readonly EditRomajiModeState editRomajiModeState;

        [Cached(typeof(ITimeTagModeState))]
        private readonly TimeTagModeState timeTagModeState;

        [Cached(typeof(IEditNoteModeState))]
        private readonly EditNoteModeState editNoteModeState;

        [Cached(typeof(IScrollingInfo))]
        private readonly LocalScrollingInfo scrollingInfo = new();

        [Cached]
        private readonly BindableBeatDivisor beatDivisor = new();

        private readonly Bindable<LyricEditorMode> bindableMode = new();
        private IBindable<bool> bindableSelecting => lyricSelectionState.Selecting;

        public IBindable<LyricEditorMode> BindableMode => bindableMode;

        private readonly Bindable<float> bindableFontSize = new();

        private readonly GridContainer gridContainer;
        private readonly GridContainer lyricEditorGridContainer;
        private readonly Container leftSideExtendArea;
        private readonly Container rightSideExtendArea;
        private readonly LyricEditorSkin skin;
        private readonly DrawableLyricEditList container;

        private const int spacing = 10;

        public LyricEditor()
        {
            // global state
            AddInternal(lyricSelectionState = new LyricSelectionState());
            AddInternal(lyricCaretState = new LyricCaretState());

            // state for target mode only.
            AddInternal(manageModeState = new ManageModeState());
            AddInternal(languageModeState = new LanguageModeState());
            AddInternal(editRubyModeState = new EditRubyModeState());
            AddInternal(editRomajiModeState = new EditRomajiModeState());
            AddInternal(timeTagModeState = new TimeTagModeState());
            AddInternal(editNoteModeState = new EditNoteModeState());

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
                                    new SkinProvidingContainer(skin = new LyricEditorSkin(null))
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

            container.OnOrderChanged += (x, nowOrder) =>
            {
                lyricsChangeHandler?.ChangeOrder(nowOrder);
            };

            BindableMode.BindValueChanged(e =>
            {
                updateAddLyricState();

                // should control grid container spacing and place some component.
                initializeExtendArea();

                // cancel selecting if switch mode.
                lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Cancel);
            }, true);

            bindableSelecting.BindValueChanged(e =>
            {
                updateAddLyricState();
            });

            bindableFontSize.BindValueChanged(e =>
            {
                skin.FontSize = e.NewValue;
            });

            lyricSelectionState.Selecting.BindValueChanged(_ =>
            {
                initializeApplySelectingArea();
            }, true);
        }

        private void updateAddLyricState()
        {
            // display add new lyric only with edit mode.
            bool disableBottomDrawable = BindableMode.Value == LyricEditorMode.Manage && !bindableSelecting.Value;
            container.DisplayBottomDrawable = disableBottomDrawable;
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
                    LyricEditorMode.Manage => new ManageExtend(),
                    LyricEditorMode.Language => new LanguageExtend(),
                    LyricEditorMode.EditRuby => new RubyTagExtend(),
                    LyricEditorMode.EditRomaji => new RomajiTagExtend(),
                    LyricEditorMode.EditTimeTag => new TimeTagExtend(),
                    LyricEditorMode.EditNote => new NoteExtend(),
                    LyricEditorMode.Singer => new SingerExtend(),
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
        private void load(EditorBeatmap beatmap, ILyricsProvider lyricsProvider)
        {
            lyricEditorConfigManager.BindWith(KaraokeRulesetLyricEditorSetting.LyricEditorFontSize, bindableFontSize);

            // set-up divisor.
            beatDivisor.Value = beatmap.BeatmapInfo.BeatDivisor;

            container.Items.BindTo(lyricsProvider.BindableLyrics);
        }

        public virtual bool OnPressed(KeyBindingPressEvent<KaraokeEditAction> e) =>
            e.Action switch
            {
                KaraokeEditAction.Up => lyricCaretState.MoveCaret(MovingCaretAction.Up),
                KaraokeEditAction.Down => lyricCaretState.MoveCaret(MovingCaretAction.Down),
                KaraokeEditAction.Left => lyricCaretState.MoveCaret(MovingCaretAction.Left),
                KaraokeEditAction.Right => lyricCaretState.MoveCaret(MovingCaretAction.Right),
                KaraokeEditAction.First => lyricCaretState.MoveCaret(MovingCaretAction.First),
                KaraokeEditAction.Last => lyricCaretState.MoveCaret(MovingCaretAction.Last),
                _ => false
            };

        public void OnReleased(KeyBindingReleaseEvent<KaraokeEditAction> e)
        {
        }

        public virtual void NavigateToFix(LyricEditorMode mode)
        {
            switch (mode)
            {
                case LyricEditorMode.Typing:
                case LyricEditorMode.Language:
                case LyricEditorMode.EditTimeTag:
                    SwitchMode(mode);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }
        }

        public LyricEditorMode Mode
            => bindableMode.Value;

        public void SwitchMode(LyricEditorMode mode)
            => bindableMode.Value = mode;

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
