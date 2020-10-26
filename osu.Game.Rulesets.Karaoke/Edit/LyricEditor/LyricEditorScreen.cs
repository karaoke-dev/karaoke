// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Beatmaps;
using osu.Game.Database;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Beatmaps.Formats;
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
        private const string backup_lrc_name = "backup.lrc";

        private KaraokeLyricEditorSkin skin;
        private FillFlowContainer<Button> controls;
        private LyricRearrangeableListContainer container;

        public IEnumerable<string> HandledExtensions => LyricFotmatExtensions;

        public static string[] LyricFotmatExtensions { get; } = { ".lrc", ".kar" };

        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved]
        private BeatmapManager beatmaps { get; set; }

        public LyricEditorScreen()
            : base(EditorScreenMode.Compose)
        {
        }

        Task ICanAcceptFiles.Import(params string[] paths)
        {
            Schedule(() =>
            {
                var firstFile = new FileInfo(paths.First());

                if (LyricFotmatExtensions.Contains(firstFile.Extension))
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

            var set = Beatmap.Value.BeatmapSetInfo;
            var oldFile = set.Files?.FirstOrDefault(f => f.Filename == backup_lrc_name);
            using (var stream = info.OpenRead())
            {
                // todo : make a backup if has new lyric file.
                /*
                if (oldFile != null)
                    beatmaps.ReplaceFile(set, oldFile, stream, backup_lrc_name);
                else
                    beatmaps.AddFile(set, stream, backup_lrc_name);
                */

                // Import and replace all the file.
                using (var reader = new IO.LineBufferedReader(stream))
                {
                    var decoder = new LrcDecoder();
                    var lrcBeatmap = decoder.Decode(reader);

                    // todo : replace all the lyric object.
                }
            }

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
