// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class ImportLyricSubScreenWithTopNavigation : ImportLyricSubScreen
    {
        public ImportLyricSubScreenWithTopNavigation()
        {
            Padding = new MarginPadding(20);
            // todo : add grid container with navigation
        }

        public class TopNavigation : Container
        {
            // todo : background part and right to next step button.
        }
    }
}
