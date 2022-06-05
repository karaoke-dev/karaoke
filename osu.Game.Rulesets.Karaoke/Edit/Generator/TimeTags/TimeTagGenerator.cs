// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Types;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Localisation;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags
{
    public abstract class TimeTagGenerator<T> : ILyricPropertyGenerator<TimeTag[]> where T : TimeTagGeneratorConfig
    {
        protected T Config { get; }

        protected TimeTagGenerator(T config)
        {
            Config = config;
        }

        public LocalisableString? GetInvalidMessage(Lyric lyric)
        {
            if (string.IsNullOrEmpty(lyric.Text))
                return TimeTagGeneratorStrings.LyricShouldNotBeEmpty;

            return null;
        }

        public virtual TimeTag[] Generate(Lyric lyric)
        {
            var timeTags = new List<TimeTag>();
            string text = lyric.Text;

            if (string.IsNullOrEmpty(text))
                return timeTags.ToArray();

            if (string.IsNullOrWhiteSpace(text))
            {
                if (Config.CheckBlankLine)
                    timeTags.Add(new TimeTag(new TextIndex(0)));

                return timeTags.ToArray();
            }

            // create tag at start of lyric
            timeTags.Add(new TimeTag(new TextIndex(0)));

            if (Config.CheckLineEndKeyUp)
                timeTags.Add(new TimeTag(new TextIndex(text.Length - 1, TextIndex.IndexState.End)));

            TimeTagLogic(lyric, timeTags);

            return timeTags.OrderBy(x => x.Index).ToArray();
        }

        protected abstract void TimeTagLogic(Lyric lyric, List<TimeTag> timeTags);
    }
}
