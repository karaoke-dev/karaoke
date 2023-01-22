// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator;

public class GeneratorNotSupportedException : NotSupportedException
{
    public GeneratorNotSupportedException()
        : base("Cannot generate the property due to have some invalid fields, please make sure that run the CanGenerate() first.")
    {
    }
}
