// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor
{
    public class LyricRearrangeableListContainer : OsuRearrangeableListContainer<LyricLine>
    {
        protected override OsuRearrangeableListItem<LyricLine> CreateOsuDrawable(LyricLine item)
            => new LyricRearrangeableListItem(item);

        public class LyricRearrangeableListItem : OsuRearrangeableListItem<LyricLine>
        {
            public LyricRearrangeableListItem(LyricLine item)
                    : base(item)
            {
                
            }

            protected override Drawable CreateContent()
            {
                return new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Children = new Drawable[]
                    {
                        new LyricControl(Model)
                        {
                            RelativeSizeAxes = Axes.X,
                        }
                    }
                };
            }
        }
    }
}
