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

public partial class RubyTagEditModeSection : TextTagEditModeSection<IEditRubyModeState, RubyTagEditMode>
{
    protected override Selection CreateSelection(RubyTagEditMode mode) =>
        mode switch
        {
            RubyTagEditMode.Generate => new Selection(),
            RubyTagEditMode.Edit => new Selection(),
            RubyTagEditMode.Verify => new RubyTagVerifySelection(),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    protected override LocalisableString GetSelectionText(RubyTagEditMode mode) =>
        mode switch
        {
            RubyTagEditMode.Generate => "Generate",
            RubyTagEditMode.Edit => "Edit",
            RubyTagEditMode.Verify => "Verify",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    protected override Color4 GetSelectionColour(OsuColour colours, RubyTagEditMode mode, bool active) =>
        mode switch
        {
            RubyTagEditMode.Generate => active ? colours.Blue : colours.BlueDarker,
            RubyTagEditMode.Edit => active ? colours.Red : colours.RedDarker,
            RubyTagEditMode.Verify => active ? colours.Yellow : colours.YellowDarker,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    protected override DescriptionFormat GetSelectionDescription(RubyTagEditMode mode) =>
        mode switch
        {
            RubyTagEditMode.Generate => "Auto-generate rubies in the lyric.",
            RubyTagEditMode.Edit => new DescriptionFormat
            {
                Text = "Create / delete and edit lyric rubies in here.\n"
                       + $"Click [{DescriptionFormat.LINK_KEY_ACTION}](directions) to select the target lyric.\n"
                       + "Press `Tab` to switch between the ruby tags.\n"
                       + $"Than, press [{DescriptionFormat.LINK_KEY_ACTION}](adjust_text_tag_index) or button to adjust ruby index after hover to edit index area.",
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
            RubyTagEditMode.Verify => "Check invalid rubies in here",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

    private partial class RubyTagVerifySelection : LyricEditorVerifySelection
    {
        protected override LyricEditorMode EditMode => LyricEditorMode.EditRuby;
    }
}
