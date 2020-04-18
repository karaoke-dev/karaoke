// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor
{
    public class LyricEditorScreen : EditorScreenWithTimeline
    {
        protected override Drawable CreateMainContent()
        {
            return new OsuScrollContainer
            {
                RelativeSizeAxes = Axes.Both
            };
        }
    }
}
