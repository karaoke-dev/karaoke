// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends
{
    public abstract class LyricEditorSettings : EditorRoundedScreenSettings
    {
        public abstract ExtendDirection Direction { get; }

        public abstract float ExtendWidth { get; }

        protected void ReloadSections()
        {
            this.ChildrenOfType<FillFlowContainer>().First().Children = CreateSections();
        }

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider)
        {
            // change the background colour to the lighter one.
            this.ChildrenOfType<Box>().First().Colour = colourProvider.Background3(state.Mode);
        }
    }
}
