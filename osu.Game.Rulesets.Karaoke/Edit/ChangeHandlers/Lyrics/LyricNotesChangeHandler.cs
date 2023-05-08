// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Notes;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

public partial class LyricNotesChangeHandler : LyricPropertyChangeHandler, ILyricNotesChangeHandler
{
    #region Auto-Generate

    [Resolved]
    private EditorBeatmap beatmap { get; set; } = null!;

    public bool CanGenerate()
    {
        var generator = GetGenerator<Note[], NoteGeneratorConfig>();
        return CanGenerate(generator);
    }

    public IDictionary<Lyric, LocalisableString> GetGeneratorNotSupportedLyrics()
    {
        var generator = GetGenerator<Note[], NoteGeneratorConfig>();
        return GetInvalidMessageFromGenerator(generator);
    }

    public void AutoGenerate()
    {
        var generator = GetGenerator<Note[], NoteGeneratorConfig>();

        PerformOnSelection(lyric =>
        {
            // clear exist notes if from those
            var matchedNotes = EditorBeatmapUtils.GetNotesByLyric(beatmap, lyric);
            RemoveRange(matchedNotes);

            var notes = generator.Generate(lyric);
            AddRange(notes);
        });
    }

    #endregion

    protected override bool IsWritePropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.IsCreateOrRemoveNoteLocked(lyric);
}
