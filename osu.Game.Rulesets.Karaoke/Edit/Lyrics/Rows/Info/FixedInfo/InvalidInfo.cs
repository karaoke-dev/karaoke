// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit.Checker;
using osu.Game.Rulesets.Karaoke.Edit.Components.Cursor;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Info.FixedInfo
{
    public class InvalidInfo : SpriteIcon, IHasContextMenu, IHasCustomTooltip<Issue[]>
    {
        // todo : might able to have auto-fix option by right-click
        public MenuItem[] ContextMenuItems => null;

        private readonly Lyric lyric;

        private Issue[] report;

        public InvalidInfo(Lyric lyric)
        {
            this.lyric = lyric;

            Size = new Vector2(12);
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, LyricCheckerManager lyricCheckerManager)
        {
            lyricCheckerManager.BindableReports.BindCollectionChanged((_, args) =>
            {
                // Ignore remove case
                if (args.NewItems == null)
                    return;

                var dict = args.NewItems.ToDictionary(k => k.Key, v => v.Value);
                if (!dict.ContainsKey(lyric))
                    return;

                report = dict[lyric];

                switch (report.Length)
                {
                    case 0:
                        Icon = FontAwesome.Solid.CheckCircle;
                        Colour = colours.Green;
                        break;

                    default:
                        Icon = FontAwesome.Solid.TimesCircle;
                        Colour = colours.Red;
                        break;
                }
            }, true);
        }

        public ITooltip<Issue[]> GetCustomTooltip()
            => new InvalidLyricToolTip();

        public Issue[] TooltipContent => report;
    }
}
