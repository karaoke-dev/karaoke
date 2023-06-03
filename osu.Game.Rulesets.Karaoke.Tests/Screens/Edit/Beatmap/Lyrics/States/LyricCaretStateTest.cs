// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.CaretPosition;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap.Lyrics.States;

[HeadlessTest]
public partial class LyricCaretStateTest : OsuTestScene
{
    private readonly IBeatmap beatmap;
    private ILyricEditorState state = null!;
    private LyricCaretState lyricCaretState = null!;

    public LyricCaretStateTest()
    {
        beatmap = new KaraokeBeatmap
        {
            BeatmapInfo =
            {
                Ruleset = new KaraokeRuleset().RulesetInfo,
            },
            HitObjects = new List<KaraokeHitObject>
            {
                new Lyric
                {
                    Text = "First lyric",
                },
                new Lyric
                {
                    Text = "Second lyric"
                },
                new Lyric
                {
                    Text = "Third lyric"
                },
            }
        };
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Dependencies.Cache(new EditorBeatmap(beatmap));
        Dependencies.Cache(new EditorClock());
        Dependencies.CacheAs(state = new TestLyricEditorState());
        Dependencies.CacheAs<ITextingModeState>(new TextingModeState());
        Dependencies.CacheAs<ITimeTagModeState>(new TimeTagModeState());
        Dependencies.Cache(new KaraokeRulesetLyricEditorConfigManager());

        var lyricsProvider = new LyricsProvider();
        Dependencies.CacheAs<ILyricsProvider>(lyricsProvider);

        Children = new Drawable[]
        {
            lyricsProvider,
            lyricCaretState = new LyricCaretState()
        };
    }

    #region Changing mode

    [Test]
    public void TestChangeFromViewModeToEditMode()
    {
        // change from view mode to another mode that contains algorithm.
        changeMode(LyricEditorMode.View);
        changeMode(LyricEditorMode.Texting);

        // get the action
        assertCaretPosition(Assert.IsNotNull);
        assertHoverCaretPosition(Assert.IsNull);
    }

    [Test]
    public void TestChangeFromEditModeToViewMode()
    {
        // change from edit mode to view mode for checking that caret position should be clear.
        changeMode(LyricEditorMode.Texting);
        changeMode(LyricEditorMode.View);

        // get the action
        assertCaretPosition(Assert.IsNull);
        assertHoverCaretPosition(Assert.IsNull);
    }

    [Test]
    public void TestChangeFromEditModeToEditMode()
    {
        // change from edit mode to view mode for checking that caret position should be clear.
        changeMode(LyricEditorMode.Texting);
        changeMode(LyricEditorMode.EditRuby);

        // get the action
        assertCaretPosition(Assert.IsInstanceOf<NavigateCaretPosition>);
        assertHoverCaretPosition(Assert.IsNull);
    }

    #endregion

    #region Moving caret with action

    [Test]
    public void MovingCaretWithViewMode()
    {
        changeMode(LyricEditorMode.View);
        movingCaret(MovingCaretAction.FirstLyric);

        // get the action
        assertCaretPosition(Assert.IsNull);
        assertHoverCaretPosition(Assert.IsNull);
    }

    [Test]
    public void MovingCaretWithEditMode()
    {
        changeMode(LyricEditorMode.Texting);
        movingCaret(MovingCaretAction.FirstLyric);

        // get the action
        assertCaretPosition(Assert.IsInstanceOf<TypingCaretPosition>);
        assertHoverCaretPosition(Assert.IsNull);
    }

    #endregion

    #region Moving caret by lyric

    [Test]
    public void MovingCaretByLyricWithViewMode()
    {
        changeMode(LyricEditorMode.View);

        var targetLyric = getLyricFromBeatmap(1);
        movingCaretByLyric(targetLyric, () => true);

        // get the action
        assertCaretPosition(Assert.IsNull);
        assertHoverCaretPosition(Assert.IsNull);
    }

    [Test]
    public void MovingCaretByLyricWithEditMode()
    {
        changeMode(LyricEditorMode.Texting);

        var targetLyric = getLyricFromBeatmap(1);
        movingCaretByLyric(targetLyric, () => true);

        // get the action
        assertCaretPosition(Assert.IsInstanceOf<TypingCaretPosition>);
        assertHoverCaretPosition(Assert.IsNull);
    }

    #endregion

    #region Moving caret by caret position

    [Test]
    public void MovingCaretByCaretPositionWithViewMode()
    {
        changeMode(LyricEditorMode.View);

        var targetLyric = getLyricFromBeatmap(1);
        movingCaretByLyric(targetLyric, () => true);

        // should not change the caret position if algorithm is null.
        assertCaretPosition(Assert.IsNull);
        assertHoverCaretPosition(Assert.IsNull);
    }

    [Test]
    public void MovingCaretByCaretPositionWithEditMode()
    {
        changeMode(LyricEditorMode.Singer);

        var targetLyric = getLyricFromBeatmap(1);
        movingCaretByLyric(targetLyric, () => true);

        // yes, should change the position if contains algorithm.
        assertCaretPosition(Assert.IsInstanceOf<NavigateCaretPosition>);
        assertHoverCaretPosition(Assert.IsNull);
    }

    [Test]
    public void MovingCaretByCaretPositionWithWrongCaretPosition()
    {
        changeMode(LyricEditorMode.Texting);

        var targetLyric = getLyricFromBeatmap(1);

        // should throw the exception if caret position type not match.
        movingCaretByLyric(targetLyric, new TextIndex(), () => false);
    }

    [Test]
    public void AdjustEndCaretPositionWithNotSupportedMode()
    {
        changeMode(LyricEditorMode.Texting);

        var targetLyric = getLyricFromBeatmap(1);
        movingCaretByLyric(targetLyric, () => true);

        // not throw the exception if the caret algorithm does not support the adjust the end index.
        adjustCaretEndIndex(new TextIndex(), () => true);
    }

    #endregion

    #region Moving hover caret by caret position

    [Test]
    public void MovingHoverCaretByCaretPositionWithViewMode()
    {
        changeMode(LyricEditorMode.View);

        var targetLyric = getLyricFromBeatmap(1);
        movingHoverCaretByLyric(targetLyric, () => true);

        // should not change the caret position if algorithm is null.
        assertCaretPosition(Assert.IsNull);
        assertHoverCaretPosition(Assert.IsNull);
    }

    [Test]
    public void MovingHoverCaretByCaretPositionWithEditMode()
    {
        changeMode(LyricEditorMode.Singer);

        var firstLyric = getLyricFromBeatmap(0);
        var targetLyric = getLyricFromBeatmap(1);
        movingHoverCaretByLyric(targetLyric, () => true);

        // because switch to the singer lyric, so current caret position will at the first lyric.
        assertCaretPosition(Assert.IsInstanceOf<NavigateCaretPosition>);
        assertCaretPosition(x => Assert.AreEqual(firstLyric, x?.Lyric));

        // yes, should change the position if contains algorithm.
        assertHoverCaretPosition(Assert.IsInstanceOf<NavigateCaretPosition>);
        assertHoverCaretPosition(x => Assert.AreEqual(targetLyric, x?.Lyric));
    }

    [Test]
    public void MovingHoverCaretByCaretPositionWithWrongCaretPosition()
    {
        changeMode(LyricEditorMode.Texting);

        var targetLyric = getLyricFromBeatmap(1);

        // should throw the exception if caret position type not match.
        movingHoverCaretByLyric(targetLyric, new TextIndex(), () => false);
    }

    #endregion

    #region Test utility

    private void changeMode(LyricEditorMode mode)
    {
        AddStep("Switch to edit mode", () =>
        {
            state.SwitchMode(mode);
        });
    }

    private void movingCaret(MovingCaretAction action)
    {
        AddStep("Moving caret by action", () =>
        {
            lyricCaretState.MoveCaret(action);
        });
    }

    private void movingCaretByLyric(Lyric lyric, Func<bool> expected)
    {
        AddStep("Moving caret by action", () =>
        {
            try
            {
                lyricCaretState.MoveCaretToTargetPosition(lyric);
                Assert.IsTrue(expected());
            }
            catch
            {
                Assert.IsFalse(expected());
            }
        });
    }

    private void movingCaretByLyric<TItem>(Lyric lyric, TItem item, Func<bool> expected)
        where TItem : notnull
    {
        AddStep("Moving caret by caret position", () =>
        {
            try
            {
                lyricCaretState.MoveCaretToTargetPosition(lyric, item);
                Assert.IsTrue(expected());
            }
            catch
            {
                Assert.IsFalse(expected());
            }
        });
    }

    private void adjustCaretEndIndex<TItem>(TItem item, Func<bool> expected)
        where TItem : notnull
    {
        AddStep("Moving caret by caret position", () =>
        {
            try
            {
                lyricCaretState.AdjustCaretEndIndex(item);
                Assert.IsTrue(expected());
            }
            catch
            {
                Assert.IsFalse(expected());
            }
        });
    }

    private void movingHoverCaretByLyric(Lyric lyric, Func<bool> expected)
    {
        AddStep("Moving hover caret by action", () =>
        {
            try
            {
                lyricCaretState.MoveHoverCaretToTargetPosition(lyric);
                Assert.IsTrue(expected());
            }
            catch
            {
                Assert.IsFalse(expected());
            }
        });
    }

    private void movingHoverCaretByLyric<TItem>(Lyric lyric, TItem item, Func<bool> expected)
        where TItem : notnull
    {
        AddStep("Moving hover caret by caret position", () =>
        {
            try
            {
                lyricCaretState.MoveHoverCaretToTargetPosition(lyric, item);
                Assert.IsTrue(expected());
            }
            catch
            {
                Assert.IsFalse(expected());
            }
        });
    }

    private Lyric getLyricFromBeatmap(int index)
        => (Lyric)beatmap.HitObjects[index];

    private void assertCaretPosition(Action<ICaretPosition?> caretPosition)
    {
        AddStep("Assert caret position", () =>
        {
            caretPosition(lyricCaretState.BindableCaretPosition.Value);
        });
    }

    private void assertHoverCaretPosition(Action<ICaretPosition?> caretPosition)
    {
        AddStep("Assert hover caret position", () =>
        {
            caretPosition(lyricCaretState.BindableHoverCaretPosition.Value);
        });
    }

    #endregion

    private class TestLyricEditorState : ILyricEditorState
    {
        private readonly Bindable<LyricEditorMode> bindableMode = new();

        private readonly Bindable<ModeWithSubMode> bindableModeWitSubMode = new();

        public IBindable<LyricEditorMode> BindableMode => bindableMode;

        public IBindable<ModeWithSubMode> BindableModeAndSubMode => bindableModeWitSubMode;

        public LyricEditorMode Mode => bindableMode.Value;

        public void SwitchMode(LyricEditorMode mode)
        {
            bindableMode.Value = mode;
            bindableModeWitSubMode.Value = bindableModeWitSubMode.Value with
            {
                Mode = mode,
                SubMode = getTheSubMode(mode)
            };
        }

        private static Enum? getTheSubMode(LyricEditorMode mode) =>
            mode switch
            {
                LyricEditorMode.View => null,
                LyricEditorMode.Texting => TextingEditMode.Typing,
                LyricEditorMode.Reference => null,
                LyricEditorMode.Language => LanguageEditMode.Generate,
                LyricEditorMode.EditRuby => RubyTagEditMode.Generate,
                LyricEditorMode.EditRomaji => RomajiTagEditMode.Generate,
                LyricEditorMode.EditTimeTag => TimeTagEditMode.Create,
                LyricEditorMode.EditNote => NoteEditMode.Generate,
                LyricEditorMode.Singer => null,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

        public void SwitchSubMode<TSubMode>(TSubMode subMode) where TSubMode : Enum
        {
            // there's no need to do anything.
        }

        public void NavigateToFix(LyricEditorMode mode)
            => throw new NotImplementedException();
    }
}
