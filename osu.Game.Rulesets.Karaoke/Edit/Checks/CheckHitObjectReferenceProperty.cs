// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public abstract class CheckHitObjectReferenceProperty<THitObject, TReferencedHitObject> : CheckHitObjectProperty<THitObject>
    where THitObject : KaraokeHitObject
    where TReferencedHitObject : KaraokeHitObject
{
    public override IEnumerable<Issue> Run(BeatmapVerifierContext context)
    {
        var hitObjects = context.CurrentDifficulty.Playable.HitObjects.OfType<THitObject>();
        var allAvailableReferencedHitObjects = context.CurrentDifficulty.Playable.HitObjects.OfType<TReferencedHitObject>().ToArray();

        var issues = base.Run(context);
        var referenceIssues = hitObjects.Select(x => CheckReferenceProperty(x, allAvailableReferencedHitObjects)).SelectMany(x => x);

        return issues.Concat(referenceIssues);
    }

    protected sealed override IEnumerable<Issue> Check(THitObject hitObject)
    {
        yield break;
    }

    protected abstract IEnumerable<Issue> CheckReferenceProperty(THitObject hitObject, IEnumerable<TReferencedHitObject> allAvailableReferencedHitObjects);
}
