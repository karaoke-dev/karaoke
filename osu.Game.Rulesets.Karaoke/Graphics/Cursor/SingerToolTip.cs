// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.Cursor
{
    public class SingerToolTip : BackgroundToolTip
    {
        public SingerToolTip()
        {
            Children = new Drawable[]
            {
                new Box
                {
                    Size = new Vector2(100),
                    Colour = Color4.Red,
                }
            };
        }

        public override bool SetContent(object content)
        {
            if(!(content is Singer singer))
                return false;

            // todo : implement
            return true;
        }
    }
}
