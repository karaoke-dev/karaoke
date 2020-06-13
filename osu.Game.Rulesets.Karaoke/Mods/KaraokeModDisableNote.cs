// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Resources.Fonts;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Mods
{
    public class KaraokeModDisableNote : Mod, IApplicableToHitObject, IApplicableToMicrophone
    {
        public override string Name => "Disable note";
        public override string Acronym => "DN";
        public override double ScoreMultiplier => 0;
        public override IconUsage? Icon => KaraokeIcon.ModDisableNote;
        public override ModType Type => ModType.Fun;

        public bool MicrophoneEnabled => false;

        public void ApplyToHitObject(HitObject hitObject)
        {
            if (hitObject is Note note)
            {
                // Disable all the note
                note.Display = false;
            }
        }
    }
}
