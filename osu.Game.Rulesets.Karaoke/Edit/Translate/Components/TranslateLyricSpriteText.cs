// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Graphics.Cursor;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Translate.Components
{
    public class TranslateLyricSpriteText : DrawableLyricSpriteText, IHasCustomTooltip<Lyric>
    {
        public TranslateLyricSpriteText(Lyric hitObject)
            : base(hitObject)
        {
        }

        public ITooltip<Lyric> GetCustomTooltip() => new LyricTooltip();

        public Lyric TooltipContent => HitObject;
    }
}
