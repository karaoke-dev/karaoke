// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Beatmaps;

namespace osu.Game.Rulesets.Karaoke.Objects.Workings;

public class LyricWorkingPropertyValidator : HitObjectWorkingPropertyValidator<Lyric, LyricWorkingProperty>
{
    public LyricWorkingPropertyValidator(Lyric hitObject)
        : base(hitObject)
    {
    }

    protected override bool HasDataProperty(LyricWorkingProperty flags) =>
        flags switch
        {
            LyricWorkingProperty.StartTime => false,
            LyricWorkingProperty.Duration => false,
            LyricWorkingProperty.Timing => false,
            LyricWorkingProperty.Singers => true,
            LyricWorkingProperty.Page => false,
            LyricWorkingProperty.ReferenceLyric => true,
            LyricWorkingProperty.EffectApplier => false,
            _ => throw new ArgumentOutOfRangeException(nameof(flags), flags, null),
        };

    protected override bool IsWorkingPropertySynced(Lyric hitObject, LyricWorkingProperty flags) =>
        flags switch
        {
            LyricWorkingProperty.StartTime => true,
            LyricWorkingProperty.Duration => true,
            LyricWorkingProperty.Timing => true,
            LyricWorkingProperty.Singers => isWorkingSingerSynced(hitObject),
            LyricWorkingProperty.Page => true,
            LyricWorkingProperty.ReferenceLyric => isReferenceLyricSynced(hitObject),
            LyricWorkingProperty.EffectApplier => true,
            _ => throw new ArgumentOutOfRangeException(nameof(flags), flags, null),
        };

    private bool isWorkingSingerSynced(Lyric lyric)
    {
        var lyricSingerIds = lyric.SingerIds.OrderBy(x => x).Distinct();
        var workingSingerIds = lyric.Singers.ToArray().Select(x =>
        {
            var ids = new List<ElementId> { x.Key.ID };
            ids.AddRange(x.Value.Select(singer => singer.ID));
            return ids;
        }).SelectMany(x => x).OrderBy(x => x).Distinct();

        return lyricSingerIds.SequenceEqual(workingSingerIds);
    }

    private bool isReferenceLyricSynced(Lyric lyric)
    {
        return lyric.ReferenceLyric?.ID == lyric.ReferenceLyricId;
    }
}
