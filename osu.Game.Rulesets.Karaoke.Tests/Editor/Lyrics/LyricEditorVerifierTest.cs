// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Lyrics;

[HeadlessTest]
public class LyricEditorVerifierTest : EditorClockTestScene
{
    private Lyric internalLyric = null!;

    private LyricEditorVerifier verifier = null!;

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
        var editorBeatmap = new EditorBeatmap(beatmap);
        Dependencies.Cache(editorBeatmap);

        Add(editorBeatmap);
    }

    [SetUp]
    public virtual void SetUp()
    {
        internalLyric = createLyricWithLanguageIssueOnly();

        AddStep("Setup", () =>
        {
            // Reset editor beatmap.
            var editorBeatmap = Dependencies.Get<EditorBeatmap>();
            editorBeatmap.Clear();
            editorBeatmap.SelectedHitObjects.Clear();
            editorBeatmap.Add(internalLyric);

            // Initialize the verifier.
            RemoveAll(x => x is LyricEditorVerifier, true);
            Add(verifier = new LyricEditorVerifier());
        });
    }

    [Test]
    public void TestFirstLoad()
    {
        // Check should contains language-related issue in the verifier.
        assertHitObjectIssueAmount(internalLyric, 1);
        assertEditModeIssueAmount(LyricEditorMode.Language, 1);

        // Should not contains issue in other edit mode.
        assertEditModeIssueAmount(LyricEditorMode.Texting, 0);
    }

    [Test]
    public void TestAdd()
    {
        var newLyric = createLyricWithLanguageIssueOnly();

        updateEditorBeatmap(editorBeatmap =>
        {
            editorBeatmap.Add(newLyric);
        });

        assertHitObjectIssueAmount(internalLyric, 1);
        assertHitObjectIssueAmount(newLyric, 1);
        assertEditModeIssueAmount(LyricEditorMode.Language, 2);
    }

    [Test]
    public void TestRemove()
    {
        updateEditorBeatmap(editorBeatmap =>
        {
            editorBeatmap.Remove(internalLyric);
        });

        assertEditModeIssueAmount(LyricEditorMode.Language, 0);
    }

    [Test]
    public void TestUpdate()
    {
        updateEditorBeatmap(editorBeatmap =>
        {
            internalLyric.Language = new CultureInfo("ja-JP");
            editorBeatmap.Update(internalLyric);
        });

        assertHitObjectIssueAmount(internalLyric, 0);
        assertEditModeIssueAmount(LyricEditorMode.Language, 0);
    }

    [Test]
    public void TestRefresh()
    {
        AddStep("Fix the language issue and refresh.", () =>
        {
            internalLyric.Language = new CultureInfo("ja-JP");
            verifier.Refresh();
        });

        assertHitObjectIssueAmount(internalLyric, 0);
        assertEditModeIssueAmount(LyricEditorMode.Language, 0);
    }

    [Test]
    public void TestRefreshByHitObject()
    {
        AddStep("Fix the language issue and refresh.", () =>
        {
            internalLyric.Language = new CultureInfo("ja-JP");
            verifier.RefreshByHitObject(internalLyric);
        });

        assertHitObjectIssueAmount(internalLyric, 0);
        assertEditModeIssueAmount(LyricEditorMode.Language, 0);
    }

    #region Tool

    private static Lyric createLyricWithLanguageIssueOnly()
        => new()
        {
            Text = "カラオケ！",
            TimeTags = new List<TimeTag>
            {
                new(new TextIndex(0), 500),
                new(new TextIndex(4, TextIndex.IndexState.End), 2000),
            },
        };

    private void updateEditorBeatmap(Action<EditorBeatmap> action)
    {
        AddStep("Prepare testing beatmap", () =>
        {
            var editorBeatmap = Dependencies.Get<EditorBeatmap>();
            action.Invoke(editorBeatmap);
        });
    }

    #endregion

    #region Assertion

    private void assertHitObjectIssueAmount(KaraokeHitObject karaokeHitObject, int issueCount)
    {
        AddStep("Check hit-object issue amount.", () =>
        {
            var editModeIssues = verifier.GetBindable(karaokeHitObject);
            Assert.AreEqual(issueCount, editModeIssues.Count);
        });
    }

    private void assertEditModeIssueAmount(LyricEditorMode editMode, int issueCount)
    {
        AddStep("Check edit mode issue amount.", () =>
        {
            var editModeIssues = verifier.GetIssueByEditMode(editMode);
            Assert.AreEqual(issueCount, editModeIssues.Count);
        });
    }

    #endregion
}
