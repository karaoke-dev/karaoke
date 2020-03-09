// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Screens.SaitenAdjustment.UI
{
    public class DrawableSaitenAdjustmentRuleset : DrawableKaraokeRuleset
    {
        public DrawableSaitenAdjustmentRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods)
            : base(ruleset, beatmap, mods)
        {
        }

        protected override Playfield CreatePlayfield() => new NotePlayfield(9);

        public override DrawableHitObject<KaraokeHitObject> CreateDrawableRepresentation(KaraokeHitObject h)
        {
            // Only get drawable note here
            var drawableHitObject = base.CreateDrawableRepresentation(h);
            if (drawableHitObject is DrawableNote)
                return drawableHitObject;

            return null;
        }
    }
}
