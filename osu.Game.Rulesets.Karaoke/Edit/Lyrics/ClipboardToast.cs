// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Framework.Extensions;
using osu.Framework.Localisation;
using osu.Game.Overlays.OSD;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class ClipboardToast : Toast
    {
        public ClipboardToast(LyricEditorMode mode, ClipboardAction action)
            : base(getDescription(), getValue(action), getShortcut(mode, action))
        {
        }

        private static LocalisableString getDescription()
            => "Lyric editor";

        private static LocalisableString getValue(ClipboardAction action)
            => action.GetDescription();

        private static LocalisableString getShortcut(LyricEditorMode mode, ClipboardAction action)
            => $"Lyric has been {action.GetDescription().ToLower()} in the {action.GetDescription().ToLower()} mode.";
    }

    public enum ClipboardAction
    {
        [Description("Cut")]
        Cut,

        [Description("Copy")]
        Copy,

        [Description("Paste")]
        Paste
    }
}
