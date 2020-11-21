// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags
{
    public abstract class TimeTagGenerator<T> where T : TimeTagGeneratorConfig
    {
        protected T Config { get; }

        protected TimeTagGenerator(T config)
        {
            Config = config;
        }

        public virtual Tuple<TimeTagIndex, double?>[] CreateTimeTags(Lyric lyric)
        {
            var timeTags = new List<Tuple<TimeTagIndex, double?>>();
            var text = lyric.Text;

            if (text.Length == 0)
                return timeTags.ToArray();

            // create tag at start of lyric
            timeTags.Add(TimeTagsUtils.Create(new TimeTagIndex(0, TimeTagIndex.IndexState.Start), null));

            if (Config.CheckLineEndKeyUp)
                timeTags.Add(TimeTagsUtils.Create(new TimeTagIndex(text.Length - 1, TimeTagIndex.IndexState.End), null));

            TimeTagLogic(lyric, timeTags);

            return timeTags.OrderBy(x => x.Item1).ToArray();
        }

        protected abstract void TimeTagLogic(Lyric lyric, List<Tuple<TimeTagIndex, double?>> timeTags);
    }
}
