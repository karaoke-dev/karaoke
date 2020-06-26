﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.UI;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeEditPlayfield : KaraokePlayfield
    {
        public KaraokeEditPlayfield()
        {
            LyricPlayfield.Anchor = LyricPlayfield.Origin = Anchor.BottomCentre;
            LyricPlayfield.Margin = new MarginPadding { Top = 150, Bottom = -100 };
            LyricPlayfield.Scale = new Vector2(0.7f);
        }
    }
}
