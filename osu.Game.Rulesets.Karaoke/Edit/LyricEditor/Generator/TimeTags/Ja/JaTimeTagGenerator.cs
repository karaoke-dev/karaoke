// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Generator.TimeTags.Ja
{
    /// <summary>
    /// Thanks for RhythmKaTTE's author
    /// http://juna-idler.blogspot.com/2016/05/rhythmkatte-version-01.html
    /// </summary>
    public class JaTimeTagGenerator : TimeTagGenerator<JaTimeTagGeneratorConfig>
    {
        public JaTimeTagGenerator(JaTimeTagGeneratorConfig config)
            : base(config)
        {
        }

        public Tuple<TimeTagIndex, double?>[] CreateTimeTagFromJapaneseLyric(Lyric lyric)
        {
            // todo : might add some setting in enum
            throw new NotImplementedException();
        }
    }
}
