// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Utils;

public class StackTraceUtils
{
    public static bool IsStackTraceContains(string text)
    {
        string? stackTrace = Environment.StackTrace;
        return stackTrace.Contains(text);
    }
}
