// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using System.Text.RegularExpressions;

namespace osu.Game.Rulesets.Karaoke.Tests.Helper
{
    public static class TestCaseTagHelper
    {
        public static RubyTag ParseRubyTag(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new RubyTag();

            var regex = new Regex("(?<start>[-0-9]+),(?<end>[-0-9]+)]:(?<ruby>.*$)");
            var result = regex.Match(str);
            if (!result.Success)
                return new RubyTag();

            var startIndex = int.Parse(result.Groups["start"]?.Value);
            var endIndex = int.Parse(result.Groups["end"]?.Value);
            var text = result.Groups["ruby"]?.Value;

            return new RubyTag
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                Text = text
            };
        }

        public static RomajiTag ParseRomajiTag(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new RomajiTag();

            var regex = new Regex("(?<start>[-0-9]+),(?<end>[-0-9]+)]:(?<romaji>.*$)");
            var result = regex.Match(str);
            if (!result.Success)
                return new RomajiTag();

            var startIndex = int.Parse(result.Groups["start"]?.Value);
            var endIndex = int.Parse(result.Groups["end"]?.Value);
            var text = result.Groups["romaji"]?.Value;

            return new RomajiTag
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                Text = text
            };
        }

        public static TimeTag ParseTimeTag(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new TimeTag(new TimeTagIndex());

            var regex = new Regex("(?<index>[-0-9]+),(?<state>start|end)]:(?<time>[-0-9]+|s*|)");
            var result = regex.Match(str);
            if (!result.Success)
                return new TimeTag(new TimeTagIndex());

            var index = int.Parse(result.Groups["index"]?.Value);
            var state = result.Groups["state"]?.Value == "start" ? TimeTagIndex.IndexState.Start : TimeTagIndex.IndexState.End;
            var timeStr = result.Groups["time"]?.Value;
            var time = timeStr == "" ? default(int?) : int.Parse(timeStr);

            return new TimeTag(new TimeTagIndex(index, state), time);
        }
    }
}
