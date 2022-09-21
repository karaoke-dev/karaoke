// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricList;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Settings;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Timing;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.UI.Scrolling.Algorithms;
using osu.Game.Screens;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    [Cached(typeof(ILyricEditorState))]
    public class LyricEditor : Container, ILyricEditorState, IKeyBindingHandler<KaraokeEditAction>, IKeyBindingHandler<PlatformAction>
    {
        [Cached]
        private readonly LyricEditorColourProvider colourProvider = new();

        [Cached(typeof(ILyricSelectionState))]
        private readonly LyricSelectionState lyricSelectionState;

        [Cached(typeof(ILyricCaretState))]
        private readonly LyricCaretState lyricCaretState;

        [Cached(typeof(ITextingModeState))]
        private readonly TextingModeState textingModeState;

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

        [Cached(typeof(ILyricEditorClipboard))]
        private readonly LyricEditorClipboard lyricEditorClipboard;

        [Cached(typeof(IScrollingInfo))]
        private readonly LocalScrollingInfo scrollingInfo = new();

        [Cached]
        private readonly BindableBeatDivisor beatDivisor = new();

        private readonly Bindable<LyricEditorMode> bindableMode = new();

        public IBindable<LyricEditorMode> BindableMode => bindableMode;

        private readonly GridContainer gridContainer;
        private readonly PreviewLyricList lyricList;
        private readonly Container leftSideSettings;
        private readonly Container rightSideSettings;

        public LyricEditor()
        {
            // global state
            AddInternal(lyricSelectionState = new LyricSelectionState());
            AddInternal(lyricCaretState = new LyricCaretState());

            // state for target mode only.
            AddInternal(textingModeState = new TextingModeState());
            AddInternal(languageModeState = new LanguageModeState());
            AddInternal(editRubyModeState = new EditRubyModeState());
            AddInternal(editRomajiModeState = new EditRomajiModeState());
            AddInternal(timeTagModeState = new TimeTagModeState());
            AddInternal(editNoteModeState = new EditNoteModeState());

            // Separated feature.
            AddInternal(lyricEditorClipboard = new LyricEditorClipboard());

            Add(gridContainer = new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                Content = new[]
                {
                    new Drawable[]
                    {
                        leftSideSettings = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                        lyricList = new PreviewLyricList
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                        rightSideSettings = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                    }
                }
            });

            BindableMode.BindValueChanged(e =>
            {
                // should control grid container spacing and place some component.
                initializeSettingsArea();

                // cancel selecting if switch mode.
                lyricSelectionState.EndSelecting(LyricEditorSelectingAction.Cancel);
            }, true);
        }

        private void initializeSettingsArea()
        {
            var settings = getSettings();
            if (settings != null && checkDuplicatedWithExistSettings(settings))
                return;

            leftSideSettings.Clear();
            rightSideSettings.Clear();

            var direction = settings?.Direction;
            float width = settings?.SettingsWidth ?? 0;

            gridContainer.ColumnDimensions = new[]
            {
                new Dimension(GridSizeMode.Absolute, direction == SettingsDirection.Left ? width : 0),
                new Dimension(),
                new Dimension(GridSizeMode.Absolute, direction == SettingsDirection.Right ? width : 0),
            };

            if (settings == null)
                return;

            switch (settings.Direction)
            {
                case SettingsDirection.Left:
                    leftSideSettings.Add(settings);
                    break;

                case SettingsDirection.Right:
                    rightSideSettings.Add(settings);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(settings.Direction));
            }

            LyricEditorSettings getSettings() =>
                Mode switch
                {
                    LyricEditorMode.Texting => new TextingSettings(),
                    LyricEditorMode.Reference => new ReferenceSettings(),
                    LyricEditorMode.Language => new LanguageSettings(),
                    LyricEditorMode.EditRuby => new RubyTagSettings(),
                    LyricEditorMode.EditRomaji => new RomajiTagSettings(),
                    LyricEditorMode.EditTimeTag => new TimeTagSettings(),
                    LyricEditorMode.EditNote => new NoteSettings(),
                    LyricEditorMode.Singer => new SingerSettings(),
                    _ => null
                };

            bool checkDuplicatedWithExistSettings(LyricEditorSettings settings)
            {
                var type = settings.GetType();
                if (leftSideSettings.Children?.FirstOrDefault()?.GetType() == type)
                    return true;

                if (rightSideSettings.Children?.FirstOrDefault()?.GetType() == type)
                    return true;

                return false;
            }
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            var baseDependencies = new DependencyContainer(base.CreateChildDependencies(parent));

            // Add shader manager as part of dependencies.
            // it will call CreateResourceStore() in KaraokeRuleset and add the resource.
            return new OsuScreenDependencies(false, new DrawableRulesetDependencies(baseDependencies.GetRuleset(), baseDependencies));
        }

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap beatmap)
        {
            // set-up divisor.
            beatDivisor.Value = beatmap.BeatmapInfo.BeatDivisor;
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

        public bool OnPressed(KeyBindingPressEvent<PlatformAction> e)
        {
            switch (e.Action)
            {
                case PlatformAction.Cut:
                    lyricEditorClipboard.Cut();
                    return true;

                case PlatformAction.Copy:
                    lyricEditorClipboard.Copy();
                    return true;

                case PlatformAction.Paste:
                    lyricEditorClipboard.Paste();
                    return true;
            }

            return false;
        }

        public void OnReleased(KeyBindingReleaseEvent<PlatformAction> e)
        {
        }

        public virtual void NavigateToFix(LyricEditorMode mode)
        {
            switch (mode)
            {
                case LyricEditorMode.Texting:
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
