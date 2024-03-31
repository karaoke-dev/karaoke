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
        var startTimeTag = lyric.TimeTags.FirstOrDefault();
        if (startTimeTag == null)
            yield break;

        string collectedRomanisedSyllable = string.Empty;

        // split the text by first romanisation syllable.
        foreach (var timeTag in lyric.TimeTags)
        {
            // collecting the romanised syllable.
            collectedRomanisedSyllable += timeTag.RomanisedSyllable;

            if (lyric.TimeTags.Last() == timeTag)
            {
                // should return the collected romanised syllable if is the last one.
                yield return toPositionText(startTimeTag.Index, timeTag.Index, collectedRomanisedSyllable);
            }
            else if (lyric.TimeTags.GetNext(timeTag).FirstSyllable)
            {
                // should return the collected romanised syllable before timeTag with first syllable.
                yield return toPositionText(startTimeTag.Index, timeTag.Index, collectedRomanisedSyllable);

                startTimeTag = lyric.TimeTags.GetNext(timeTag);
                collectedRomanisedSyllable = string.Empty;
            }
        }

        yield break;

        static PositionText toPositionText(TextIndex startIndex, TextIndex endIndex, string text)
        {
            int startCharIndex = TextIndexUtils.ToCharIndex(startIndex);
            int endCharIndex = TextIndexUtils.ToCharIndex(endIndex);

            return new PositionText(text, startCharIndex, endCharIndex);
        }
    }

    protected override IReadOnlyDictionary<double, TextIndex> CalculateTimeTags(Lyric lyric)
    {
        return TimeTagsUtils.ToTimeBasedDictionary(lyric.TimeTags);
    }
}
