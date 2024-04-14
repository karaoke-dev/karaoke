// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

public class NotImportStepScreenException : InvalidOperationException
{
    public NotImportStepScreenException()
        : base("Screen stack should only contains step screen")
    {
    }
}
