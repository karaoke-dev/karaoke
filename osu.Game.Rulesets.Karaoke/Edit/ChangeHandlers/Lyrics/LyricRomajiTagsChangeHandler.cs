// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RomajiTags;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public partial class LyricRomajiTagsChangeHandler : LyricTextTagsChangeHandler<RomajiTag>, ILyricRomajiTagsChangeHandler
    {
        #region Auto-Generate

        public bool CanGenerate()
        {
            var generator = GetSelector<RomajiTag[], RomajiTagGeneratorConfig>();
            return CanGenerate(generator);
        }

        public IDictionary<Lyric, LocalisableString> GetGeneratorNotSupportedLyrics()
        {
            var generator = GetSelector<RomajiTag[], RomajiTagGeneratorConfig>();
            return GetInvalidMessageFromGenerator(generator);
        }

        public void AutoGenerate()
        {
            var generator = GetSelector<RomajiTag[], RomajiTagGeneratorConfig>();

            PerformOnSelection(lyric =>
            {
                lyric.RomajiTags = generator.Generate(lyric);
            });
        }

        #endregion

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
