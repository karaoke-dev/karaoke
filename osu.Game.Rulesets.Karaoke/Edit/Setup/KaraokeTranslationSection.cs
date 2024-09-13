// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Setup.Components;
using osu.Game.Screens.Edit.Setup;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup;

public partial class KaraokeTranslationSection : SetupSection
{
    public override LocalisableString Title => "Translation";

    [Cached(typeof(IBeatmapTranslationsChangeHandler))]
    private readonly BeatmapTranslationsChangeHandler changeHandler = new();

    private LabelledLanguageList singerList = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        AddInternal(changeHandler);

        Children = new Drawable[]
        {
            singerList = new LabelledLanguageList
            {
                Label = "Translation list",
                Description = "All the lyric translation in beatmap.",
                FixedLabelWidth = LABEL_WIDTH,
            },
        };

        singerList.Languages.AddRange(changeHandler.Languages);
        singerList.Languages.BindCollectionChanged((_, args) =>
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var language in args.NewItems?.Cast<CultureInfo>() ?? Array.Empty<CultureInfo>())
                    {
                        changeHandler.Add(language);
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var language in args.OldItems?.Cast<CultureInfo>() ?? Array.Empty<CultureInfo>())
                    {
                        changeHandler.Remove(language);
                    }

                    break;
            }
        });
    }
}
