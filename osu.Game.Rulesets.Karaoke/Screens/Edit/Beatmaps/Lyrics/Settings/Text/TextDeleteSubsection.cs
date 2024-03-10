// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Text;

public partial class TextDeleteSubsection : SelectLyricButton
{
    [Resolved]
    private ILyricsChangeHandler lyricsChangeHandler { get; set; } = null!;

    protected override LocalisableString StandardText => "Delete";

    protected override LocalisableString SelectingText => "Cancel delete";

    protected override void Apply()
    {
        lyricsChangeHandler.Remove();
    }
}
