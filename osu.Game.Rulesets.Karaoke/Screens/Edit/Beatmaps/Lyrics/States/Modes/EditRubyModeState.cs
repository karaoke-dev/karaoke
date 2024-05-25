// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.States.Modes;

public partial class EditRubyModeState : ModeStateWithBlueprintContainer<RubyTag>, IEditRubyModeState
{
    public Bindable<RubyTagEditStep> BindableEditStep { get; } = new();

    public Bindable<RubyTagEditMode> BindableRubyTagEditMode { get; } = new();

    protected override bool IsWriteLyricPropertyLocked(Lyric lyric)
        => HitObjectWritableUtils.IsWriteLyricPropertyLocked(lyric, nameof(Lyric.RubyTags));

    protected override bool SelectFirstProperty(Lyric lyric)
        => BindableEditStep.Value == RubyTagEditStep.Edit;

    protected override IEnumerable<RubyTag> SelectableProperties(Lyric lyric)
        => lyric.RubyTags;
}
