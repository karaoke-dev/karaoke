// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers.Components
{
    public class CreateNewLyricPlacementColumn : LyricPlacementColumn
    {
        public CreateNewLyricPlacementColumn()
            : base(new Singer(-1) { Name = "Press to create new singer" })
        {
        }

        protected override float SingerInfoSize => 178;

        protected override Drawable CreateSingerInfo(Singer singer)
        {
            return new Container
            {
                Child = new IconButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Icon = FontAwesome.Solid.PlusCircle,
                    Size = new Vector2(32),
                    TooltipText = "Click to add new singer",
                    Action = () => OnClick?.Invoke(),
                }
            };
        }

        protected override Drawable CreateTimeLinePart(KaraokeHitObjectComposer composer) => new Container();

        public Action OnClick;
    }
}
