// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Checker.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Infos.FixedInfo
{
    public class InvalidInfo : SpriteIcon, IHasContextMenu, IHasCustomTooltip
    {
        // todo : might able to have auto-fix option by right-click
        public MenuItem[] ContextMenuItems => null;

        public object TooltipContent => report;

        private readonly Lyric lyric;

        private LyricCheckReport report;

        public InvalidInfo(Lyric lyric)
        {
            this.lyric = lyric;

            Size = new Vector2(12);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, LyricCheckerManager lyricCheckerManager)
        {
            lyricCheckerManager.BindableReports.BindCollectionChanged((i, args) =>
            {
                var dict = args.NewItems.Cast<Dictionary<Lyric, LyricCheckReport>>().FirstOrDefault();
                if (!dict.ContainsKey(lyric))
                    return;

                report = dict[lyric];

                switch (report.IsValid)
                {
                    case true:
                        Icon = FontAwesome.Solid.CheckCircle;
                        Colour = colours.Green;
                        break;

                    case false:
                        Icon = FontAwesome.Solid.TimesCircle;
                        Colour = colours.Green;
                        break;
                    default:
                        throw new IndexOutOfRangeException(nameof(report.IsValid));
                }

            }, true);
        }

        public ITooltip GetCustomTooltip()
            => new InvalidLyricToolTip();
    }
}
