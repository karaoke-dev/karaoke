// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags
{
    public abstract class TimeTagGeneratorConfig
    {
        /// <summary>
        /// Will create a <see cref="TimeTag"/> at the first of the lyric if only contains spacing in the <see cref="Lyric"/>.
        /// </summary>
        public bool CheckBlankLine { get; set; }

        /// <summary>
        /// Add end <see cref="TimeTag"/> at the end of the <see cref="Lyric"/>.
        /// </summary>
        public bool CheckLineEndKeyUp { get; set; }

        /// <summary>
        /// Will add the <see cref="TimeTag"/> if meet the spacing.
        /// </summary>
        public bool CheckWhiteSpace { get; set; }

        /// <summary>
        /// Add the end <see cref="TimeTag"/> instead.
        /// This feature will work only if enable the <see cref="CheckWhiteSpace"/>.
        /// </summary>
        public bool CheckWhiteSpaceKeyUp { get; set; }
    }
}
