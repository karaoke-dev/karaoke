// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions.Color4Extensions;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Skinning.Elements
{
    public class NoteStyle : IKaraokeSkinElement
    {
        public static readonly NoteStyle DEFAULT = new()
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
    }
}
