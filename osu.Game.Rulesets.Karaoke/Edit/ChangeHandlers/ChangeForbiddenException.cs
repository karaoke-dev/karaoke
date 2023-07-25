// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;

public class ChangeForbiddenException : InvalidOperationException
{
    public ChangeForbiddenException(string message)
        : base(message)
    {
    }
}
