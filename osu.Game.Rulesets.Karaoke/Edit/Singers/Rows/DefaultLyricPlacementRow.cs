// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Rows
{
    public class DefaultLyricPlacementColumn : LyricPlacementColumn
    {
        public DefaultLyricPlacementColumn()
            : base(new Singer(0) { Name = "Default" })
        {
        }

        protected override float SingerInfoSize => 200;

        // todo : might display song info?
        protected override Drawable CreateSingerInfo(Singer singer) => new Container();
    }
}
