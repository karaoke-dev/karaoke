// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Checks;

public abstract class CheckBeatmapProperty<TProperty, THitObject> : ICheck where THitObject : KaraokeHitObject
{
    public CheckMetadata Metadata => new(CheckCategory.Metadata, Description);

    protected abstract string Description { get; }

    public abstract IEnumerable<IssueTemplate> PossibleTemplates { get; }

    public IEnumerable<Issue> Run(BeatmapVerifierContext context)
    {
        var beatmap = getBeatmap(context);
        var property = GetPropertyFromBeatmap(beatmap);
        if (property == null)
            return Array.Empty<Issue>();

        var issues = CheckProperty(property);
        var hitObjectIssues = context.Beatmap.HitObjects.OfType<THitObject>().SelectMany(x => CheckHitObject(property, x));
        var hitObjectsIssues = CheckHitObjects(property, context.Beatmap.HitObjects.OfType<THitObject>().ToList());

        return issues.Concat(hitObjectIssues).Concat(hitObjectsIssues);
    }

    protected abstract TProperty? GetPropertyFromBeatmap(KaraokeBeatmap karaokeBeatmap);

    protected virtual IEnumerable<Issue> CheckProperty(TProperty property)
    {
        yield break;
    }

    protected virtual IEnumerable<Issue> CheckHitObject(TProperty property, THitObject hitObject)
    {
        yield break;
    }

    protected virtual IEnumerable<Issue> CheckHitObjects(TProperty property, IReadOnlyList<THitObject> hitObject)
    {
        yield break;
    }

    private static KaraokeBeatmap getBeatmap(BeatmapVerifierContext context)
    {
        // follow the usage in the IssueList in osu.Game
        if (context.Beatmap is EditorBeatmap editorBeatmap)
            return EditorBeatmapUtils.GetPlayableBeatmap(editorBeatmap);

        throw new InvalidOperationException();
    }
}
