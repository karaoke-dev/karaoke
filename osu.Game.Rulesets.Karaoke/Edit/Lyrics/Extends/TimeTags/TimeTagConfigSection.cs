// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Components.Containers;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.TimeTags
{
    public class TimeTagConfigSection : Section
    {
        protected override string Title => "Config";

        [BackgroundDependencyLoader]
        private void load(ILyricEditorState state)
        {
            state.BindableMode.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case LyricEditorMode.RecordTimeTag:
                        Show();
                        Children = new[]
                        {
                            new LabelledDropdown<RecordingMovingCaretMode>
                            {
                                Label = "Record tag",
                                Description = "Only record time with start/end time-tag while recording.",
                                Current = state.BindableRecordingMovingCaretMode,
                                Items = EnumUtils.GetValues<RecordingMovingCaretMode>(),
                            }
                        };
                        break;

                    default:
                        Hide();
                        Children = new Drawable[] { };
                        break;
                }
            }, true);
        }
    }
}
