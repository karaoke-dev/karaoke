// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Replays;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.UI.Position
{
    public class NotePositionCalculator
    {
        private readonly int columns;
        private readonly float columnSpacing;
        private readonly Tone offset;

        public float ColumnHeight { get; }

        public NotePositionCalculator(int columns, float columnHeight, float columnSpacing, Tone offset = new Tone())
        {
            ColumnHeight = columnHeight;

            // todo : not sure should column can be even.
            this.columns = columns;
            this.columnSpacing = columnSpacing;
            this.offset = offset;
        }

        public Tone ToneAt(Vector2 screenSpacePosition) => throw new NotImplementedException();

        public float YPositionAt(Note note) => YPositionAt(note.Tone);

        public float YPositionAt(Tone tone) => YPositionAt(toFloat(tone));

        public float YPositionAt(KaraokeSaitenAction action) => YPositionAt(action.Scale);

        public float YPositionAt(KaraokeReplayFrame frame) => YPositionAt(frame.Scale);

        public float YPositionAt(float scale) => -(columnSpacing + ColumnHeight) * Math.Clamp(scale, toFloat(MinTone), toFloat(MaxTone));

        public Tone MaxTone =>
            new Tone
            {
                Scale = columns / 2
            };

        public Tone MinTone => -MaxTone;

        private float toFloat(Tone tone)
            => tone.Scale + (tone.Half ? 0.5f : 0);
    }
}
