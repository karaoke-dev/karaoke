// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Config.Sections.Graphics
{
    public class LyricFontSettings : KaraokeSettingsSubsection
    {
        protected override string Header => "Lyric font";

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {

            };
        }
    }
}
