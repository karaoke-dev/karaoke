// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.RubyRomaji;

public partial class RomajiTagEditModeSection : TextTagEditModeSection<IEditRomajiModeState, RomajiTagEditMode>
{
    protected override Selection CreateSelection(RomajiTagEditMode mode) =>
        mode switch
        {
            RomajiTagEditMode.Generate => new Selection(),
            RomajiTagEditMode.Edit => new Selection(),
            RomajiTagEditMode.Verify => new RomajiTagVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    protected override LocalisableString GetSelectionText(RomajiTagEditMode mode) =>
        mode switch
        {
            RomajiTagEditMode.Generate => "Generate",
            RomajiTagEditMode.Edit => "Edit",
            RomajiTagEditMode.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    protected override Color4 GetSelectionColour(OsuColour colours, RomajiTagEditMode mode, bool active) =>
        mode switch
        {
            RomajiTagEditMode.Generate => active ? colours.Blue : colours.BlueDarker,
            RomajiTagEditMode.Edit => active ? colours.Red : colours.RedDarker,
            RomajiTagEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    protected override DescriptionFormat GetSelectionDescription(RomajiTagEditMode mode) =>
        mode switch
        {
            RomajiTagEditMode.Generate => "Auto-generate romajies in the lyric.",
            RomajiTagEditMode.Edit => new DescriptionFormat
            {
                Text = "Create / delete and edit lyric rubies in here.\n"
                       + $"Click [{DescriptionFormat.LINK_KEY_ACTION}](directions) to select the target lyric.\n"
                       + "Press `Tab` to switch between the romaji tags.\n"
                       + $"Than, press [{DescriptionFormat.LINK_KEY_ACTION}](adjust_text_tag_index) or button to adjust romaji index after hover to edit index area.",
                Actions = new Dictionary<string, IDescriptionAction>
                {
                    {
                        "directions", new InputKeyDescriptionAction
                        {
                            Text = "Up or down",
                            AdjustableActions = new List<KaraokeEditAction>
                            {
                                KaraokeEditAction.MoveToPreviousLyric,
                                KaraokeEditAction.MoveToNextLyric
                            }
                        }
                    },
                    {
                        "adjust_text_tag_index", new InputKeyDescriptionAction
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
            },
            RomajiTagEditMode.Verify => "Check invalid romajies in here.",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    private partial class RomajiTagVerifySelection : LyricEditorVerifySelection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditRomaji;
    }
}
