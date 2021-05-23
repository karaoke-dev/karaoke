// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends
{
    public abstract class EditExtend : CompositeDrawable
    {
        public abstract ExtendDirection Direction { get; }

        public abstract float ExtendWidth { get; }

        protected EditExtend()
        {
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(new CornerBackground
            {
                Depth = float.MaxValue,
                RelativeSizeAxes = Axes.Both,
                Alpha = 0.5f,
                Colour = Color4.Black
            });
        }
    }
}
