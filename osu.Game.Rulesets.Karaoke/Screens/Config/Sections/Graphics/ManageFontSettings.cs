// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Graphics
{
    public class ManageFontSettings : KaraokeSettingsSubsection
    {
        protected override string Header => "Font Management";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                 new SettingsButton
                 {
                     Text = "Open import text folder",
                     TooltipText = "After open the folder, you can drag the font file to the folder you wants to import",
                     Action = () => {
                         // todo : open the folder.
                         // todo : or should change the button to call file selector to import the file?
                     }
                 },
                 new SettingsButton
                 {
                     Text = "Import file",
                     TooltipText = "If some font is placed into folder but not import, press here to try again."
                 }
            };
        }
    }
}
