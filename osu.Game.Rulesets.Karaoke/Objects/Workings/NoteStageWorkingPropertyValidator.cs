// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Objects.Workings;

public class NoteStageWorkingPropertyValidator : HitObjectWorkingPropertyValidator<Note, NoteStageWorkingProperty>
{
    public NoteStageWorkingPropertyValidator(Note hitObject)
        : base(hitObject)
    {
    }

    protected override bool HasDataProperty(NoteStageWorkingProperty flags) => false;

    protected override bool IsWorkingPropertySynced(Note hitObject, NoteStageWorkingProperty flags) => true;
}
