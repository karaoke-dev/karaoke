// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Tests.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Pages;

public class PageGenerator : BeatmapPropertyGenerator<Page[], PageGeneratorConfig>
{
    public PageGenerator(PageGeneratorConfig config)
        : base(config)
    {
    }

    protected override LocalisableString? GetInvalidMessageFromItem(KaraokeBeatmap item)
    {
        var lyrics = item.HitObjects.OfType<Lyric>().ToArray();
        if (lyrics.Length < 1)
            return "There's not lyric in the beatmap.";

        var timeTagChecker = new CheckLyricTimeTag();
        var invalidIssues = timeTagChecker.Run(getContext(item));
        if (invalidIssues.Any())
            return "Should not have any time-tag related issues";

        return null;
    }

    private static BeatmapVerifierContext getContext(IBeatmap beatmap)
        => new(beatmap, new TestWorkingBeatmap(beatmap));

    protected override Page[] GenerateFromItem(KaraokeBeatmap item)
    {
        if (Config.MinTime.Value < CheckBeatmapPageInfo.MIN_INTERVAL || Config.MaxTime.Value > CheckBeatmapPageInfo.MAX_INTERVAL)
            throw new InvalidOperationException("Interval time should be validate.");

        var existPages = Config.ClearExistPages.Value ? Array.Empty<Page>() : item.PageInfo.SortedPages.ToArray();
        var lyricTimingInfos = item.HitObjects.OfType<Lyric>().Select(x => new LyricTimingInfo
        {
            StartTime = x.LyricStartTime,
            EndTime = x.LyricEndTime,
        }).OrderBy(x => x).ToList();

        if (lyricTimingInfos.Count == 0)
            return existPages;

        return calculatePageByLyrics(lyricTimingInfos, existPages).ToArray();
    }

    private IEnumerable<Page> calculatePageByLyrics(IReadOnlyList<LyricTimingInfo> lyricTimingInfos, IReadOnlyList<Page> existPages)
    {
        double currentTime;

        // create first page with it's start time.
        yield return createReturnPage(existPages.FirstOrDefault()?.Time ?? lyricTimingInfos.FirstOrDefault().StartTime);

        for (int i = 0; i < lyricTimingInfos.Count; i++)
        {
            bool lsLast = i == lyricTimingInfos.Count - 1;

            var currentLyricTimingInfo = lyricTimingInfos[i];
            LyricTimingInfo? nextLyricTimingInfo = lsLast ? null : lyricTimingInfos[i + 1];

            bool getAverageTimeWithNextLyric = nextLyricTimingInfo != null && nextLyricTimingInfo.Value.StartTime > currentLyricTimingInfo.EndTime;
            double expectedEndTime = getAverageTimeWithNextLyric
                ? (currentLyricTimingInfo.EndTime + nextLyricTimingInfo!.Value.StartTime) / 2
                : currentLyricTimingInfo.EndTime;

            while (currentTime < expectedEndTime)
            {
                if (expectedEndTime - currentTime < Config.MinTime.Value && getAverageTimeWithNextLyric)
                {
                    break;
                }

                if (expectedEndTime - currentTime > Config.MaxTime.Value)
                {
                    yield return createReturnPage(currentTime + Config.MaxTime.Value);
                }
                else
                {
                    yield return createReturnPage(expectedEndTime);
                }
            }
        }

        Page createReturnPage(double time)
        {
            currentTime = time;
            return new Page { Time = time };
        }
    }

    private struct LyricTimingInfo : IComparable<LyricTimingInfo>
    {
        public double StartTime { get; set; }

        public double EndTime { get; set; }

        public int CompareTo(LyricTimingInfo other)
        {
            return ComparableUtils.CompareByProperty(this, other,
                t => t.StartTime,
                t => t.EndTime);
        }
    }
}
