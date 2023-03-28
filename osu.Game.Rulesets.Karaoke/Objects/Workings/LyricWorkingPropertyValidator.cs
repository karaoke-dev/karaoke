// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Objects.Workings;

public class LyricWorkingPropertyValidator : HitObjectWorkingPropertyValidator<Lyric, LyricWorkingProperty>
{
    public LyricWorkingPropertyValidator(Lyric hitObject)
        : base(hitObject)
    {
    }

    protected override bool CanCheckWorkingPropertySync(Lyric hitObject, LyricWorkingProperty flags) =>
        flags switch
        {
            LyricWorkingProperty.StartTime => false,
            LyricWorkingProperty.Duration => false,
            LyricWorkingProperty.Timing => false,
            LyricWorkingProperty.Page => false, // there's no way to check working page is sync to the page info.
            LyricWorkingProperty.ReferenceLyric => true,
            _ => throw new ArgumentOutOfRangeException(nameof(flags), flags, null)
        };

    protected override bool NeedToSyncWorkingProperty(Lyric hitObject, LyricWorkingProperty flags) =>
        flags switch
        {
            LyricWorkingProperty.StartTime => false,
            LyricWorkingProperty.Duration => false,
            LyricWorkingProperty.Timing => false,
            LyricWorkingProperty.Page => false,
            LyricWorkingProperty.ReferenceLyric => hitObject.ReferenceLyric?.ID != hitObject.ReferenceLyricId,
            _ => throw new ArgumentOutOfRangeException(nameof(flags), flags, null)
        };
}
