// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Components;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Info;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows
{
    public class EditLyricRow : LyricEditorRow
    {
        private const int min_height = 75;

        public EditLyricRow(Lyric lyric)
            : base(lyric)
        {
            AutoSizeAxes = Axes.Y;
        }

        protected override Drawable CreateLyricInfo(Lyric lyric)
        {
            return new InfoControl(lyric)
            {
                // todo : cannot use relative size to both because it will cause size cannot roll-back if make lyric smaller.
                RelativeSizeAxes = Axes.X,
                Height = min_height,
            };
        }

        protected override Drawable CreateContent(Lyric lyric)
        {
            return new SingleLyricEditor(lyric)
            {
                Margin = new MarginPadding { Left = 10 },
                RelativeSizeAxes = Axes.X,
            };
        }

        public class SingleLyricEditor : Container
        {
            [Cached]
            private readonly EditorKaraokeSpriteText karaokeSpriteText;

            public SingleLyricEditor(Lyric lyric)
            {
                CornerRadius = 5;
                AutoSizeAxes = Axes.Y;
                Padding = new MarginPadding { Bottom = 10 };
                Children = new Drawable[]
                {
                    karaokeSpriteText = new EditorKaraokeSpriteText(lyric),
                    new TimeTagLayer(lyric)
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    new CaretLayer(lyric)
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    new BlueprintLayer(lyric)
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                };
            }

            [BackgroundDependencyLoader]
            private void load(EditorClock clock)
            {
                karaokeSpriteText.Clock = clock;
            }
        }
    }
}
