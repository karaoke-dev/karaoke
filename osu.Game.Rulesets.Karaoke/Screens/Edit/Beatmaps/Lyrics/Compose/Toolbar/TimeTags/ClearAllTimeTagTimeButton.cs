// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Compose.Toolbar.TimeTags
{
    public partial class ClearAllTimeTagTimeButton : ActionButton
    {
        [Resolved, AllowNull]
        private ILyricTimeTagsChangeHandler lyricTimeTagsChangeHandler { get; set; }

        public ClearAllTimeTagTimeButton()
        {
            SetIcon(FontAwesome.Solid.Redo);

            Action = () =>
            {
                lyricTimeTagsChangeHandler.ClearAllTimeTagTime();
            };
        }
    }
}
