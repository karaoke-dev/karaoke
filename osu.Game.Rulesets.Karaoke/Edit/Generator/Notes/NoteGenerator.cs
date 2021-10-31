// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
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
            var timeTags = TimeTagsUtils.ToDictionary(lyric.TimeTags);
            var notes = new List<Note>();

            foreach (var timeTag in timeTags)
            {
                var (key, endTime) = timeTags.GetNext(timeTag);

                if (key.Index <= 0)
                    continue;

                var startTime = timeTag.Value;

                int startIndex = timeTag.Key.Index;
                int endIndex = TextIndexUtils.ToStringIndex(key);

                var text = lyric.Text[startIndex..endIndex];
                var ruby = lyric.RubyTags?.Where(x => x.StartIndex == startIndex && x.EndIndex == endIndex).FirstOrDefault()?.Text;

                if (!string.IsNullOrEmpty(text))
                {
                    notes.Add(new Note
                    {
                        StartTime = startTime,
                        Duration = endTime - startTime,
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
