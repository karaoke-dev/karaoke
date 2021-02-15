// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Infos.FixedInfo
{
    public class InvalidInfo : SpriteIcon, IHasContextMenu, IHasCustomTooltip
    {
        [Resolved]
        private LyricInvalidChecker lyricInvalidChecker { get; set; }

        // todo : might able to have auto-fix option by right-click
        public MenuItem[] ContextMenuItems => null;

        public object TooltipContent => lyric;

        private readonly Lyric lyric;

        public InvalidInfo(Lyric lyric)
        {
            this.lyric = lyric;

            Size = new Vector2(12);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            // todo : should be able to get bindable state by 
            Icon = FontAwesome.Solid.CheckCircle;
            Colour = colours.Green;
        }

        public ITooltip GetCustomTooltip()
            => new InvalidLyricToolTip();
    }
}
