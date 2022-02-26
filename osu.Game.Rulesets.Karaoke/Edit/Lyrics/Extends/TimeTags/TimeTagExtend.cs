// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagExtend : EditExtend
    {
        public override ExtendDirection Direction => ExtendDirection.Right;
        public override float ExtendWidth => 300;

        private IBindable<LyricEditorMode> bindableMode;

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            bindableMode = state.BindableMode.GetBoundCopy();
            bindableMode.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case LyricEditorMode.CreateTimeTag:
                        Children = new Drawable[]
                        {
                            new TimeTagEditModeSection(),
                            new TimeTagAutoGenerateSection(),
                            new TimeTagCreateConfigSection(),
                        };
                        break;

                    case LyricEditorMode.RecordTimeTag:
                        Children = new Drawable[]
                        {
                            new TimeTagEditModeSection(),
                            new TimeTagRecordingConfigSection(),
                        };
                        break;

                    case LyricEditorMode.AdjustTimeTag:
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
    }
}
