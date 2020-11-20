// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.


using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Generator.TimeTags
{
    public abstract class TimeTagGenerator<T> where T : TimeTagGeneratorConfig
    {
        protected T Config { get; private set; }

        protected TimeTagGenerator(T config)
        {
            Config = config;
        }

        public virtual Tuple<TimeTagIndex, double?>[] CreateTimeTag(Lyric lyric)
        {
            var timeTags = new List<Tuple<TimeTagIndex, double?>>();
            var text = lyric.Text;

            if (text.Length == 0)
                return timeTags.ToArray();

            // create tag at start of lyric
            timeTags.Add(TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), null));

            if(Config.CheckLineEndKeyUp)
                timeTags.Add(TimeTagsUtils.Create(new TimeTagIndex(text.Length - 1, TimeTagIndex.IndexState.End), null));


            TimeTagLogic(lyric, timeTags);

            return timeTags.ToArray();
        }

        protected abstract void TimeTagLogic(Lyric lyric, List<Tuple<TimeTagIndex, double?>> timeTags);

        /// <summary>
        /// Check this character is english
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected static bool IsLatin(char c)
        {
            return c >= 'A' && c <= 'Z' ||
                   c >= 'a' && c <= 'z' ||
                   c >= 'Ａ' && c <= 'Ｚ' ||
                   c >= 'ａ' && c <= 'ｚ';
        }

        /// <summary>
        /// Check this char is symbol
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected static bool IsASCIISymbol(char c)
        {
            return c >= ' ' && c <= '/' ||
                   c >= ':' && c <= '@' ||
                   c >= '[' && c <= '`' ||
                   c >= '{' && c <= '~';
        }
    }
}
