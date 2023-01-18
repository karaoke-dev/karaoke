// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Extensions;
using osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Beatmaps;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Stages.Classic.Stage.Settings;

public partial class TimingPointsSection : EditorSection
{
    protected override LocalisableString Title => "Timings";

    public TimingPointsSection()
    {
        Add(new SectionPageInfoEditor());
    }

    private partial class SectionPageInfoEditor : SectionTimingInfoItemsEditor<ClassicLyricTimingPoint>
    {
        [BackgroundDependencyLoader]
        private void load(IStageEditorStateProvider stageEditorStateProvider)
        {
            Items.BindTo(stageEditorStateProvider.StageInfo.LyricTimingInfo.Timings);
        }

        protected override DrawableTimingInfoItem CreateTimingInfoDrawable(ClassicLyricTimingPoint item) => new DrawableTimingPoint(item);

        protected override EditorSectionButton? CreateCreateNewItemButton() => new CreateNewTimingPointButton();

        private partial class DrawableTimingPoint : DrawableTimingInfoItem
        {
            private readonly IBindable<int> timingPointsVersion = new Bindable<int>();

            [Resolved, AllowNull]
            private IBeatmapClassicStageChangeHandler beatmapClassicStageChangeHandler { get; set; }

            public DrawableTimingPoint(ClassicLyricTimingPoint item)
                : base(item)
            {
            }

            protected override void RemoveItem(ClassicLyricTimingPoint item)
            {
                beatmapClassicStageChangeHandler.RemoveTimingPoint(item);
            }

            [BackgroundDependencyLoader]
            private void load(IStageEditorStateProvider stageEditorStateProvider)
            {
                timingPointsVersion.BindTo(stageEditorStateProvider.StageInfo.LyricTimingInfo.TimingVersion);
                timingPointsVersion.BindValueChanged(_ =>
                {
                    int? order = stageEditorStateProvider.StageInfo.LyricTimingInfo.GetTimingPointOrder(Item);
                    double time = Item.Time;

                    ChangeDisplayOrder((int)time);
                    Text = $"#{order} {time.ToEditorFormattedString()}";
                }, true);
            }
        }

        private partial class CreateNewTimingPointButton : EditorSectionButton
        {
            [Resolved, AllowNull]
            private IBeatmapClassicStageChangeHandler beatmapClassicStageChangeHandler { get; set; }

            [Resolved, AllowNull]
            private EditorClock clock { get; set; }

            public CreateNewTimingPointButton()
            {
                Text = "Create new timing";
                Action = () =>
                {
                    double currentTime = clock.CurrentTime;
                    beatmapClassicStageChangeHandler.AddTimingPoint(x => x.Time = currentTime);
                };
            }
        }
    }
}
