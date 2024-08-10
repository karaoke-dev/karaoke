// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Objects.Workings;

public class LyricStageWorkingPropertyValidator : HitObjectWorkingPropertyValidator<Lyric, LyricStageWorkingProperty>
{
    public LyricStageWorkingPropertyValidator(Lyric hitObject)
        : base(hitObject)
    {
    }

    protected override bool HasDataProperty(LyricStageWorkingProperty flags) => false;

    protected override bool IsWorkingPropertySynced(Lyric hitObject, LyricStageWorkingProperty flags) => true;
}
