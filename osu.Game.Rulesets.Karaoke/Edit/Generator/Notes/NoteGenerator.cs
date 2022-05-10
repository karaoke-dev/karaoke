﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Notes
{
    public class NoteGenerator
    {
        protected NoteGeneratorConfig Config { get; }

        public NoteGenerator(NoteGeneratorConfig config)
        {
            Config = config;
        }

        public Note[] CreateNotes(Lyric lyric)
        {
            var timeTags = TimeTagsUtils.ToTimeBasedDictionary(lyric.TimeTags);
            var notes = new List<Note>();

            foreach (var timeTag in timeTags)
            {
                // should not continue if
                if (timeTags.LastOrDefault().Key == timeTag.Key)
                    break;

                (double time, var textIndex) = timeTag;
                (double nextTime, var nextTextIndex) = timeTags.GetNext(timeTag);

                int startIndex = TextIndexUtils.ToStringIndex(textIndex);
                int endIndex = TextIndexUtils.ToStringIndex(nextTextIndex);

                // prevent reverse time-tag to generate the note.
                if (startIndex >= endIndex)
                    continue;

                string text = lyric.Text[startIndex..endIndex];
                string ruby = lyric.RubyTags?.Where(x => x.StartIndex == startIndex && x.EndIndex == endIndex).FirstOrDefault()?.Text;

                if (!string.IsNullOrEmpty(text))
                {
                    notes.Add(new Note
                    {
                        StartTime = time,
                        Duration = nextTime - time,
                        StartIndex = startIndex,
                        EndIndex = endIndex,
                        Text = text,
                        RubyText = ruby,
                        ParentLyric = lyric
                    });
                }
            }

            return notes.ToArray();
        }
    }
}
