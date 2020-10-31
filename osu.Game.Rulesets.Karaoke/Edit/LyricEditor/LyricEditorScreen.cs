// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Beatmaps;
using osu.Game.Database;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Import;
using osu.Game.Rulesets.Karaoke.Edit.Timelines;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;
using osu.Game.Skinning;
using osuTK;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace osu.Game.Rulesets.Karaoke.Edit.LyricEditor
{
    public class LyricEditorScreen : EditorScreenWithTimeline, ICanAcceptFiles
    {
        private KaraokeLyricEditorSkin skin;
        private FillFlowContainer<Button> controls;
        private LyricRearrangeableListContainer container;

        public IEnumerable<string> HandledExtensions => ImportManager.LyricFotmatExtensions;

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved]
        private BeatmapManager beatmaps { get; set; }

        [Resolved(CanBeNull = true)]
        private DialogOverlay dialogOverlay { get; set; }

        public LyricEditorScreen()
            : base(EditorScreenMode.Compose)
        {
        }

        Task ICanAcceptFiles.Import(params string[] paths)
        {
            Schedule(() =>
            {
                var firstFile = new FileInfo(paths.First());

                if (HandledExtensions.Contains(firstFile.Extension))
                {
                    // Import lyric file
                    ImportLyricFile(firstFile);
                }
            });
            return Task.CompletedTask;
        }

        public bool ImportLyricFile(FileInfo info)
        {
            if (!info.Exists)
                return false;

            dialogOverlay?.Push(new ImportLyricDialog(info));

            return true;
        }

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
                Content = new[]
                {
                    new Drawable[]
                    {
                        controls = new FillFlowContainer<Button>
                        {
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Spacing = new Vector2(10),
                            Children = new[]
                            {
                                new OsuButton
                                {
                                    Width = 30,
                                    Height = 25,
                                    Text = "+",
                                    Action = () => skin.FontSize += 3,
                                },
                                new OsuButton
                                {
                                    Width = 30,
                                    Height = 25,
                                    Text = "-",
                                    Action = () => skin.FontSize -= 3,
                                },
                            }
                        }
                    },
                    new Drawable[]
                    {
                        new SkinProvidingContainer(skin = new KaraokeLyricEditorSkin())
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = container = new LyricRearrangeableListContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                            }
                        }
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
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
            if (hitObject is Lyric lyric)
            {
                container.Items.Add(lyric);
            }
        }

        private void removeHitObject(HitObject hitObject)
        {
            if (!(hitObject is Lyric lyric))
                return;

            container.Items.Remove(lyric);
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
