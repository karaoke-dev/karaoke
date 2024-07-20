// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.Setup.Components;
using osu.Game.Screens.Edit.Setup;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup;

public partial class KaraokeSingerSection : SetupSection
{
    public override LocalisableString Title => "Singers";

    private LabelledSingerList singerList = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        Children = new Drawable[]
        {
            singerList = new LabelledSingerList
            {
                Label = "Singer list",
                Description = "All the singers in beatmap.",
                FixedLabelWidth = LABEL_WIDTH,
                SingerNamePrefix = "#",
            },
        };
    }
}
