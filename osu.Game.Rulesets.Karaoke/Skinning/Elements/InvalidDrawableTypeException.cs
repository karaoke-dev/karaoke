// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;

namespace osu.Game.Rulesets.Karaoke.Skinning.Elements
{
    public class InvalidDrawableTypeException : Exception
    {
        public InvalidDrawableTypeException(string message)
            : base(@$"Drawable type does not supported ({message})")
        {
        }
    }
}
