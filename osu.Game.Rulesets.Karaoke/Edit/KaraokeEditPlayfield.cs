// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.UI;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI.Scrolling;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;

namespace osu.Game.Rulesets.Karaoke.Edit
{
    public class KaraokeEditPlayfield : ScrollingPlayfield
    {
        public LyricMakerPlayfield LyricMakerPlayfield { get; }

        public KaraokeEditPlayfield()
        {
            AddInternal(LyricMakerPlayfield = new LyricMakerPlayfield
            {
                RelativeSizeAxes = Axes.Both,
                Depth = -1
            });

            AddNested(LyricMakerPlayfield);

            HitObjectContainer.Hide();
        }

        public override void Add(DrawableHitObject h)
        {
            switch (h)
            {
                case DrawableLyricLine _:
                    LyricMakerPlayfield.Add(h);
                    break;

                default:
                    base.Add(h);
                    break;
            }
            
        }

        public override bool Remove(DrawableHitObject h)
        {
            switch (h)
            {
                case DrawableLyricLine _:
                    return LyricMakerPlayfield.Remove(h);

                default:
                    return base.Remove(h);
            }
        }
    }
}
