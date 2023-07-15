// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Beatmaps.Stages.Classic;

public class ClassicLyricLayoutCategoryGenerator : BeatmapPropertyGenerator<ClassicLyricLayoutCategory, ClassicLyricLayoutCategoryGeneratorConfig>
{
    public ClassicLyricLayoutCategoryGenerator(ClassicLyricLayoutCategoryGeneratorConfig config)
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

    protected override ClassicLyricLayoutCategory GenerateFromItem(KaraokeBeatmap item)
    {
        int rowAmount = Config.LyricRowAmount.Value;
        bool applyMappingToTheLyric = Config.ApplyMappingToTheLyric.Value;

        var lyrics = item.HitObjects.OfType<Lyric>().ToArray();
        var layoutCategory = new ClassicLyricLayoutCategory();

        // create the element first.
        var layouts = mappingLayoutsToLyric(layoutCategory, rowAmount).ToArray();

        if (!applyMappingToTheLyric)
            return layoutCategory;

        // then, mapping to the lyric.
        for (int i = 0; i < lyrics.Length; i++)
        {
            var lyric = lyrics.ElementAt(i);
            var layout = layouts.ElementAt(i % rowAmount);

            layoutCategory.AddToMapping(layout, lyric);
        }

        return layoutCategory;
    }

    private IEnumerable<ClassicLyricLayout> mappingLayoutsToLyric(ClassicLyricLayoutCategory category, int amount)
    {
        switch (amount)
        {
            case 4:
                yield return addElementWithLine(category, 3, ClassicLyricLayoutAlignment.Left);
                yield return addElementWithLine(category, 2, ClassicLyricLayoutAlignment.Right);
                yield return addElementWithLine(category, 1, ClassicLyricLayoutAlignment.Left);
                yield return addElementWithLine(category, 0, ClassicLyricLayoutAlignment.Right);

                yield break;

            case 3:
                yield return addElementWithLine(category, 2, ClassicLyricLayoutAlignment.Left);
                yield return addElementWithLine(category, 1, ClassicLyricLayoutAlignment.Center);
                yield return addElementWithLine(category, 0, ClassicLyricLayoutAlignment.Right);

                yield break;

            case 2:
                yield return addElementWithLine(category, 1, ClassicLyricLayoutAlignment.Left);
                yield return addElementWithLine(category, 0, ClassicLyricLayoutAlignment.Right);

                yield break;

            default:
                throw new InvalidOperationException();
        }
    }

    private ClassicLyricLayout addElementWithLine(ClassicLyricLayoutCategory category, int line, ClassicLyricLayoutAlignment alignment)
    {
        float horizontalMargin = Config.HorizontalMargin.Value;

        return category.AddElement(x =>
        {
            x.Name = $"{generateName(alignment)} {x.ID}";
            x.Alignment = alignment;
            x.HorizontalMargin = horizontalMargin;
            x.Line = line;
        });

        static string generateName(ClassicLyricLayoutAlignment alignment) =>
            alignment switch
            {
                ClassicLyricLayoutAlignment.Left => "Left",
                ClassicLyricLayoutAlignment.Center => "Center",
                ClassicLyricLayoutAlignment.Right => "Right",
                _ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null),
            };
    }
}
