// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor
{
    public class TestSceneLyricEditorScreen : KaraokeEditorScreenTestScene<LyricEditorScreen>
    {
        [Cached]
        private readonly Bindable<LyricEditorMode> bindableLyricEditorMode = new();

        protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

        protected override LyricEditorScreen CreateEditorScreen() => new();

        private DialogOverlay dialogOverlay;
        private LyricCheckerManager lyricCheckManager;

        [BackgroundDependencyLoader]
        private void load()
        {
            base.Content.AddRange(new Drawable[]
            {
                Content,
                dialogOverlay = new DialogOverlay(),
                lyricCheckManager = new LyricCheckerManager(),
            });

            Dependencies.Cache(dialogOverlay);
            Dependencies.Cache(lyricCheckManager);
            Dependencies.Cache(new KaraokeRulesetLyricEditorConfigManager());
            Dependencies.Cache(new KaraokeRulesetEditGeneratorConfigManager());
        }

        [TestCase(LyricEditorMode.View)]
        [TestCase(LyricEditorMode.Manage)]
        [TestCase(LyricEditorMode.Typing)]
        [TestCase(LyricEditorMode.Language)]
        [TestCase(LyricEditorMode.EditRuby)]
        [TestCase(LyricEditorMode.EditRomaji)]
        [TestCase(LyricEditorMode.EditTimeTag)]
        [TestCase(LyricEditorMode.EditNote)]
        [TestCase(LyricEditorMode.Singer)]
        public void TestSwitchMode(LyricEditorMode mode)
        {
            AddStep($"switch to mode {Enum.GetName(typeof(LyricEditorMode), mode)}", () =>
            {
                bindableLyricEditorMode.Value = mode;
            });
            AddWaitStep("wait for switch to new mode", 5);
        }
    }
}
