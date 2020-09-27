// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osuTK;
using System.Collections.Generic;
using System.Linq;

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

            lyricGraph.SelectedLyricLine.BindValueChanged(e =>
            {
                // todo : move noteGraph to target time.
            });
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            background.Colour = colours.ContextMenuGray;
        }

        internal class SaitenResultLyricPreview : LyricPreview
        {
            public SaitenResultLyricPreview(IBeatmap beatmap)
                : base(beatmap.HitObjects.OfType<LyricLine>())
            {
            }

            protected override ClickableLyric CreateLyricContainer(LyricLine lyric)
                => new SaitenResultClickableLyric(lyric);

            internal class SaitenResultClickableLyric : ClickableLyric
            {
                public SaitenResultClickableLyric(LyricLine lyric)
                    : base(lyric)
                {
                }

                protected override PreviewLyricSpriteText CreateLyric(LyricLine lyric)
                    => new PreviewLyricSpriteText(lyric)
                    {
                        Font = new FontUsage(size: 15),
                        RubyFont = new FontUsage(size: 7),
                        RomajiFont = new FontUsage(size: 7),
                        Margin = new MarginPadding { Left = 5}
                    };

                protected override Drawable CreateIcon()
                    => new Container();
            }
        }

        internal class NoteGraph : CompositeDrawable
        {
            public NoteGraph(ScoreInfo score)
            {
                var noteEvents = score.HitEvents?.Where(x => x.HitObject is Note note && note.Display).ToList() ?? new List<HitEvent>();
                foreach (var noteEvent in noteEvents)
                {
                    // TOOD : add note into here
                }

                // todo : add list of note colors to present state.
            }

            internal class DrawableNote : Box
            {
                internal DrawableNote(HitResult result)
                {
                    // TOOD : assign color with different hit result.
                }
            }
        }
    }
}
