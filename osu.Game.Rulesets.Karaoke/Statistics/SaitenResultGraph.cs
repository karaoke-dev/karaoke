// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Statistics
{
    public class SaitenResultGraph : CompositeDrawable
    {
        private readonly Box background;
        private readonly SaitenResultLyricPreview lyricGraph;
        private readonly NoteGraph noteGraph;

        public SaitenResultGraph(ScoreInfo score, IBeatmap beatmap)
        {
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Masking = true,
                    CornerRadius = 5,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            Name = "Background",
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                        },
                        lyricGraph = new SaitenResultLyricPreview(beatmap)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Spacing = new Vector2(5),
                        },
                        noteGraph = new NoteGraph(score)
                    },
                },
            };

            lyricGraph.SelectedLyric.BindValueChanged(e =>
            {
                // todo : move noteGraph to target time.
            });
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.ContextMenuGray;
        }

        private class SaitenResultLyricPreview : LyricPreview
        {
            public SaitenResultLyricPreview(IBeatmap beatmap)
                : base(beatmap.HitObjects.OfType<Lyric>())
            {
            }

            protected override ClickableLyric CreateLyricContainer(Lyric lyric)
                => new SaitenResultClickableLyric(lyric);

            private class SaitenResultClickableLyric : ClickableLyric
            {
                public SaitenResultClickableLyric(Lyric lyric)
                    : base(lyric)
                {
                }

                protected override PreviewLyricSpriteText CreateLyric(Lyric lyric)
                    => new(lyric)
                    {
                        Font = new FontUsage(size: 15),
                        RubyFont = new FontUsage(size: 7),
                        RomajiFont = new FontUsage(size: 7),
                        Margin = new MarginPadding { Left = 5 }
                    };

                protected override Drawable CreateIcon()
                    => new Container();
            }
        }

        private class NoteGraph : CompositeDrawable
        {
            public NoteGraph(ScoreInfo score)
            {
                var noteEvents = score.HitEvents?.Where(x => x.HitObject is Note { Display: true }).ToList() ?? new List<HitEvent>();

                foreach (var noteEvent in noteEvents)
                {
                    // TODO : add note into here
                }

                // todo : add list of note colors to present state.
            }

            internal class DrawableNote : Box
            {
                internal DrawableNote(HitResult result)
                {
                    // TODO : assign color with different hit result.
                }
            }
        }
    }
}
