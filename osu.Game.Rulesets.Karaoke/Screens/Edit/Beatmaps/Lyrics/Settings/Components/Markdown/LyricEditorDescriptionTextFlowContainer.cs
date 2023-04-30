// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Graphics.Containers.Markdown;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Components.Markdown;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Components.Markdown;

public partial class LyricEditorDescriptionTextFlowContainer : DescriptionTextFlowContainer
{
    protected override OsuMarkdownLinkText GetLinkTextByDescriptionAction(IDescriptionAction descriptionAction) =>
        descriptionAction switch
        {
            SwitchModeDescriptionAction switchMode => new SwitchMoteText(switchMode),
            _ => base.GetLinkTextByDescriptionAction(descriptionAction),
        };
}
