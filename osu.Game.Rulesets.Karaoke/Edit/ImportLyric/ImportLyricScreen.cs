// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric.DragFile;
using osu.Game.Screens.Edit;
using System.IO;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public class ImportLyricScreen : EditorScreen
    {
        private readonly ImportLyricWaveContainer waves;

        [Cached]
        private readonly ImportLyricSubScreenStack screenStack;

        public ImportLyricScreen(FileInfo fileInfo)
            : this()
        {
            if (!(screenStack.CurrentScreen is DragFileSubScreen dragFileSubScreen))
                throw new ScreenStack.ScreenNotInStackException($"{nameof(DragFileSubScreen)} does not in the screen.");

            dragFileSubScreen.ImportLyricFile(fileInfo);
        }

        public ImportLyricScreen()
            : base(EditorScreenMode.SongSetup)
        {
            var backgroundColour = Color4Extensions.FromHex(@"3e3a44");

            InternalChild = waves = new ImportLyricWaveContainer
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = backgroundColour,
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding { Top = Header.HEIGHT },
                        Child = screenStack = new ImportLyricSubScreenStack { RelativeSizeAxes = Axes.Both }
                    },
                    new Header(screenStack),
                }
            };

            screenStack.Push(ImportLyricStep.ImportLyric);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            waves.Show();
        }

        private class ImportLyricWaveContainer : WaveContainer
        {
            protected override bool StartHidden => true;

            public ImportLyricWaveContainer()
            {
                FirstWaveColour = Color4Extensions.FromHex(@"654d8c");
                SecondWaveColour = Color4Extensions.FromHex(@"554075");
                ThirdWaveColour = Color4Extensions.FromHex(@"44325e");
                FourthWaveColour = Color4Extensions.FromHex(@"392850");
            }
        }
    }
}
