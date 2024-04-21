// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.TimeTags;

public partial class TimeTagRecordingToolSection : EditorSection
{
    protected override LocalisableString Title => "Tool";

    [BackgroundDependencyLoader]
    private void load()
    {
        Children = new Drawable[]
        {
            new RecordingTapControl(),
        };
    }
}
