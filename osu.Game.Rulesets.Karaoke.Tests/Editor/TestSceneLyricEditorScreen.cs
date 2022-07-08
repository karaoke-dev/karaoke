// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    public class TestSceneLyricEditorScreen : KaraokeEditorScreenTestScene<LyricEditorScreen>
    {
        [Cached]
        private readonly Bindable<LyricEditorMode> bindableLyricEditorMode = new();

        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        protected override LyricEditorScreen CreateEditorScreen() => new();

        private DialogOverlay dialogOverlay = null!;
        private LyricsProvider lyricsProvider = null!;
        private LyricCheckerManager lyricCheckManager = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            base.Content.AddRange(new Drawable[]
            {
                Content,
                dialogOverlay = new DialogOverlay(),
                lyricsProvider = new LyricsProvider(),
                lyricCheckManager = new LyricCheckerManager(),
            });

            Dependencies.CacheAs<IDialogOverlay>(dialogOverlay);
            Dependencies.CacheAs<ILyricsProvider>(lyricsProvider);
            Dependencies.Cache(lyricCheckManager);
            Dependencies.Cache(new KaraokeRulesetLyricEditorConfigManager());
            Dependencies.Cache(new KaraokeRulesetEditGeneratorConfigManager());
        }

        [Test]
        public void TestViewMode()
        {
            switchToMode(LyricEditorMode.View);
        }

        [Test]
        public void TestManageMode()
        {
            switchToMode(LyricEditorMode.Manage);
        }

        [Test]
        public void TestTypingMode()
        {
            switchToMode(LyricEditorMode.Typing);
        }

        [Test]
        public void TestLanguageMode()
        {
            switchToMode(LyricEditorMode.Language);
            clickEditModeButtons<LanguageEditMode>();
        }

        [Test]
        public void TestEditRubyMode()
        {
            switchToMode(LyricEditorMode.EditRuby);
            clickEditModeButtons<TextTagEditMode>();
        }

        [Test]
        public void TestEditRomajiMode()
        {
            switchToMode(LyricEditorMode.EditRomaji);
            clickEditModeButtons<TextTagEditMode>();
        }

        [Test]
        public void TestEditTimeTagMode()
        {
            switchToMode(LyricEditorMode.EditTimeTag);
            clickEditModeButtons<TimeTagEditMode>();
        }

        [Test]
        public void TestEditNoteMode()
        {
            switchToMode(LyricEditorMode.EditNote);
            clickEditModeButtons<NoteEditMode>();
        }

        [Test]
        public void TestSingerMode()
        {
            switchToMode(LyricEditorMode.Singer);
        }

        private void switchToMode(LyricEditorMode mode)
        {
            AddStep($"switch to mode {Enum.GetName(typeof(LyricEditorMode), mode)}", () =>
            {
                bindableLyricEditorMode.Value = mode;
            });
            AddWaitStep("wait for switch to new mode", 5);
        }

        private void clickEditModeButtons<T>() where T : Enum
        {
            foreach (var editMode in EnumUtils.GetValues<T>())
            {
                clickTargetEditModeButton(editMode);
            }
        }

        private void clickTargetEditModeButton<T>(T editMode) where T : Enum
        {
            AddStep("Click the button", () =>
            {
                var editModeSection = this.ChildrenOfType<EditModeSection<T>>().Single();
                editModeSection.UpdateEditMode(editMode);
            });
            AddWaitStep("wait for switch to new edit mode.", 1);
        }
    }
}
