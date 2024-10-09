// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Screens.Edit.Setup;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup;

public partial class KaraokeNoteSection : SetupSection
{
    public override LocalisableString Title => "Note";

    private FormCheckBox scorable = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        Children = new Drawable[]
        {
            scorable = new FormCheckBox
            {
                Caption = "Scorable",
                HintText = "Will not show score playfield if the option is unchecked.",
                Current = { Value = true },
            },
        };

        scorable.Current.BindValueChanged(_ => updateValues());
    }

    private void updateValues()
    {
        if (Beatmap.PlayableBeatmap is not KaraokeBeatmap karaokeBeatmap)
            throw new InvalidOperationException();

        karaokeBeatmap.Scorable = scorable.Current.Value;
    }
}
