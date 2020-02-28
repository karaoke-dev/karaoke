// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.UI.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.Position
{
    public class PositionCalculator : IPositionCalculator
    {
        private readonly int column;

        public PositionCalculator(int column)
        {
            this.column = column;
        }

        public Tone ToneAt(Vector2 screenSpacePosition) => throw new NotImplementedException();

        public float YPositionAt(Tone tone) => -(NotePlayfield.COLUMN_SPACING + ColumnBackground.COLUMN_HEIGHT) * (tone.Scale + (tone.Half ? 0.5f : 0));

        public float YPositionAt(KaraokeSaitenAction action) => -(NotePlayfield.COLUMN_SPACING + ColumnBackground.COLUMN_HEIGHT) * action.Scale;

        public float CenterPosition() => YPositionAt(new Tone { Scale = column / 2, Half = column % 2 == 1 });

        public float Distance() => YPositionAt(new Tone { Scale = 1 });

        public Tone MaxTone() =>
            new Tone
            {
                Scale = column / 2
            };

        public Tone MinTone() => -MaxTone();
    }
}
