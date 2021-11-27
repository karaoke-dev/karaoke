// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    [Cached]
    public class LyricImporter : ScreenWithBeatmapBackground
    {
        private readonly LyricImporterWaveContainer waves;

        [Cached]
        protected LyricImporterSubScreenStack ScreenStack { get; private set; }

        [Cached]
        private readonly ImportLyricManager importManager;

        public LyricImporter()
        {
            var backgroundColour = Color4Extensions.FromHex(@"3e3a44");

            InternalChild = waves = new LyricImporterWaveContainer
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = backgroundColour,
                    },
                    new KaraokeEditInputManager(new KaraokeRuleset().RulesetInfo)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding { Top = Header.HEIGHT },
                        Child = ScreenStack = new LyricImporterSubScreenStack { RelativeSizeAxes = Axes.Both }
                    },
                    new Header(ScreenStack),
                }
            };

            ScreenStack.Push(LyricImporterStep.ImportLyric);

            AddInternal(importManager = new ImportLyricManager());
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            waves.Show();
        }

        private class LyricImporterWaveContainer : WaveContainer
        {
            protected override bool StartHidden => true;

            public LyricImporterWaveContainer()
            {
                FirstWaveColour = Color4Extensions.FromHex(@"654d8c");
                SecondWaveColour = Color4Extensions.FromHex(@"554075");
                ThirdWaveColour = Color4Extensions.FromHex(@"44325e");
                FourthWaveColour = Color4Extensions.FromHex(@"392850");
            }
        }
    }
}
