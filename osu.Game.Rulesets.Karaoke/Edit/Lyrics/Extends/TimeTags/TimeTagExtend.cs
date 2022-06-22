// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;
        public override float ExtendWidth => 300;

        private readonly IBindable<TimeTagEditMode> bindableMode = new Bindable<TimeTagEditMode>();

        public TimeTagExtend()
        {
            bindableMode.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case TimeTagEditMode.Create:
                        Children = new Drawable[]
                        {
                            new TimeTagEditModeSection(),
                            new TimeTagAutoGenerateSection(),
                            new TimeTagCreateConfigSection(),
                            new CreateTimeTagActionReceiver()
                        };
                        break;

                    case TimeTagEditMode.Recording:
                        Children = new Drawable[]
                        {
                            new TimeTagEditModeSection(),
                            new TimeTagRecordingConfigSection(),
                            new RecordTimeTagActionReceiver()
                        };
                        break;

                    case TimeTagEditMode.Adjust:
                        Children = new Drawable[]
                        {
                            new TimeTagEditModeSection(),
                            new TimeTagAdjustConfigSection(),
                            new TimeTagIssueSection(),
                        };
                        break;

                    default:
                        return;
                }
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(ITimeTagModeState timeTagModeState)
        {
            bindableMode.BindTo(timeTagModeState.BindableEditMode);
        }
    }
}
