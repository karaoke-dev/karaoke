// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Utils
{
    public static class ZoomableScrollContainerUtils
    {
        public static float GetZoomLevelForVisibleMilliseconds(EditorClock editorClock, double milliseconds)
            => Math.Max(1, (float)(editorClock.TrackLength / milliseconds));
    }
}
