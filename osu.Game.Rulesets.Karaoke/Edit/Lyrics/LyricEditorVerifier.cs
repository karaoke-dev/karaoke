// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Caching;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Configuration;
using osu.Game.Rulesets.Karaoke.Edit.Checks;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics;

public class LyricEditorVerifier : Component, ILyricEditorVerifier
{
    [Resolved, AllowNull]
    private EditorBeatmap beatmap { get; set; }

    [Resolved, AllowNull]
    private IBindable<WorkingBeatmap> workingBeatmap { get; set; }

    private readonly Dictionary<KaraokeHitObject, BindableList<Issue>> hitObjectIssues = new();

    private readonly IDictionary<LyricEditorMode, BindableList<Issue>> editModeIssues = new Dictionary<LyricEditorMode, BindableList<Issue>>
    {
        { LyricEditorMode.Texting, new BindableList<Issue>() },
        { LyricEditorMode.Reference, new BindableList<Issue>() },
        { LyricEditorMode.Language, new BindableList<Issue>() },
        { LyricEditorMode.EditRuby, new BindableList<Issue>() },
        { LyricEditorMode.EditRomaji, new BindableList<Issue>() },
        { LyricEditorMode.EditTimeTag, new BindableList<Issue>() },
        { LyricEditorMode.EditNote, new BindableList<Issue>() },
    };

    private readonly Cached editModeCache = new();
    private readonly LyricEditorBeatmapVerifier verifier = new();

    public IBindableList<Issue> GetBindable(KaraokeHitObject hitObject)
        => hitObjectIssues[hitObject];

    public IBindableList<Issue> GetIssueByEditMode(LyricEditorMode editorMode)
        => editModeIssues[editorMode];

    public void Refresh()
        => recalculateIssues();

    protected override void LoadComplete()
    {
        base.LoadComplete();

        // need to check is there any lyric added or removed.
        beatmap.HitObjectAdded += hitObjectAdded;
        beatmap.HitObjectRemoved += hitObjectRemoved;
        beatmap.HitObjectUpdated += hitObjectUpdated;

        recalculateIssues();
    }

    [BackgroundDependencyLoader(true)]
    private void load(KaraokeRulesetEditCheckerConfigManager? rulesetEditCheckerConfigManager)
    {
        // todo: adjust the config in the config.
    }

    private void recalculateIssues()
    {
        hitObjectIssues.Clear();

        var hitObjects = beatmap.HitObjects.OfType<KaraokeHitObject>();

        foreach (var hitObject in hitObjects)
        {
            hitObjectAdded(hitObject);
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
        var groupByEditModeIssues = allIssues.GroupBy(x => getNavigateEditMode(x.Check)).ToDictionary(x => x.Key, x => x.ToArray());

        foreach (var (editorMode, editModeBindableList) in editModeIssues)
        {
            editModeBindableList.Clear();

            if (groupByEditModeIssues.TryGetValue(editorMode, out var issues))
                editModeBindableList.AddRange(issues);
        }

        editModeCache.Validate();
    }

    private IEnumerable<Issue> getIssueByHitObject(KaraokeHitObject karaokeHitObject)
    {
        var context = new BeatmapVerifierContext(new Beatmap<HitObject>
        {
            HitObjects =
            {
                karaokeHitObject
            }
        }, workingBeatmap.Value);
        return verifier.Run(context);
    }

    private static LyricEditorMode getNavigateEditMode(ICheck check)
    {
        switch (check)
        {
            case CheckLyricText:
                return LyricEditorMode.Texting;

            case CheckLyricReferenceLyric:
                return LyricEditorMode.Reference;

            case CheckLyricLanguage:
                return LyricEditorMode.Language;

            case CheckLyricRubyTag:
                return LyricEditorMode.EditRuby;

            case CheckLyricRomajiTag:
                return LyricEditorMode.EditRomaji;

            case CheckLyricTimeTag:
                return LyricEditorMode.EditTimeTag;

            case CheckNoteReferenceLyric:
            case CheckNoteText:
                return LyricEditorMode.EditNote;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);

        beatmap.HitObjectAdded -= hitObjectAdded;
        beatmap.HitObjectRemoved -= hitObjectRemoved;
        beatmap.HitObjectUpdated -= hitObjectUpdated;
    }

    private class LyricEditorBeatmapVerifier : IBeatmapVerifier
    {
        private readonly IDictionary<LyricEditorMode, ICheck[]> editModeChecks = new J2N.Collections.Generic.Dictionary<LyricEditorMode, ICheck[]>
        {
            { LyricEditorMode.Texting, new ICheck[] { new CheckLyricText() } },
            { LyricEditorMode.Reference, new ICheck[] { new CheckLyricReferenceLyric() } },
            { LyricEditorMode.Language, new ICheck[] { new CheckLyricLanguage() } },
            { LyricEditorMode.EditRuby, new ICheck[] { new CheckLyricRubyTag() } },
            { LyricEditorMode.EditRomaji, new ICheck[] { new CheckLyricRomajiTag() } },
            { LyricEditorMode.EditTimeTag, new ICheck[] { new CheckLyricTimeTag() } },
            { LyricEditorMode.EditNote, new ICheck[] { new CheckNoteReferenceLyric(), new CheckNoteText() } },
        };

        public IEnumerable<Issue> Run(BeatmapVerifierContext context)
        {
            var allChecks = editModeChecks.Values.SelectMany(x => x);
            return allChecks.SelectMany(check => check.Run(context));
        }
    }
}
