// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Caching;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics;

public partial class LyricEditorVerifier : EditorVerifier<LyricEditorMode>, ILyricEditorVerifier
{
    [Resolved]
    private EditorBeatmap beatmap { get; set; } = null!;

    private readonly Dictionary<KaraokeHitObject, BindableList<Issue>> hitObjectIssues = new();

    private readonly Cached editModeCache = new();

    protected override IEnumerable<ICheck> CreateChecks(LyricEditorMode type) =>
        type switch
        {
            LyricEditorMode.View => Array.Empty<ICheck>(),
            LyricEditorMode.Texting => new ICheck[] { new CheckLyricText() },
            LyricEditorMode.Reference => new ICheck[] { new CheckLyricReferenceLyric() },
            LyricEditorMode.Language => new ICheck[] { new CheckLyricLanguage() },
            LyricEditorMode.EditRuby => new ICheck[] { new CheckLyricRubyTag() },
            LyricEditorMode.EditRomaji => new ICheck[] { new CheckLyricRomajiTag() },
            LyricEditorMode.EditTimeTag => new ICheck[] { new CheckLyricTimeTag() },
            LyricEditorMode.EditNote => new ICheck[] { new CheckNoteReferenceLyric(), new CheckNoteText(), new CheckNoteTime() },
            LyricEditorMode.Singer => Array.Empty<ICheck>(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };

    public IBindableList<Issue> GetBindable(KaraokeHitObject hitObject)
        => hitObjectIssues[hitObject];

    public override void Refresh()
        => recalculateIssues();

    public void RefreshByHitObject(KaraokeHitObject hitObject)
    {
        if (hitObjectIssues.ContainsKey(hitObject))
        {
            hitObjectUpdated(hitObject);
        }
        else
        {
            hitObjectAdded(hitObject);
        }
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        // need to check is there any lyric added or removed.
        beatmap.HitObjectAdded += hitObjectAdded;
        beatmap.HitObjectRemoved += hitObjectRemoved;
        beatmap.HitObjectUpdated += hitObjectUpdated;

        recalculateIssues();
    }

    [BackgroundDependencyLoader]
    private void load(KaraokeRulesetEditCheckerConfigManager? rulesetEditCheckerConfigManager)
    {
        // todo: adjust the config in the config.
    }

    private void recalculateIssues()
    {
        var hitObjects = beatmap.HitObjects.OfType<KaraokeHitObject>().ToArray();
        var listedHitObjects = hitObjectIssues.Keys.ToArray();

        var newHitObjects = hitObjects.Except(listedHitObjects);
        var removeHitObjects = listedHitObjects.Except(hitObjects);
        var updateHitObjects = hitObjects.Intersect(listedHitObjects);

        foreach (var hitObject in newHitObjects)
        {
            hitObjectAdded(hitObject);
        }

        foreach (var hitObject in removeHitObjects)
        {
            hitObjectRemoved(hitObject);
        }

        foreach (var hitObject in updateHitObjects)
        {
            hitObjectUpdated(hitObject);
        }

        recalculateEditModeIssue();
    }

    private void hitObjectAdded(HitObject obj)
    {
        if (obj is not KaraokeHitObject karaokeHitObject)
            return;

        hitObjectIssues.Add(karaokeHitObject, new BindableList<Issue>());
        hitObjectUpdated(obj);

        editModeCache.Invalidate();
    }

    private void hitObjectRemoved(HitObject obj)
    {
        if (obj is not KaraokeHitObject karaokeHitObject)
            return;

        hitObjectIssues[karaokeHitObject].Clear();
        hitObjectIssues.Remove(karaokeHitObject);

        editModeCache.Invalidate();
    }

    private void hitObjectUpdated(HitObject obj)
    {
        if (obj is not KaraokeHitObject karaokeHitObject)
            return;

        var bindableIssues = hitObjectIssues[karaokeHitObject];

        var issues = getIssueByHitObject(karaokeHitObject).ToArray();
        bindableIssues.Clear();
        bindableIssues.AddRange(issues);

        editModeCache.Invalidate();
    }

    protected override void Update()
    {
        base.Update();

        if (!editModeCache.IsValid)
            recalculateEditModeIssue();
    }

    private void recalculateEditModeIssue()
    {
        var allIssues = hitObjectIssues.Values.SelectMany(x => x);
        var groupByEditModeIssues = allIssues.GroupBy(ClassifyIssue).ToDictionary(x => x.Key, x => x.ToArray());

        foreach (var editorMode in Enum.GetValues<LyricEditorMode>())
        {
            ClearChecks(editorMode);

            if (groupByEditModeIssues.TryGetValue(editorMode, out var issues))
                AddChecks(editorMode, issues);
        }

        editModeCache.Validate();
    }

    protected override BeatmapVerifierContext CreateBeatmapVerifierContext(IBeatmap beatmap, WorkingBeatmap workingBeatmap) => new(new Beatmap<HitObject>(), workingBeatmap);

    private IEnumerable<Issue> getIssueByHitObject(KaraokeHitObject karaokeHitObject)
    {
        return CreateIssues(context =>
        {
            if (context.Beatmap is not Beatmap<HitObject> beatmap)
                throw new InvalidCastException();

            beatmap.HitObjects.Add(karaokeHitObject);
        });
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);

        beatmap.HitObjectAdded -= hitObjectAdded;
        beatmap.HitObjectRemoved -= hitObjectRemoved;
        beatmap.HitObjectUpdated -= hitObjectUpdated;
    }
}
