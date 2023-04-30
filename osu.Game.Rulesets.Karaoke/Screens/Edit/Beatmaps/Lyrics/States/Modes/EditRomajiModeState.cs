// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public partial class EditRomajiModeState : ModeStateWithBlueprintContainer<RomajiTag>, IEditRomajiModeState
{
    private readonly Bindable<RomajiTagEditMode> bindableEditMode = new();

    public IBindable<RomajiTagEditMode> BindableEditMode => bindableEditMode;

    public void ChangeEditMode(RomajiTagEditMode mode)
        => bindableEditMode.Value = mode;

    protected override bool IsWriteLyricPropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.RomajiTags));

    protected override bool SelectFirstProperty(Lyric lyric)
        => BindableEditMode.Value == RomajiTagEditMode.Edit;

    protected override IEnumerable<RomajiTag> SelectableProperties(Lyric lyric)
        => lyric.RomajiTags;
}
