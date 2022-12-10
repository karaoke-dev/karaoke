// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings
{
    public abstract partial class LyricEditorSettings : EditorSettings
    {
        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider)
        {
            // change the background colour to the lighter one.
            this.ChildrenOfType<Box>().First().Colour = colourProvider.Background3(state.Mode);
        }
    }
}
