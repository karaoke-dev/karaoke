// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Default;
using osu.Game.Rulesets.Karaoke.Skinning.Metadatas.Fonts;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.LyricRows.Lyrics
{
    public class EditorLyricPiece : DefaultLyricPiece
    {
        public Action<LyricFont> ApplyFontAction;

        protected Lyric HitObject;

        public EditorLyricPiece(Lyric lyric)
            : base(lyric)
        {
            HitObject = lyric;

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

        [BackgroundDependencyLoader(true)]
        private void load(ISkinSource skin)
        {
            // this is a temp way to apply font.
            skin.GetConfig<KaraokeSkinLookup, LyricFont>(new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricStyle, HitObject.Singers))?.BindValueChanged(karaokeFont =>
            {
                var newFont = karaokeFont.NewValue;
                if (newFont == null)
                    return;

                ApplyFont(karaokeFont.NewValue);

                // Apply text font info
                var lyricFont = newFont.LyricTextFontInfo.LyricTextFontInfo;
                Font = getFont(lyricFont.CharSize);

                var rubyFont = newFont.RubyTextFontInfo.LyricTextFontInfo;
                RubyFont = getFont(rubyFont.CharSize);

                var romajiFont = newFont.RomajiTextFontInfo.LyricTextFontInfo;
                RomajiFont = getFont(romajiFont.CharSize);

                static FontUsage getFont(float? charSize = null)
                    => FontUsage.Default.With(size: charSize * 2);
            }, true);
        }
    }
}
