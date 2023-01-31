// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Edit.Checks.Issues;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Tests.Beatmaps;
using static osu.Game.Rulesets.Karaoke.Edit.Checks.CheckBeatmapNoteInfo;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

public class CheckBeatmapNoteInfoTest : BeatmapPropertyCheckTest<CheckBeatmapNoteInfo>
{
    [Test]
    public void TestCheckColumnNotEnough()
    {
        var beatmap = createTestingBeatmap(x => x.Columns = MIN_COLUMNS - 1, null);
        AssertNotOk<IssueTemplateColumnNotEnough>(getContext(beatmap));
    }

    [Test]
    public void TestCheckColumnExceed()
    {
        var beatmap = createTestingBeatmap(x => x.Columns = MAX_COLUMNS + 1, null);
        AssertNotOk<IssueTemplateColumnExceed>(getContext(beatmap));
    }

    [Test]
    public void TestCheckNoteToneTooLow()
    {
        var beatmap = createTestingBeatmap(x => x.Columns = MIN_COLUMNS, new[]
        {
            new Note
            {
                Tone = new Tone(-MIN_COLUMNS)
            }
        });
        AssertNotOk<NoteIssue, IssueTemplateNoteToneTooLow>(getContext(beatmap));
    }

    [Test]
    public void TestCheckNoteToneTooHigh()
    {
        var beatmap = createTestingBeatmap(x => x.Columns = MIN_COLUMNS, new[]
        {
            new Note
            {
                Tone = new Tone(MIN_COLUMNS)
            }
        });
        AssertNotOk<NoteIssue, IssueTemplateNoteToneTooHigh>(getContext(beatmap));
    }

    private static IBeatmap createTestingBeatmap(Action<NoteInfo> action, IEnumerable<Note>? notes)
    {
        var karaokeBeatmap = new KaraokeBeatmap
        {
            BeatmapInfo =
            {
                Ruleset = new KaraokeRuleset().RulesetInfo,
            },
            HitObjects = notes?.OfType<KaraokeHitObject>().ToList() ?? new List<KaraokeHitObject>(),
        };

        action(karaokeBeatmap.NoteInfo);

        return new EditorBeatmap(karaokeBeatmap);
    }

    private static BeatmapVerifierContext getContext(IBeatmap beatmap)
        => new(beatmap, new TestWorkingBeatmap(beatmap));
}
