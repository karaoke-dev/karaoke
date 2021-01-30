// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Infos;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Components
{
    public class EditLyricRow : LyricEditorRow
    {
        private const int min_height = 75;
        private const int continuous_spacing = 20;

        public EditLyricRow(Lyric lyric)
            : base(lyric)
        {
            AutoSizeAxes = Axes.Y;
        }

        protected override Drawable CreateLyricInfo(Lyric lyric)
        {
            // todo : need to refactor this part.
            var isContinuous = lyric.LayoutIndex == -1;
            var continuousSpacing = isContinuous ? continuous_spacing : 0;

            return new InfoControl(lyric)
            {
                Margin = new MarginPadding
                {
                    Left = continuousSpacing,
                },
                // todo : cannot use relative size to both because it will cause size cannot roll-back if make lyric smaller.
                RelativeSizeAxes = Axes.X,
                Height = min_height,
            };
        }

        protected override Drawable CreateContent(Lyric lyric)
        {
            return new LyricControl(lyric)
            {
                Margin = new MarginPadding { Left = 10 },
                RelativeSizeAxes = Axes.X,
            };
        }
    }
}
