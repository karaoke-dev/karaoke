// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Timing;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.LyricEditor.Components;
using osu.Game.Rulesets.Karaoke.Edit.Timelines;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor
{
    public class LyricEditorScreen : EditorScreenWithTimeline
    {
        private FillFlowContainer<Button> controls;
        private FillFlowContainer<LyricControl> container;

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        protected override Drawable CreateTimelineContent() => new KaraokeTimelineBlueprintContainer();

        protected override Drawable CreateMainContent()
        {
            return new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.Absolute, 30)
                },
                Content = new Drawable[][]
                {
                    new Drawable[]
                    {
                        controls = new FillFlowContainer<Button>
                        {
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Spacing = new osuTK.Vector2(10),
                            Children = new []
                            {
                                new OsuButton
                                {
                                    Width = 30,
                                    Height = 25,
                                    Text = "+"
                                },
                                new OsuButton
                                {
                                    Width = 30,
                                    Height = 25,
                                    Text = "-"
                                },
                            }
                        }
                    },
                    new Drawable[]
                    {
                        new SkinProvidingContainer(new KaraokeLegacySkinTransformer(null))
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = new OsuScrollContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = container = new FillFlowContainer<LyricControl>
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Vertical,
                                    Padding = new MarginPadding { Right = 50 },
                                }
                            }
                        }
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(IFrameBasedClock framedClock)
        {
            container.Clock = framedClock;

            foreach (var obj in beatmap.HitObjects)
                Schedule(() => addHitObject(obj));
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            beatmap.HitObjectAdded += addHitObject;
            beatmap.HitObjectRemoved += removeHitObject;
        }

        private void addHitObject(HitObject hitObject)
        {
            // see how `DrawableEditRulesetWrapper` do
            if (hitObject is LyricLine lyric)
            {
                container.Add(new LyricControl(lyric)
                {
                    RelativeSizeAxes = Axes.X,
                });
            }
        }

        private void removeHitObject(HitObject hitObject)
        {
            if (!(hitObject is LyricLine lyric))
                return;

            var drawableHitObject = container.Children.FirstOrDefault(x => x.Lyric == lyric);
            container.Remove(drawableHitObject);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (beatmap == null)
                return;

            beatmap.HitObjectAdded -= addHitObject;
            beatmap.HitObjectRemoved -= removeHitObject;
        }
    }
}
