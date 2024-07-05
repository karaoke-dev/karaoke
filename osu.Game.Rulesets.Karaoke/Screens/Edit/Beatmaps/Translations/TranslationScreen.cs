// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Translations;

public partial class TranslationScreen : BeatmapEditorRoundedScreen
{
    [Cached(typeof(IBeatmapLanguagesChangeHandler))]
    private readonly BeatmapLanguagesChangeHandler beatmapLanguagesChangeHandler;

    [Cached(typeof(ILyricTranslationChangeHandler))]
    private readonly LyricTranslationChangeHandler lyricTranslationChangeHandler;

    public TranslationScreen()
        : base(KaraokeBeatmapEditorScreenMode.Translation)
    {
        AddInternal(beatmapLanguagesChangeHandler = new BeatmapLanguagesChangeHandler());
        AddInternal(lyricTranslationChangeHandler = new LyricTranslationChangeHandler());
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Add(new SectionsContainer<Container>
        {
            FixedHeader = new TranslationScreenHeader(),
            RelativeSizeAxes = Axes.Both,
            Children = new Container[]
            {
                new TranslationEditSection
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                },
            },
        });
    }

    internal partial class TranslationScreenHeader : OverlayHeader
    {
        protected override OverlayTitle CreateTitle() => new TranslationScreenTitle();

        private partial class TranslationScreenTitle : OverlayTitle
        {
            public TranslationScreenTitle()
            {
                Title = "translation";
                Description = "create translation of your beatmap";
                Icon = OsuIcon.Online;
            }
        }
    }
}
