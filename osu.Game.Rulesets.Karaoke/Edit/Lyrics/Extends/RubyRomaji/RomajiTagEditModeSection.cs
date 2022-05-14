// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.Components.Description;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RomajiTagEditModeSection : TextTagEditModeSection
    {
        [Resolved]
        private IEditRomajiModeState editRomajiModeState { get; set; }

        protected override TextTagEditMode DefaultMode()
            => editRomajiModeState.EditMode;

        protected override Dictionary<TextTagEditMode, EditModeSelectionItem> CreateSelections()
            => new()
            {
                {
                    TextTagEditMode.Generate, new EditModeSelectionItem("Generate", "Auto-generate romajies in the lyric.")
                },
                {
                    TextTagEditMode.Edit,
                    new EditModeSelectionItem("Edit", new DescriptionFormat
                    {
                        Text = "Create / delete and edit lyric rubies in here.\n"
                               + "Click [key](directions) to select the target lyric.\n"
                               + "Press `Tab` to switch between the romaji tags.\n"
                               + "Than, press [key](adjust_text_tag_index) or button to adjust romaji index after hover to edit index area.",
                        Keys = new Dictionary<string, InputKey>
                        {
                            {
                                "directions", new InputKey
                                {
                                    Text = "Up or down",
                                    AdjustableActions = new List<KaraokeEditAction>
                                    {
                                        KaraokeEditAction.Up,
                                        KaraokeEditAction.Down
                                    }
                                }
                            },
                            {
                                "adjust_text_tag_index", new InputKey
                                {
                                    Text = "Keys",
                                    AdjustableActions = new List<KaraokeEditAction>
                                    {
                                        KaraokeEditAction.EditTextTagReduceStartIndex,
                                        KaraokeEditAction.EditTextTagIncreaseStartIndex,
                                        KaraokeEditAction.EditTextTagReduceEndIndex,
                                        KaraokeEditAction.EditTextTagIncreaseEndIndex,
                                    }
                                }
                            }
                        }
                    })
                },
                {
                    TextTagEditMode.Verify, new EditModeSelectionItem("Verify", "Check invalid romajies in here.")
                }
            };

        internal override void UpdateEditMode(TextTagEditMode mode)
        {
            editRomajiModeState.ChangeEditMode(mode);

            base.UpdateEditMode(mode);
        }
    }
}
