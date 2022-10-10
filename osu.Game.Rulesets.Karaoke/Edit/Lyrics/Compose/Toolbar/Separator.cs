// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Compose.Toolbar
{
    public class Separator : CompositeDrawable
    {
        private readonly IBindable<LyricEditorMode> bindableMode = new Bindable<LyricEditorMode>();

        private readonly Box barline;

        public Separator()
        {
            Size = new Vector2(3, SpecialActionToolbar.HEIGHT);
            InternalChild = barline = new Box
            {
                RelativeSizeAxes = Axes.Both,
            };
        }

        [BackgroundDependencyLoader(true)]
        private void load(ILyricEditorState state, LyricEditorColourProvider colourProvider)
        {
            bindableMode.BindValueChanged(x =>
            {
                barline.Colour = colourProvider.Background1(state.Mode);
            }, true);
        }
    }
}
