// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends
{
    public class TimeTagExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;
        public override float ExtendWidth => 300;

        private readonly IBindable<TimeTagEditMode> bindableMode = new Bindable<TimeTagEditMode>();

        [BackgroundDependencyLoader]
        private void load(ITimeTagModeState timeTagModeState)
        {
            bindableMode.BindTo(timeTagModeState.BindableEditMode);
            bindableMode.BindValueChanged(e =>
            {
                ReloadSections();
            }, true);
        }

        protected override IReadOnlyList<Drawable> CreateSections() => bindableMode.Value switch
        {
            TimeTagEditMode.Create => new Drawable[]
            {
                new TimeTagEditModeSection(),
                new TimeTagAutoGenerateSection(),
                new TimeTagCreateConfigSection(),
                new CreateTimeTagActionReceiver()
            },
            TimeTagEditMode.Recording => new Drawable[]
            {
                new TimeTagEditModeSection(),
                new TimeTagRecordingConfigSection(),
                new RecordTimeTagActionReceiver()
            },
            TimeTagEditMode.Adjust => new Drawable[]
            {
                new TimeTagEditModeSection(),
                new TimeTagAdjustConfigSection(),
                new TimeTagIssueSection(),
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
