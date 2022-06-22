// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects.Drawables;
using osu.Game.Rulesets.Karaoke.Skinning.Default;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning.Elements
{
    public class NoteStyle : IKaraokeSkinElement
    {
        public static NoteStyle CreateDefault() => new()
        {
            Name = "Default",
            NoteColor = Color4Extensions.FromHex("#44AADD"),
            BlinkColor = Color4Extensions.FromHex("#FF66AA"),
            TextColor = Color4Extensions.FromHex("#FFFFFF"),
            BoldText = true,
        };

        public int ID { get; set; }

        public string Name { get; set; }

        public Color4 NoteColor { get; set; }

        public Color4 BlinkColor { get; set; }

        public Color4 TextColor { get; set; }

        public bool BoldText { get; set; }

        public void ApplyTo(Drawable d)
        {
            if (d is not DrawableNote drawableNote)
                throw new InvalidDrawableTypeException(nameof(d));

            drawableNote.ApplyToLyricText(text =>
            {
                text.Colour = TextColor;
            });

            drawableNote.ApplyToBackground(background =>
            {
                if (background.Drawable is not DefaultBodyPiece defaultBodyPiece)
                    return;

                defaultBodyPiece.AccentColour.Value = NoteColor;
                defaultBodyPiece.HitColour.Value = BlinkColor;
            });
        }
    }
}
