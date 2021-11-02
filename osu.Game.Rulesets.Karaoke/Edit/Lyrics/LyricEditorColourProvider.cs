// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricEditorColourProvider
    {
        public Color4 Colour1(LyricEditorMode mode) => getColour(mode, 1, 0.7f);
        public Color4 Colour2(LyricEditorMode mode) => getColour(mode, 0.8f, 0.6f);
        public Color4 Colour3(LyricEditorMode mode) => getColour(mode, 0.6f, 0.5f);
        public Color4 Colour4(LyricEditorMode mode) => getColour(mode, 0.4f, 0.3f);

        public Color4 Highlight1(LyricEditorMode mode) => getColour(mode, 1, 0.7f);
        public Color4 Content1(LyricEditorMode mode) => getColour(mode, 0.4f, 1);
        public Color4 Content2(LyricEditorMode mode) => getColour(mode, 0.4f, 0.9f);
        public Color4 Light1(LyricEditorMode mode) => getColour(mode, 0.4f, 0.8f);
        public Color4 Light2(LyricEditorMode mode) => getColour(mode, 0.4f, 0.75f);
        public Color4 Light3(LyricEditorMode mode) => getColour(mode, 0.4f, 0.7f);
        public Color4 Light4(LyricEditorMode mode) => getColour(mode, 0.4f, 0.5f);
        public Color4 Dark1(LyricEditorMode mode) => getColour(mode, 0.2f, 0.35f);
        public Color4 Dark2(LyricEditorMode mode) => getColour(mode, 0.2f, 0.3f);
        public Color4 Dark3(LyricEditorMode mode) => getColour(mode, 0.2f, 0.25f);
        public Color4 Dark4(LyricEditorMode mode) => getColour(mode, 0.2f, 0.2f);
        public Color4 Dark5(LyricEditorMode mode) => getColour(mode, 0.2f, 0.15f);
        public Color4 Dark6(LyricEditorMode mode) => getColour(mode, 0.2f, 0.1f);
        public Color4 Foreground1(LyricEditorMode mode) => getColour(mode, 0.1f, 0.6f);
        public Color4 Background1(LyricEditorMode mode) => getColour(mode, 0.1f, 0.4f);
        public Color4 Background2(LyricEditorMode mode) => getColour(mode, 0.1f, 0.3f);
        public Color4 Background3(LyricEditorMode mode) => getColour(mode, 0.1f, 0.25f);
        public Color4 Background4(LyricEditorMode mode) => getColour(mode, 0.1f, 0.2f);
        public Color4 Background5(LyricEditorMode mode) => getColour(mode, 0.1f, 0.15f);
        public Color4 Background6(LyricEditorMode mode) => getColour(mode, 0.1f, 0.1f);

        private Color4 getColour(LyricEditorMode mode, float saturation, float lightness) => Color4.FromHsl(new Vector4(getBaseHue(mode), saturation, lightness, 1));

        private static float getBaseHue(LyricEditorMode mode)
        {
            switch (mode)
            {
                case LyricEditorMode.View:
                    return 182 / 360f; // miku blue

                case LyricEditorMode.Manage:
                    return 50 / 360f; // yellow

                case LyricEditorMode.Typing:
                case LyricEditorMode.Language:
                case LyricEditorMode.EditRuby:
                case LyricEditorMode.EditRomaji:
                    return 333 / 360f; // pink

                case LyricEditorMode.CreateTimeTag:
                case LyricEditorMode.RecordTimeTag:
                case LyricEditorMode.AdjustTimeTag:
                    return 33 / 360f; // orange

                case LyricEditorMode.EditNote:
                    return 203 / 360f; // blue

                case LyricEditorMode.Layout:
                case LyricEditorMode.Singer:
                    return 271 / 360f; // purple

                default:
                    throw new InvalidEnumArgumentException($@"{mode} colour scheme does not provide a hue value in {nameof(getBaseHue)}.");
            }
        }
    }
}
