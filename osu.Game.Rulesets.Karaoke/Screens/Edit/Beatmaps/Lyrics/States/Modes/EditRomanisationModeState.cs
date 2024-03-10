// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Settings.Romanisation;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public partial class EditRomanisationModeState : ModeStateWithBlueprintContainer<TimeTag>, IEditRomanisationModeState
{
    private readonly Bindable<RomanisationTagEditStep> bindableEditMode = new();

    public IBindable<RomanisationTagEditStep> BindableEditStep => bindableEditMode;

    public void ChangeEditStep(RomanisationTagEditStep step)
        => bindableEditMode.Value = step;

    public Bindable<RomanisationEditPropertyMode> BindableRomajiEditPropertyMode { get; } = new();

    protected override bool IsWriteLyricPropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.TimeTags));

    protected override bool SelectFirstProperty(Lyric lyric)
        => BindableEditStep.Value == RomanisationTagEditStep.Edit;

    protected override IEnumerable<TimeTag> SelectableProperties(Lyric lyric)
        => lyric.TimeTags;
}
