// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics
{
    public partial class LyricRubyTagsChangeHandler : LyricTextTagsChangeHandler<RubyTag>, ILyricRubyTagsChangeHandler
    {
        #region Auto-Generate

        public bool CanGenerate()
        {
            var generator = GetSelector<RubyTag[], RubyTagGeneratorConfig>();
            return CanGenerate(generator);
        }

        public IDictionary<Lyric, LocalisableString> GetGeneratorNotSupportedLyrics()
        {
            var generator = GetSelector<RubyTag[], RubyTagGeneratorConfig>();
            return GetInvalidMessageFromGenerator(generator);
        }

        public void AutoGenerate()
        {
            var generator = GetSelector<RubyTag[], RubyTagGeneratorConfig>();

            PerformOnSelection(lyric =>
            {
                lyric.RubyTags = generator.Generate(lyric);
            });
        }

        #endregion

        protected override bool ContainsInLyric(Lyric lyric, RubyTag textTag)
            => lyric.RubyTags.Contains(textTag);

        protected override void AddToLyric(Lyric lyric, RubyTag textTag)
            => lyric.RubyTags.Add(textTag);

        protected override void RemoveFromLyric(Lyric lyric, RubyTag textTag)
            => lyric.RubyTags.Remove(textTag);

        protected override bool IsWritePropertyLocked(Lyric lyric)
            => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.RubyTags));
    }
}
