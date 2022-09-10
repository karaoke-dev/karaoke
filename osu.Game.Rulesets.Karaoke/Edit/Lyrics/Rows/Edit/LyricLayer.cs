// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Edit
{
    public class LyricLayer : BaseLayer
    {
        public LyricLayer(Lyric lyric, Drawable karaokeSpriteText)
            : base(lyric)
        {
            InternalChild = karaokeSpriteText;
        }

        public override void UpdateDisableEditState(bool editable)
        {
            this.FadeTo(editable ? 1 : 0.5f, 100);
        }
    }
}
