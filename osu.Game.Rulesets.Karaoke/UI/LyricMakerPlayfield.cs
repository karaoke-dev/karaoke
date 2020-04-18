// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.UI;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.UI
{
    public class LyricMakerPlayfield : Playfield
    {
        public LyricMakerPlayfield()
        {
            AddInternal(new OsuScrollContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = HitObjectContainer
            });
        }
    }
}
