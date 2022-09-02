// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Utils
{
    public static class EditorBeatmapUtils
    {
        public static IEnumerable<Lyric> GetAllReferenceLyrics(EditorBeatmap editorBeatmap, Lyric referencedLyric)
            => editorBeatmap.HitObjects.OfType<Lyric>().Where(x => x.ReferenceLyric == referencedLyric);

        public static IEnumerable<Note> GetNotesByLyric(EditorBeatmap editorBeatmap, Lyric lyric)
            => editorBeatmap.HitObjects.OfType<Note>().Where(x => x.ReferenceLyric == lyric);
    }
}
