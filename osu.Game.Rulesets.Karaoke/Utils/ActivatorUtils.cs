// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Utils;

public static class ActivatorUtils
{
    public static TObject CreateInstance<TObject>(params object?[]? args)
    {
        var algorithm = (TObject?)Activator.CreateInstance(typeof(TObject), args);
        if (algorithm == null)
            throw new InvalidOperationException();

        return algorithm;
    }
}
