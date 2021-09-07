// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Graphics.Cursor
{
    public class LyricTooltip : BackgroundToolTip<Lyric>
    {
        public override void SetContent(Lyric lyric)
        {
            Child = new PreviewLyricSpriteText(lyric)
            {
                Margin = new MarginPadding(10),
                Font = new FontUsage(size: 32),
                RubyFont = new FontUsage(size: 12),
                RomajiFont = new FontUsage(size: 12)
            };
        }
    }
}
