// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Badges
{
    public class LanguageInfoBadge : Badge
    {
        public LanguageInfoBadge(Lyric lyric)
            : base(lyric)
        {
            lyric.LanguageBindable.BindValueChanged(value =>
            {
                var language = value.NewValue;

                BadgeText = language == null ? "None" : language.DisplayName;
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            BadgeColour = colours.BlueDarker;
        }
    }
}
