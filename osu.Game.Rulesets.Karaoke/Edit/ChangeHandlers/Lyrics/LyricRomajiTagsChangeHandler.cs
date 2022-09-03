// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public class LyricRomajiTagsChangeHandler : LyricTextTagsChangeHandler<RomajiTag>, ILyricRomajiTagsChangeHandler
    {
        protected override bool ContainsInLyric(Lyric lyric, RomajiTag textTag)
            => lyric.RomajiTags.Contains(textTag);

        protected override void AddToLyric(Lyric lyric, RomajiTag textTag)
            => lyric.RomajiTags.Add(textTag);

        protected override void RemoveFromLyric(Lyric lyric, RomajiTag textTag)
            => lyric.RomajiTags.Remove(textTag);

        protected override bool IsWritePropertyLocked(Lyric lyric)
            => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.RomajiTags));
    }
}
