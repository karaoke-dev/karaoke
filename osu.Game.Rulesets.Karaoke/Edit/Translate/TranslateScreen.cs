// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate
{
    public class TranslateScreen : KaraokeEditorScreen
    {
        [Cached]
        protected readonly OverlayColourProvider ColourProvider;

        [Cached]
        protected readonly TranslateManager TranslateManager;

        public TranslateScreen()
            : base(KaraokeEditorScreenMode.Translate)
        {
            ColourProvider = new OverlayColourProvider(OverlayColourScheme.Green);
            AddInternal(TranslateManager = new TranslateManager());
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, DialogOverlay dialogOverlay, LanguageSelectionDialog languageSelectionDialog)
        {
            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding(50),
                Child = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    CornerRadius = 10,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = colours.GreySeafoamDark,
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
                            }
                        },
                    }
                }
            });

            // ask only once if contains no language after switch to translate editor.
            bool alreadyAsked;
            TranslateManager.Languages.BindCollectionChanged((_, _) =>
            {
                alreadyAsked = true;

                if (TranslateManager.Languages.Count == 0 && !alreadyAsked)
                {
                    dialogOverlay.Push(new CreateNewLanguagePopupDialog(isOk =>
                    {
                        if (isOk)
                        {
                            languageSelectionDialog.Show();
                        }
                    }));
                }
            }, true);
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
