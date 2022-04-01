// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Languages;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate
{
    public class TranslateScreen : KaraokeEditorRoundedScreen
    {
        [Cached(typeof(ILanguagesChangeHandler))]
        private readonly LanguagesChangeHandler languagesChangeHandler;

        [Cached(typeof(ILyricTranslateChangeHandler))]
        private readonly LyricTranslateChangeHandler lyricTranslateChangeHandler;

        public TranslateScreen()
            : base(KaraokeEditorScreenMode.Translate)
        {
            AddInternal(languagesChangeHandler = new LanguagesChangeHandler());
            AddInternal(lyricTranslateChangeHandler = new LyricTranslateChangeHandler());
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(new SectionsContainer<Container>
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
                }
            });
        }

        internal class TranslateScreenHeader : OverlayHeader
        {
            protected override OverlayTitle CreateTitle() => new TranslateScreenTitle();

            private class TranslateScreenTitle : OverlayTitle
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
}
