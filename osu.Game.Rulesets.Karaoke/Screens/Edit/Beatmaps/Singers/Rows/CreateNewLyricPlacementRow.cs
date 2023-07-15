// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Singers.Rows;

public partial class CreateNewLyricPlacementColumn : LyricPlacementColumn
{
    [Resolved]
    private IBeatmapSingersChangeHandler beatmapSingersChangeHandler { get; set; } = null!;

    public CreateNewLyricPlacementColumn()
        : base(new Singer { Name = "Press to create new singer" })
    {
    }

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
                Action = () =>
                {
                    beatmapSingersChangeHandler.Add();
                },
            },
        };
    }

    protected override Drawable CreateTimeLinePart(Singer singer) => Empty();
}
