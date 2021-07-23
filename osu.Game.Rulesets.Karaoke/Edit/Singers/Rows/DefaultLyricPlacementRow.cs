// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Rows
{
    public class DefaultLyricPlacementColumn : LyricPlacementColumn
    {
        public static Singer DEFAULT_SINGER { get; } = new Singer(0) { Name = "Default" };

        public DefaultLyricPlacementColumn()
            : base(DEFAULT_SINGER)
        {
        }

        protected override float SingerInfoSize => 200;

        // todo : might display song info?
        protected override Drawable CreateSingerInfo(Singer singer) => new Container();
    }
}
