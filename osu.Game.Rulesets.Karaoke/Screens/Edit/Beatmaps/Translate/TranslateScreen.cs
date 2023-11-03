﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Translate;

public partial class TranslateScreen : BeatmapEditorScreen
{
    [Cached(typeof(IBeatmapLanguagesChangeHandler))]
    private readonly BeatmapLanguagesChangeHandler beatmapLanguagesChangeHandler;

    [Cached(typeof(ILyricTranslateChangeHandler))]
    private readonly LyricTranslateChangeHandler lyricTranslateChangeHandler;

    public TranslateScreen()
        : base(KaraokeBeatmapEditorScreenMode.Translate)
    {
        AddInternal(beatmapLanguagesChangeHandler = new BeatmapLanguagesChangeHandler());
        AddInternal(lyricTranslateChangeHandler = new LyricTranslateChangeHandler());
    }

    [BackgroundDependencyLoader]
    private void load(OverlayColourProvider colourProvider)
    {
        Children = new Drawable[]
        {
            new Box
            {
                Colour = colourProvider.Background3,
                RelativeSizeAxes = Axes.Both,
            },
            new SectionsContainer<Container>
            {
                FixedHeader = new TranslateScreenHeader(),
                RelativeSizeAxes = Axes.Both,
                Children = new Container[]
                {
                    new TranslateEditSection
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                    },
                },
            },
        };
    }

    internal partial class TranslateScreenHeader : OverlayHeader
    {
        protected override OverlayTitle CreateTitle() => new TranslateScreenTitle();

        private partial class TranslateScreenTitle : OverlayTitle
        {
            public TranslateScreenTitle()
            {
                Title = "translate";
                Description = "create translation of your beatmap";
                IconTexture = "Icons/Hexacons/social";
            }
        }
    }
}
