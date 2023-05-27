// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Notes;

public class NoteGenerator : LyricPropertyGenerator<Note[], NoteGeneratorConfig>
{
    public NoteGenerator(NoteGeneratorConfig config)
        : base(config)
    {
    }

    protected override LocalisableString? GetInvalidMessageFromItem(Lyric item)
    {
        var timeTags = item.TimeTags;

        if (item.TimeTags.Count < 2)
            return "Sorry, lyric must have at least two time-tags.";

        if (timeTags.Any(x => x.Time == null))
            return "All time-tag should have the time.";

        return null;
    }

    protected override Note[] GenerateFromItem(Lyric item)
    {
        var timeTags = TimeTagsUtils.ToTimeBasedDictionary(item.TimeTags);
        var notes = new List<Note>();

        foreach (var timeTag in timeTags)
        {
            // should not continue if
            if (timeTags.LastOrDefault().Key == timeTag.Key)
                break;

            (double _, var textIndex) = timeTag;
            (double _, var nextTextIndex) = timeTags.GetNext(timeTag);

            int startIndex = TextIndexUtils.ToGapIndex(textIndex);
            int endIndex = TextIndexUtils.ToGapIndex(nextTextIndex);

            // prevent reverse time-tag to generate the note.
            if (startIndex >= endIndex)
                continue;

            int timeTagIndex = timeTags.IndexOf(timeTag);
            string text = item.Text[startIndex..endIndex];
            string? ruby = item.RubyTags?.Where(x => x.StartIndex == startIndex && x.EndIndex == endIndex).FirstOrDefault()?.Text;

            if (!string.IsNullOrEmpty(text))
            {
                notes.Add(new Note
                {
                    Text = text,
                    RubyText = ruby,
                    ReferenceLyricId = item.ID,
                    // technically this property should be assigned by beatmap processor, but should be OK to assign here for testing purpose.
                    ReferenceLyric = item,
                    ReferenceTimeTagIndex = timeTagIndex
                });
            }
        }

        return notes.ToArray();
    }
}
