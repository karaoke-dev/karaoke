// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes
{
    public class TimeTagModeState : Component, ITimeTagModeState
    {
        public BindableList<TimeTag> SelectedItems { get; } = new();

        public BindableFloat BindableRecordZoom { get; } = new();

        public BindableFloat BindableAdjustZoom { get; } = new();

        [BackgroundDependencyLoader]
        private void load(EditorClock editorClock)
        {
            BindableRecordZoom.MaxValue = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 800);
            BindableRecordZoom.MinValue = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 4000);
            BindableRecordZoom.Value = BindableRecordZoom.Default = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 2000);

            BindableAdjustZoom.MaxValue = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 1600);
            BindableAdjustZoom.MinValue = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 8000);
            BindableAdjustZoom.Value = BindableAdjustZoom.Default = ZoomableScrollContainerUtils.GetZoomLevelForVisibleMilliseconds(editorClock, 4000);
        }
    }
}
