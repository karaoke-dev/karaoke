﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Lyrics;
using osu.Game.Rulesets.Karaoke.Edit.Blueprints.Notes;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Edit;

public partial class KaraokeBlueprintContainer : ComposeBlueprintContainer
{
    public KaraokeBlueprintContainer(HitObjectComposer composer)
        : base(composer)
    {
    }

    public override HitObjectSelectionBlueprint CreateHitObjectBlueprintFor(HitObject hitObject) =>
        hitObject switch
        {
            Note note => new NoteSelectionBlueprint(note),
            Lyric lyric => new LyricSelectionBlueprint(lyric),
            _ => throw new ArgumentOutOfRangeException(nameof(hitObject)),
        };

    protected override SelectionHandler<HitObject> CreateSelectionHandler() => new KaraokeSelectionHandler();

    protected override bool TryMoveBlueprints(DragEvent e, IList<(SelectionBlueprint<HitObject> blueprint, Vector2[] originalSnapPositions)> blueprints)
        => false;
}
