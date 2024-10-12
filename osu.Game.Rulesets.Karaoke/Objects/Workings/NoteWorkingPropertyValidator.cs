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

    protected override bool HasDataProperty(NoteWorkingProperty flags) =>
        flags switch
        {
            NoteWorkingProperty.Page => false,
            NoteWorkingProperty.ReferenceLyric => true,
            NoteWorkingProperty.CommandGenerator => false,
            _ => throw new ArgumentOutOfRangeException(nameof(flags), flags, null),
        };

    protected override bool IsWorkingPropertySynced(Note hitObject, NoteWorkingProperty flags) =>
        flags switch
        {
            NoteWorkingProperty.Page => true,
            NoteWorkingProperty.ReferenceLyric => hitObject.ReferenceLyric?.ID == hitObject.ReferenceLyricId,
            NoteWorkingProperty.CommandGenerator => true,
            _ => throw new ArgumentOutOfRangeException(nameof(flags), flags, null),
        };
}
