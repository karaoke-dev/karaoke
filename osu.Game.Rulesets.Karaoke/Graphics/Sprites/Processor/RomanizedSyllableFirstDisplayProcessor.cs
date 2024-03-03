// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites.Processor;

public class RomanizedSyllableFirstDisplayProcessor : BaseDisplayProcessor
{
    public RomanizedSyllableFirstDisplayProcessor(Lyric lyric, LyricDisplayProperty displayProperty)
        : base(lyric, displayProperty)
    {
        // Note: some of the properties are not implemented yet because we are not sure people actually use it.
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
            // Text only display at bottom.
            UpdateBottomText();
        });
        property.TimeTagsBindable.BindCollectionChanged((_, _) =>
        {
            // Ruby change might affect the center text, which will affect all property.
            UpdateAll();
        });
        property.TimeTagsRomajiVersion.BindValueChanged(_ =>
        {
            // Ruby change might affect the center text, which will affect all property.
            UpdateAll();
        });
        property.TimeTagsTimingVersion.BindValueChanged(_ =>
        {
            UpdateTimeTags();
        });
    }

    protected override IEnumerable<PositionText> CalculateTopTexts(Lyric lyric)
    {
        // todo: implementation needed.
        yield break;
    }

    protected override string CalculateCenterText(Lyric lyric) =>
        string.Join("", lyric.TimeTags.Select((x, i) =>
        {
            bool hasEmptySpace = i != 0 && x.FirstSyllable;
            return hasEmptySpace ? " " + x.RomanizedSyllable : x.RomanizedSyllable;
        }));

    protected override IEnumerable<PositionText> CalculateBottomTexts(Lyric lyric)
    {
        // todo: implementation needed.
        yield break;
    }

    protected override IReadOnlyDictionary<double, TextIndex> CalculateTimeTags(Lyric lyric)
    {
        // todo: implementation needed.
        return new Dictionary<double, TextIndex>();
    }
}
