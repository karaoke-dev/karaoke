// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using LrcParser.Model;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using Lyric = osu.Game.Rulesets.Karaoke.Objects.Lyric;
using RubyTag = osu.Game.Rulesets.Karaoke.Objects.RubyTag;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Formats;

public class LrcDecoder : Decoder<Beatmap>
{
    public static void Register()
    {
        // Lrc decoder looks like [mm:ss:__]
        AddDecoder<Beatmap>("[", _ => new LrcDecoder());
    }

    protected override void ParseStreamInto(LineBufferedReader stream, Beatmap output)
    {
        // Clear all hitobjects
        output.HitObjects.Clear();

        string lyricText = stream.ReadToEnd();
        var result = new LrcParser.Parser.Lrc.LrcParser().Decode(lyricText);

        var newLyrics = result.Lyrics.Select(lrcLyric =>
        {
            var lrcTimeTags = lrcLyric.TimeTags.Select(convertTimeTag).ToArray();
            var lrcRubies = lrcLyric.RubyTags.Select(convertRubyTag).ToArray();
            var lrcRubyTimeTags = lrcLyric.RubyTags.Select(convertTimeTagsFromRubyTags).SelectMany(x => x).ToArray();

            return new Lyric
            {
                Order = output.HitObjects.Count + 1, // should create default order.
                Text = lrcLyric.Text,
                TimeTags = TimeTagsUtils.Sort(lrcTimeTags.Concat(lrcRubyTimeTags)),
                RubyTags = lrcRubies,
            };
        });

        output.HitObjects.AddRange(newLyrics);

        static TimeTag convertTimeTag(KeyValuePair<TextIndex, int?> timeTag)
            => new(convertTextIndex(timeTag.Key), timeTag.Value);

        static Framework.Graphics.Sprites.TextIndex convertTextIndex(TextIndex textIndex)
        {
            int index = textIndex.Index;
            var state = textIndex.State == IndexState.Start ? Framework.Graphics.Sprites.TextIndex.IndexState.Start : Framework.Graphics.Sprites.TextIndex.IndexState.End;

            return new Framework.Graphics.Sprites.TextIndex(index, state);
        }

        static RubyTag convertRubyTag(LrcParser.Model.RubyTag rubyTag)
            => new()
            {
                Text = rubyTag.Text,
                StartIndex = rubyTag.StartCharIndex,
                EndIndex = rubyTag.EndCharIndex,
            };

        static TimeTag[] convertTimeTagsFromRubyTags(LrcParser.Model.RubyTag rubyTag)
        {
            int startIndex = rubyTag.StartCharIndex;
            return rubyTag.TimeTags.Select(x => convertTimeTag(new KeyValuePair<TextIndex, int?>(new TextIndex(startIndex), x.Value))).ToArray();
        }
    }
}
