// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Singers;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings;

public partial class SingerSettings : LyricEditorSettings
{
    public override SettingsDirection Direction => SettingsDirection.Left;

    public override float SettingsWidth => 300;

    protected override IReadOnlyList<EditorSection> CreateEditorSections() => new[]
    {
        new SingerEditSection(),
    };
}
