// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components
{
    public class DefaultLyricPlacementColumn : LyricPlacementColumn
    {
        public DefaultLyricPlacementColumn()
            : base(new Singer(-1) { Name = "Default" })
        {
        }
    }
}
