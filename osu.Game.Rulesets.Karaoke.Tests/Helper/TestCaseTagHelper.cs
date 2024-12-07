// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Integration.Formats;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Tests.Extensions;

namespace osu.Game.Rulesets.Karaoke.Tests.Helper;

public static class TestCaseTagHelper
{
    private const string char_index_range_str = @"\[(?<start>[-0-9]+)(?:,(?<end>[-0-9]+))?\]";
    private const string time_index_regex = @"\[(?<index>[-0-9]+),(?<state>start|end)]";
    private const string time_range_str = @"\[(?<startTime>[-0-9]+)(?:,(?<endTime>[-0-9]+))?\]";
    private const string id_str = "(?<id>[-0-9]+)]";

    private static string getStringPropertyRegex(char prefix, string propertyName)
        => @$"(?:{prefix}(?<{propertyName}>[^\s]+))?";

    private static string getNumberPropertyRegex(char prefix, string propertyName)
        => $"(?:{prefix}(?<{propertyName}>[-0-9]+|s*|))?";

    private static string generateRegex(string regexPrefix, IEnumerable<string> regexProperties)
        => regexPrefix + string.Join("", regexProperties);

    private static TObject getMatchByStatement<TObject>(string? str, string regexStr, Func<Match?, TObject> returnValue)
    {
        if (string.IsNullOrEmpty(str))
            return returnValue(null);

        var regex = new Regex(regexStr);
        var result = regex.Match(str);
        if (!result.Success)
            throw new RegexMatchTimeoutException(nameof(str));

        return returnValue(result);
    }

    /// <summary>
    /// Process test case ruby string format into <see cref="RubyTag"/>
    /// </summary>
    /// <example>
    /// [0]:ruby        -> has range index with text.<br/>
    /// [0,3]:ruby      -> has range index with text.<br/>
    /// [0]:            -> has range index with empty text.<br/>
    /// [0,3]:          -> has range index with empty text.<br/>
    /// [0]             -> has range index with empty text.<br/>
    /// [0,3]           -> has range index with empty text.<br/>
    /// </example>
    /// <param name="str">Ruby tag string format</param>
    /// <returns><see cref="RubyTag"/>Ruby tag object</returns>
    public static RubyTag ParseRubyTag(string? str)
    {
        string regex = generateRegex(char_index_range_str, new[]
        {
            getStringPropertyRegex(':', "ruby"),
        });

        return getMatchByStatement(str, regex, result =>
        {
            if (result == null)
                return new RubyTag();

            int startIndex = result.GetGroupValue<int>("start");
            int? endIndex = result.GetGroupValue<int?>("end");
            string text = result.GetGroupValue<string>("ruby");

            return new RubyTag
            {
                StartIndex = startIndex,
                EndIndex = endIndex ?? startIndex,
                Text = text,
            };
        });
    }

    /// <summary>
    /// Process test case time tag string format into <see cref="TimeTag"/>
    /// </summary>
    /// <example>
    /// [0,start]:1000              -> has time-tag index with time.<br/>
    /// [0,start]                   -> has time-tag index with no time.<br/>
    /// [0,start]:                  -> has time-tag index with no time.<br/>
    /// [0,start]#karaoke           -> has time-tag index with romanised syllable.<br/>
    /// [0,start]#^karaoke          -> has time-tag index with romanised syllable, and it's the first one.<br/>
    /// [0,start]:1000#karaoke      -> has time-tag index with time and romanised syllable.<br/>
    /// </example>
    /// <param name="str">Time tag string format</param>
    /// <returns><see cref="TimeTag"/>Time tag object</returns>
    public static TimeTag ParseTimeTag(string? str)
    {
        string regex = generateRegex(time_index_regex, new[]
        {
            getNumberPropertyRegex(':', "time"),
            getStringPropertyRegex('#', "text"),
        });

        return getMatchByStatement(str, regex, result =>
        {
            if (result == null)
                return new TimeTag(new TextIndex());

            int index = result.GetGroupValue<int>("index");
            var state = result.GetGroupValue<string>("state") == "start" ? TextIndex.IndexState.Start : TextIndex.IndexState.End;
            int? time = result.GetGroupValue<int?>("time");
            string? text = result.GetGroupValue<string?>("text");
            bool? firstSyllable = text?.StartsWith('^');

            return new TimeTag(new TextIndex(index, state), time)
            {
                FirstSyllable = firstSyllable ?? default,
                RomanisedSyllable = text?.Replace("^", ""),
            };
        });
    }

    /// <summary>
    /// Process test case text index string format into <see cref="TextIndex"/>
    /// </summary>
    /// <example>
    /// [0,start]   -> has time-tag index with time.<br/>
    /// [0,end]     -> has time-tag index with time.<br/>
    /// </example>
    /// <param name="str">Text tag string format</param>
    /// <returns><see cref="TimeTag"/>Text tag object</returns>
    public static TextIndex ParseTextIndex(string? str)
    {
        string regex = generateRegex(time_index_regex, Array.Empty<string>());

        return getMatchByStatement(str, regex, result =>
        {
            if (result == null)
                return new TextIndex();

            int index = result.GetGroupValue<int>("index");
            var state = result.GetGroupValue<string>("state") == "start" ? TextIndex.IndexState.Start : TextIndex.IndexState.End;

            return new TextIndex(index, state);
        });
    }

    /// <summary>
    /// Process test case lyric string format into <see cref="Lyric"/>
    /// </summary>
    /// <example>
    /// [1000,3000]:karaoke     -> has time-range and lyric.<br/>
    /// [1000,3000]:            -> has time-range.<br/>
    /// [1000,3000]             -> has time-range.<br/>
    /// </example>
    /// <param name="str">Lyric string format</param>
    /// <param name="id">Id if needed</param>
    /// <returns><see cref="Lyric"/>Lyric object</returns>
    public static Lyric ParseLyric(string str, int? id = null)
    {
        string regex = generateRegex(time_range_str, new[]
        {
            getStringPropertyRegex(':', "lyric"),
        });

        return getMatchByStatement(str, regex, result =>
        {
            if (result == null)
                return new Lyric();

            double startTime = result.GetGroupValue<double>("startTime");
            double endTime = result.GetGroupValue<double>("endTime");
            string text = result.GetGroupValue<string>("lyric");

            return new Lyric
            {
                Text = text,
                TimeTags = new[]
                {
                    new TimeTag(new TextIndex(0), startTime),
                    new TimeTag(new TextIndex(text.Length - 1, TextIndex.IndexState.End), endTime),
                },
            }.ChangeId(id != null ? TestCaseElementIdHelper.CreateElementIdByNumber(id.Value) : ElementId.Empty);
        });
    }

    /// <summary>
    /// Process test case lyric string format into <see cref="Lyric"/>
    /// </summary>
    /// <example>
    /// "[00:01.00]か[00:02.00]ら[00:03.00]お[00:04.00]け[00:05.00]"
    /// </example>
    /// <param name="str">Lyric string format</param>
    /// <returns><see cref="Lyric"/>Lyric object</returns>
    public static Lyric ParseLyricWithTimeTag(string? str)
    {
        if (string.IsNullOrEmpty(str))
            return new Lyric();

        // Create karaoke note decoder
        var decoder = new KarDecoder();
        return decoder.Decode(str).First();
    }

    /// <summary>
    /// Process test case singer string format into <see cref="Singer"/>
    /// </summary>
    /// <example>
    /// [0]name:singer001       -> singer with id and name.<br/>
    /// [0]romanisation:singer001     -> singer with id and romanisation.<br/>
    /// [0]eg:singer001         -> singer with id and english name.<br/>
    /// </example>
    /// <param name="str">Singer string format</param>
    /// <returns><see cref="Singer"/>sSinger object</returns>
    public static Singer ParseSinger(string? str)
    {
        string regex = generateRegex(id_str, Array.Empty<string>());

        return getMatchByStatement(str, regex, result =>
        {
            if (result == null)
                return new Singer().ChangeId(ElementId.Empty);

            // todo : implementation
            int id = result.GetGroupValue<int>("id");

            return new Singer().ChangeId(id);
        });
    }

    public static RubyTag[] ParseRubyTags(IEnumerable<string?> strings)
        => strings.Select(ParseRubyTag).ToArray();

    public static TimeTag[] ParseTimeTags(IEnumerable<string?> strings)
        => strings.Select(ParseTimeTag).ToArray();

    public static Lyric[] ParseLyrics(IEnumerable<string> strings)
        => strings.Select((str, index) => ParseLyric(str, index)).ToArray();

    public static Singer[] ParseSingers(IEnumerable<string?> strings)
        => strings.Select(ParseSinger).ToArray();
}
