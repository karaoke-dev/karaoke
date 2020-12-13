// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Database;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.Components.Timeline;
using osu.Game.Rulesets.Karaoke.Edit.ImportLyric;
using osu.Game.Screens.Edit;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    public class LyricEditorScreen : EditorScreenWithTimeline, ICanAcceptFiles
    {
        private FillFlowContainer<Button> controls;
        private LyricEditor lyricEditor;

        public IEnumerable<string> HandledExtensions => ImportLyricManager.LyricFormatExtensions;

        [Resolved(CanBeNull = true)]
        private DialogOverlay dialogOverlay { get; set; }

        [Resolved(CanBeNull = true)]
        private KaraokeHitObjectComposer composer { get; set; }

        [Cached]
        protected readonly TimeTagManager TranslateManager;
        [Cached]
        protected readonly LyricManager LyricManager;

        public LyricEditorScreen()
            : base(EditorScreenMode.Compose)
        {
            Content.Add(TranslateManager = new TimeTagManager());
            Content.Add(LyricManager = new LyricManager());
        }

        public Task Import(Stream stream, string filename)
        {
            // todo : wail until really implement needed.
            throw new System.NotImplementedException("Report to https://github.com/karaoke-dev/karaoke and i will implement it.");
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

            // todo : should open import screen instead.
            //dialogOverlay?.Push(new ImportLyricDialog(info));

            return true;
        }

        protected override Drawable CreateTimelineContent() => new TimelineBlueprintContainer(composer);

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
                                    Action = () => lyricEditor.FontSize += 3,
                                },
                                new OsuButton
                                {
                                    Width = 30,
                                    Height = 25,
                                    Text = "-",
                                    Action = () => lyricEditor.FontSize -= 3,
                                },
                            }
                        }
                    },
                    new Drawable[]
                    {
                        lyricEditor = new LyricEditor
                        {
                            RelativeSizeAxes = Axes.Both
                        },
                    }
                }
            };
        }
    }
}
