// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Testing;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition.Algorithms;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.States;

[HeadlessTest]
public abstract partial class BaseLyricCaretStateTest : OsuTestScene
{
    private TestLyricEditorState state = null!;
    private EditorBeatmap editorBeatmap = null!;

    private LyricCaretState lyricCaretState = null!;

    protected ILyricCaretState LyricCaretState => lyricCaretState;

    [BackgroundDependencyLoader]
    private void load()
    {
        var beatmap = new KaraokeBeatmap
        {
            BeatmapInfo =
            {
                Ruleset = new KaraokeRuleset().RulesetInfo,
            },
        };

        Dependencies.Cache(editorBeatmap = new EditorBeatmap(beatmap));
        Dependencies.Cache(new EditorClock());
        Dependencies.CacheAs<ILyricEditorState>(state = new TestLyricEditorState());
        Dependencies.CacheAs<IEditTextModeState>(new EditTextModeState());
        Dependencies.CacheAs<IEditRubyModeState>(new EditRubyModeState());
        Dependencies.CacheAs<IEditTimeTagModeState>(new EditTimeTagModeState());
        Dependencies.Cache(new KaraokeRulesetLyricEditorConfigManager());

        var lyricsProvider = new LyricsProvider();
        Dependencies.CacheAs<ILyricsProvider>(lyricsProvider);

        Children = new Drawable[]
        {
            state,
            lyricsProvider,
            lyricCaretState = new LyricCaretState(),
        };
    }

    [SetUp]
    public void Setup()
    {
        AddStep("Set-up", () =>
        {
            state.SwitchMode(LyricEditorMode.View);
        });
    }

    #region Test utility

    public void PrepareLyrics(IEnumerable<string> lyricTexts)
    {
        AddStep("Prepare lyrics", () =>
        {
            var lyrics = lyricTexts.Select((x, i) => new Lyric { Text = x, Order = i });

            editorBeatmap.Clear();
            editorBeatmap.AddRange(lyrics);
        });
    }

    protected void ChangeMode(TestCaretType type)
    {
        AddStep("Switch to edit mode", () =>
        {
            switch (type)
            {
                case TestCaretType.ViewOnly:
                    state.SwitchMode(LyricEditorMode.View);
                    return;

                case TestCaretType.CaretEnable:
                    state.SwitchMode(LyricEditorMode.EditReferenceLyric);
                    break;

                case TestCaretType.CaretWithIndex:
                    state.SwitchMode(LyricEditorMode.EditText);
                    state.SwitchEditStep(TextEditStep.Split);
                    break;

                case TestCaretType.CaretDraggable:
                    state.SwitchMode(LyricEditorMode.EditText);
                    state.SwitchEditStep(TextEditStep.Typing);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        });
    }

    protected Lyric GetLyric(int index)
    {
        var hitObjects = editorBeatmap.HitObjects[index];

        if (hitObjects is not Lyric lyric)
            throw new InvalidCastException();

        return lyric;
    }

    public void PrepareHoverCaretPosition(Func<ICaretPosition?> getPosition)
    {
        AddStep("Set hover caret position", () =>
        {
            if (LyricCaretState.BindableHoverCaretPosition is not Bindable<ICaretPosition?> bindable)
                throw new InvalidOperationException();

            bindable.Value = getPosition();
        });
    }

    public void PrepareCaretPosition(Func<ICaretPosition?> getPosition)
    {
        AddStep("Set caret position", () =>
        {
            if (LyricCaretState.BindableCaretPosition is not Bindable<ICaretPosition?> bindable)
                throw new InvalidOperationException();

            bindable.Value = getPosition();
        });
    }

    public void PrepareRangeCaretPosition(Func<RangeCaretPosition?> getPosition)
    {
        AddStep("Set range caret position", () =>
        {
            if (LyricCaretState.BindableRangeCaretPosition is not Bindable<RangeCaretPosition?> bindable)
                throw new InvalidOperationException();

            bindable.Value = getPosition();
        });
    }

    protected void AssertHoverCaretPosition(Func<ICaretPosition?> getPosition)
    {
        AddAssert("Assert hover caret position", () => EqualityComparer<ICaretPosition?>.Default.Equals(getPosition(), LyricCaretState.HoverCaretPosition));
    }

    protected void AssertCaretPosition(Func<ICaretPosition?> getPosition)
    {
        AddAssert("Assert caret position", () => EqualityComparer<ICaretPosition?>.Default.Equals(getPosition(), LyricCaretState.CaretPosition));
    }

    protected void AssertDraggableCaretPosition(Func<RangeCaretPosition?> getPosition)
    {
        AddAssert("Assert range caret position", () => EqualityComparer<RangeCaretPosition?>.Default.Equals(getPosition(), LyricCaretState.RangeCaretPosition));
    }

    #endregion

    private partial class TestLyricEditorState : Component, ILyricEditorState
    {
        private readonly Bindable<LyricEditorMode> bindableMode = new();

        private readonly Bindable<EditorModeWithEditStep> bindableModeWithEditStep = new();

        public IBindable<LyricEditorMode> BindableMode => bindableMode;

        public IBindable<EditorModeWithEditStep> BindableModeWithEditStep => bindableModeWithEditStep;

        public LyricEditorMode Mode => bindableMode.Value;

        [Resolved]
        private IEditTextModeState editTextModeState { get; set; } = null!;

        public void SwitchMode(LyricEditorMode mode)
        {
            bindableMode.Value = mode;

            bindableMode.BindValueChanged(_ =>
            {
                updateModeWithEditStep();
            }, true);
            editTextModeState.BindableEditStep.BindValueChanged(e =>
            {
                updateModeWithEditStep();
            });
        }

        private void updateModeWithEditStep()
        {
            bindableModeWithEditStep.Value = new EditorModeWithEditStep
            {
                Mode = bindableMode.Value,
                EditStep = getTheEditStep(bindableMode.Value),
                Default = false,
            };

            Enum? getTheEditStep(LyricEditorMode mode) =>
                mode switch
                {
                    LyricEditorMode.View => null,
                    LyricEditorMode.EditText => editTextModeState.EditStep,
                    LyricEditorMode.EditReferenceLyric => null,
                    LyricEditorMode.EditLanguage => throw new NotSupportedException(),
                    LyricEditorMode.EditRuby => throw new NotSupportedException(),
                    LyricEditorMode.EditTimeTag => throw new NotSupportedException(),
                    LyricEditorMode.EditRomaji => throw new NotSupportedException(),
                    LyricEditorMode.EditNote => throw new NotSupportedException(),
                    LyricEditorMode.EditSinger => throw new NotSupportedException(),
                    _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
                };
        }

        public void SwitchEditStep<TEditStep>(TEditStep editStep) where TEditStep : Enum
        {
            if (editStep is not TextEditStep textEditStep)
                throw new NotSupportedException();

            editTextModeState.ChangeEditStep(textEditStep);
        }

        public void NavigateToFix(LyricEditorMode mode)
            => throw new NotSupportedException();
    }
}

/// <summary>
/// The main goal of the test case is testing the behavior if using different <see cref="ICaretPositionAlgorithm"/>
/// Not test all the <see cref="ICaretPositionAlgorithm"/>
/// So we choose the some representative <see cref="ICaretPositionAlgorithm"/>.
/// </summary>
public enum TestCaretType
{
    ViewOnly,

    CaretEnable,

    CaretWithIndex,

    CaretDraggable,
}
