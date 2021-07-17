// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.Position
{
    public class NotePositionCalculator
    {
        private readonly int column;
        private readonly float columnHeight;
        private readonly float columnSpacing;
        private readonly Tone offset;

        public NotePositionCalculator(int column, float columnHeight, float columnSpacing, Tone offset = new Tone())
        {
            this.column = column;
            this.columnHeight = columnHeight;
            this.columnSpacing = columnSpacing;
            this.offset = offset;
        }

        public Tone ToneAt(Vector2 screenSpacePosition) => throw new NotImplementedException();

        public float YPositionAt(Tone tone) => -(columnSpacing + columnHeight) * (tone.Scale + (tone.Half ? 0.5f : 0));

        public float YPositionAt(Note note) => YPositionAt(note.Tone);

        public float YPositionAt(KaraokeSaitenAction action) => -(columnSpacing + columnHeight) * action.Scale;

        public float CenterPosition() => YPositionAt(new Tone { Scale = column / 2, Half = column % 2 == 1 });

        public float Height() => columnHeight;

        public Tone MaxTone() =>
            new Tone
            {
                Scale = column / 2
            };

        public Tone MinTone() => -MaxTone();
    }
}
