// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components
{
    public class CreateNewLyricPlacementColumn : LyricPlacementColumn
    {
        public CreateNewLyricPlacementColumn()
            : base(new Singer(-1) { Name = "Press to create new singer" })
        {
        }

        protected override float SingerInfoSize => 178;

        // todo : might display add icon
        protected override Drawable CreateSingerInfo(Singer singer) => new Container();
    }
}
