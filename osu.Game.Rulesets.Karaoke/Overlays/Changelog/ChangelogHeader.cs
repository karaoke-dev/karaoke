// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Overlays;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
    public class ChangelogHeader : BreadcrumbControlOverlayHeader
    {
        protected override OverlayTitle CreateTitle() => new ChangelogHeaderTitle();

        private class ChangelogHeaderTitle : OverlayTitle
        {
            public ChangelogHeaderTitle()
            {
                Title = "changelog";
                IconTexture = "Icons/changelog";
            }
        }
    }
}
