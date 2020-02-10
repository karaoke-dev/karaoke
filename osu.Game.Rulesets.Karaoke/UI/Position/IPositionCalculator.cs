// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.Position
{
    public interface IPositionCalculator
    {
        Tone ToneAt(Vector2 screenSpacePosition);

        float YPositionAt(Tone tone);

        float YPositionAt(KaraokeSaitenAction action);

        float CenterPosition();

        float Distance();

        Tone MaxTone();

        Tone MinTone();
    }
}
