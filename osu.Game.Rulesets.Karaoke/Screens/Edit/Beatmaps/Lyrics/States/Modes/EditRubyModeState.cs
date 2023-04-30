// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public partial class EditRubyModeState : ModeStateWithBlueprintContainer<RubyTag>, IEditRubyModeState
{
    private readonly Bindable<RubyTagEditMode> bindableEditMode = new();

    public IBindable<RubyTagEditMode> BindableEditMode => bindableEditMode;

    public void ChangeEditMode(RubyTagEditMode mode)
        => bindableEditMode.Value = mode;

    protected override bool IsWriteLyricPropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.RubyTags));

    protected override bool SelectFirstProperty(Lyric lyric)
        => BindableEditMode.Value == RubyTagEditMode.Edit;

    protected override IEnumerable<RubyTag> SelectableProperties(Lyric lyric)
        => lyric.RubyTags;
}
