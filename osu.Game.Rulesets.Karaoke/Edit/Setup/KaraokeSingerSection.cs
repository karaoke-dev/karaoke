// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Setup.Components;
using osu.Game.Screens.Edit.Setup;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup;

public partial class KaraokeSingerSection : SetupSection
{
    public override LocalisableString Title => "Singers";

    [Cached(typeof(IKaraokeBeatmapResourcesProvider))]
    private KaraokeBeatmapResourcesProvider karaokeBeatmapResourcesProvider = new();

    private readonly IBeatmapSingersChangeHandler changeHandler = new BeatmapSingersChangeHandler();

    private LabelledSingerList singerList = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        AddInternal(karaokeBeatmapResourcesProvider);

        Children = new Drawable[]
        {
            singerList = new LabelledSingerList
            {
                Label = "Singer list",
                Description = "All the singers in beatmap.",
                FixedLabelWidth = LABEL_WIDTH,
            },
        };

        if (Beatmap.BeatmapSkin != null)
            singerList.Singers.BindTo(changeHandler.Singers);
    }
}
