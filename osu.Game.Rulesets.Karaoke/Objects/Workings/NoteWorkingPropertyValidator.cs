// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Objects.Workings;

public class NoteWorkingPropertyValidator : HitObjectWorkingPropertyValidator<Note, NoteWorkingProperty>
{
    public NoteWorkingPropertyValidator(Note hitObject)
        : base(hitObject)
    {
    }

    protected override bool CanCheckWorkingPropertySync(Note hitObject, NoteWorkingProperty flags) =>
        flags switch
        {
            NoteWorkingProperty.PreemptTime => false,
            NoteWorkingProperty.Page => false,
            NoteWorkingProperty.ReferenceLyric => true,
            NoteWorkingProperty.EffectApplier => false,
            _ => throw new ArgumentOutOfRangeException(nameof(flags), flags, null)
        };

    protected override bool NeedToSyncWorkingProperty(Note hitObject, NoteWorkingProperty flags) =>
        flags switch
        {
            NoteWorkingProperty.PreemptTime => false,
            NoteWorkingProperty.Page => false,
            NoteWorkingProperty.ReferenceLyric => hitObject.ReferenceLyric?.ID != hitObject.ReferenceLyricId,
            NoteWorkingProperty.EffectApplier => false,
            _ => throw new ArgumentOutOfRangeException(nameof(flags), flags, null)
        };
}
