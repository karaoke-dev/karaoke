// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Classic;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Stages.Classic;

public class ClassicLyricTimingInfoGenerator : StageInfoPropertyGenerator<ClassicLyricTimingInfo, ClassicLyricTimingInfoGeneratorConfig>
{
    public ClassicLyricTimingInfoGenerator(ClassicLyricTimingInfoGeneratorConfig config)
        : base(config)
    {
    }

    protected override LocalisableString? GetInvalidMessageFromItem(KaraokeBeatmap item)
    {
        var lyrics = item.HitObjects.OfType<Lyric>().ToArray();
        if (!lyrics.Any())
            return "Should have lyric in the beatmap.";

        return null;
    }

    protected override ClassicLyricTimingInfo GenerateFromItem(KaraokeBeatmap item)
    {
        int lyricAmount = Config.LyricRowAmount.Value;

        var lyrics = item.HitObjects.OfType<Lyric>().ToArray();
        var timingInfo = new ClassicLyricTimingInfo();

        // add start timing info.
        var firstTimingPoint = timingInfo.AddTimingPoint();

        for (int i = 0; i < lyricAmount; i++)
        {
            var showLyric = lyrics.ElementAt(i);
            timingInfo.AddToMapping(firstTimingPoint, showLyric);
        }

        // should hide the current and show the next n lyric if touch the lyric end time.
        for (int i = 0; i < lyrics.Length - lyricAmount; i++)
        {
            var disappearLyric = lyrics.ElementAt(i);
            var showLyric = lyrics.ElementAt(i + lyricAmount);

            var timingPoint = timingInfo.AddTimingPoint(x => x.Time = disappearLyric.LyricEndTime);
            timingInfo.AddToMapping(timingPoint, disappearLyric);
            timingInfo.AddToMapping(timingPoint, showLyric);
        }

        // add end timing info.
        var lastTimingPoint = timingInfo.AddTimingPoint(x => x.Time = lyrics.Last().LyricEndTime);

        for (int i = lyrics.Length - lyricAmount; i < lyrics.Length; i++)
        {
            var disappearLyric = lyrics.ElementAt(i);
            timingInfo.AddToMapping(lastTimingPoint, disappearLyric);
        }

        return timingInfo;
    }
}
