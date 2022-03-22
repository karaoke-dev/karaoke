// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Singers;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Singers
{
    [Cached(typeof(ISingerScreenScrollingInfoProvider))]
    public class SingerScreen : KaraokeEditorRoundedScreen, ISingerScreenScrollingInfoProvider
    {
        [Cached(typeof(ISingersChangeHandler))]
        private readonly SingersChangeHandler singersChangeHandler;

        [Cached(typeof(ILyricSingerChangeHandler))]
        private readonly LyricSingerChangeHandler lyricSingerChangeHandler;

        [Cached]
        private readonly BindableList<Lyric> selectedLyrics = new();

        public BindableFloat BindableZoom { get; } = new();

        public BindableFloat BindableCurrent { get; } = new();

        public SingerScreen()
            : base(KaraokeEditorScreenMode.Singer)
        {
            AddInternal(singersChangeHandler = new SingersChangeHandler());
            AddInternal(lyricSingerChangeHandler = new LyricSingerChangeHandler());
        }

        [BackgroundDependencyLoader]
        private void load(EditorBeatmap editorBeatmap, EditorClock editorClock)
        {
            BindablesUtils.Sync(selectedLyrics, editorBeatmap.SelectedHitObjects);

            // initialize scroll zone.
            BindableZoom.MaxValue = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 8000);
            BindableZoom.MinValue = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 80000);
            BindableZoom.Value = BindableZoom.Default = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 40000);

            Add(new FixedSectionsContainer<Drawable>
            {
                FixedHeader = new SingerScreenHeader(),
                RelativeSizeAxes = Axes.Both,
                Children = new[]
                {
                    new SingerEditSection
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                }
            });
        }

        protected override void PopOut()
        {
            base.PopOut();

            // should clear the selected lyrics because other place might not support multi select.
            selectedLyrics.Clear();
        }

        private class FixedSectionsContainer<T> : SectionsContainer<T> where T : Drawable
        {
            private readonly Container<T> content;

            // todo: check what this shit doing.
            protected override Container<T> Content => content;

            public FixedSectionsContainer()
            {
                AddInternal(content = new Container<T>
                {
                    Masking = true,
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Top = 55 }
                });
            }
        }

        private class SingerScreenHeader : OverlayHeader
        {
            protected override OverlayTitle CreateTitle() => new TranslateScreenTitle();

            private class TranslateScreenTitle : OverlayTitle
            {
                public TranslateScreenTitle()
                {
                    Title = "singer";
                    Description = "create singer of your beatmap";
                    IconTexture = "Icons/Hexacons/social";
                }
            }
        }
    }
}
