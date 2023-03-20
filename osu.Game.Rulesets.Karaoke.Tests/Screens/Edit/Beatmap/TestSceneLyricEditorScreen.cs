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
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Tests.Screens.Edit.Beatmap;

public partial class TestSceneLyricEditorScreen : BeatmapEditorScreenTestScene<LyricEditorScreen>
{
    [Cached]
    private readonly Bindable<LyricEditorMode> bindableLyricEditorMode = new();

    protected override Container<Drawable> Content { get; } = new Container { RelativeSizeAxes = Axes.Both };

    protected override LyricEditorScreen CreateEditorScreen() => new();

    private DialogOverlay dialogOverlay = null!;
    private LyricsProvider lyricsProvider = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        base.Content.AddRange(new Drawable[]
        {
            Content,
            dialogOverlay = new DialogOverlay(),
            lyricsProvider = new LyricsProvider(),
        });

        Dependencies.CacheAs<IDialogOverlay>(dialogOverlay);
        Dependencies.CacheAs<ILyricsProvider>(lyricsProvider);
        Dependencies.Cache(new KaraokeRulesetLyricEditorConfigManager());
        Dependencies.Cache(new KaraokeRulesetEditGeneratorConfigManager());
    }

    [Test]
    public void TestViewMode()
    {
        switchToMode(LyricEditorMode.View);
    }

    [Test]
    public void TestTextingMode()
    {
        switchToMode(LyricEditorMode.Texting);
        clickEditModeButtons<TextingEditMode>();
    }

    [Test]
    public void TestReferenceMode()
    {
        switchToMode(LyricEditorMode.Reference);
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
        clickEditModeButtons<RubyTagEditMode>();
    }

    [Test]
    public void TestEditRomajiMode()
    {
        switchToMode(LyricEditorMode.EditRomaji);
        clickEditModeButtons<RomajiTagEditMode>();
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
        AddStep($"switch to mode {Enum.GetName(mode)}", () =>
        {
            bindableLyricEditorMode.Value = mode;
        });
        AddWaitStep("wait for switch to new mode", 10);
    }

    private void clickEditModeButtons<T>() where T : struct, Enum
    {
        foreach (var editMode in Enum.GetValues<T>())
        {
            clickTargetEditModeButton(editMode);
        }
    }

    private void clickTargetEditModeButton<T>(T editMode) where T : struct, Enum
    {
        AddStep("Click the button", () =>
        {
            var editModeSection = this.ChildrenOfType<LyricEditorEditModeSection<T>>().Single();
            editModeSection.UpdateEditMode(editMode);
        });
        AddWaitStep("wait for switch to new edit mode.", 10);
    }
}
