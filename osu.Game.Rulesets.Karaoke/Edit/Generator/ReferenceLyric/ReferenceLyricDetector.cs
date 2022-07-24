// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.ReferenceLyric
{
    public class ReferenceLyricDetector : ILyricPropertyDetector<Lyric>
    {
        private readonly Lyric[] lyrics;
        private readonly ReferenceLyricDetectorConfig config;

        public ReferenceLyricDetector(IEnumerable<Lyric> lyrics, ReferenceLyricDetectorConfig config)
        {
            this.lyrics = lyrics.ToArray();
            this.config = config;
        }

        public LocalisableString? GetInvalidMessage(Lyric lyric)
        {
            var referencedLyric = getReferenceLyric(lyric);
            if (referencedLyric == null)
                return "There's no matched lyric.";

            return null;
        }

        public Lyric Detect(Lyric lyric)
        {
            var referencedLyric = getReferenceLyric(lyric);
            if (referencedLyric == null)
                throw new NullReferenceException();

            return referencedLyric;
        }

        private Lyric? getReferenceLyric(Lyric lyric)
        {
            if (!lyrics.Contains(lyric))
                throw new InvalidOperationException();

            return lyrics.Except(new[] { lyric }).OrderBy(x => x.Order).FirstOrDefault(x => canBeReferenced(lyric, x));
        }

        private bool canBeReferenced(Lyric lyric, Lyric referencedLyric)
        {
            string lyricText = lyric.Text;
            string referencedLyricText = referencedLyric.Text;

            if (lyricText == referencedLyricText)
                return true;

            if (!config.IgnorePrefixAndPostfixSymbol)
                return false;

            // check if contains intersect part between two lyrics.
            if (!lyricText.Contains(referencedLyricText) && !referencedLyricText.Contains(lyricText))
                return false;

            // check if except part are all symbols.
            var except1 = lyricText.Except(referencedLyricText);
            var except2 = referencedLyricText.Except(lyricText);
            return allCharsEmptyOrSymbol(except1) && allCharsEmptyOrSymbol(except2);

            static bool allCharsEmptyOrSymbol(IEnumerable<char> chars)
                => chars.All(x => CharUtils.IsSpacing(x) || CharUtils.IsAsciiSymbol(x));
        }
    }
}
