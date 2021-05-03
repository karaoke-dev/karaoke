// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Default;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics
{
    public class EditorLyricPiece : DefaultLyricPiece
    {
        public Action<LyricFont> ApplyFontAction;

        public EditorLyricPiece(Lyric lyric)
            : base(lyric)
        {
            DisplayRuby = true;
            DisplayRomaji = true;
        }

        public override void ApplyFont(LyricFont font)
        {
            ApplyFontAction?.Invoke(font);
            base.ApplyFont(font);
        }

        public float GetPercentageWidth(int startIndex, int endIndex, float percentage = 0)
        {
            return GetPercentageWidth(getTextIndexByIndex(startIndex), getTextIndexByIndex(endIndex), percentage);

            // todo : it's a temp way to get position.
            TextIndex getTextIndexByIndex(int index)
            {
                if (Text?.Length <= index)
                    return new TextIndex(index - 1, TextIndex.IndexState.End);

                return new TextIndex(index);
            }
        }

        public TextIndex GetHoverIndex(float position)
        {
            var text = Text;
            if (string.IsNullOrEmpty(text))
                return new TextIndex();

            for (int i = 0; i < text.Length; i++)
            {
                if (GetPercentageWidth(i, i + 1, 0.5f) > position)
                    return new TextIndex(i);

                if (GetPercentageWidth(i, i + 1, 1f) > position)
                    return new TextIndex(i, TextIndex.IndexState.End);
            }

            return new TextIndex(text.Length - 1, TextIndex.IndexState.End);
        }
    }
}
