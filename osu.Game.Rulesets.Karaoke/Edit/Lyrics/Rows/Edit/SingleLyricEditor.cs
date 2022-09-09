// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Rows.Edit
{
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
