// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Utils;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites.Processor;

public class LyricFirstDisplayProcessor : BaseDisplayProcessor
{
    public LyricFirstDisplayProcessor(Lyric lyric, LyricDisplayProperty displayProperty)
        : base(lyric, displayProperty)
    {
    }

    protected override void ProcessBindableChange(AvailableProperty property)
    {
        property.RubyTagsBindable.BindCollectionChanged((_, _) =>
        {
            // Ruby only display at top.
            UpdateTopText();
        });
        property.RubyTagsVersion.BindValueChanged(_ =>
        {
            // Ruby only display at top.
            UpdateTopText();
        });
        property.TextBindable.BindValueChanged(_ =>
        {
            UpdateAll();
        });
        property.TimeTagsBindable.BindCollectionChanged((_, _) =>
        {
            // If create/remove the time-tag, romanised syllable might be affected.
            UpdateBottomText();
            UpdateTimeTags();
        });
        property.TimeTagsRomanisationVersion.BindValueChanged(_ =>
        {
            UpdateBottomText();
        });
        property.TimeTagsTimingVersion.BindValueChanged(_ =>
        {
            UpdateTimeTags();
        });
    }

    protected override IEnumerable<PositionText> CalculateTopTexts(Lyric lyric)
    {
        return lyric.RubyTags.Select(toPositionText);

        static PositionText toPositionText(RubyTag rubyTag)
            => new(rubyTag.Text, rubyTag.StartIndex, rubyTag.EndIndex);
    }

    protected override string CalculateCenterText(Lyric lyric)
        => lyric.Text;

    protected override IEnumerable<PositionText> CalculateBottomTexts(Lyric lyric)
    {
        // should be able to combine the position text.
        var groupByIndex = lyric.TimeTags.GroupBy(x => x.Index).ToDictionary(k => k.Key, v => v.ToArray());

        foreach (var (currentIndex, tags) in groupByIndex)
        {
            var nextIndex = groupByIndex.Keys.GetNext(currentIndex);
            if (nextIndex == default)
                yield break;

            // note: need to change the [0,start], [1,start] to [0,start], [0,end]
            int startCharIndex = TextIndexUtils.ToCharIndex(currentIndex);
            int endCharIndex = TextIndexUtils.ToCharIndex(TextIndexUtils.GetPreviousIndex(nextIndex));

            yield return toPositionText(startCharIndex, endCharIndex, tags);
        }

        yield break;

        static PositionText toPositionText(int startCharIndex, int endCharIndex, TimeTag[] timeTags)
        {
            string text = string.Join("", timeTags.Select(x => x.RomanisedSyllable));
            return new PositionText(text, startCharIndex, endCharIndex);
        }
    }

    protected override IReadOnlyDictionary<double, TextIndex> CalculateTimeTags(Lyric lyric)
    {
        return TimeTagsUtils.ToTimeBasedDictionary(lyric.TimeTags);
    }
}
