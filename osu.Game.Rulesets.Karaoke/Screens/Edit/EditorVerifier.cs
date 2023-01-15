// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

/// <summary>
/// This class is focus on mange the list of <see cref="ICheck"/> and save/load list of <see cref="Issue"/>.
/// </summary>
/// <typeparam name="TEnum"></typeparam>
public abstract partial class EditorVerifier<TEnum> : Component where TEnum : struct, Enum
{
    [Resolved, AllowNull]
    private EditorBeatmap beatmap { get; set; }

    [Resolved, AllowNull]
    private IBindable<WorkingBeatmap> workingBeatmap { get; set; }

    private readonly IDictionary<TEnum, ICheck[]> checkMappings = new Dictionary<TEnum, ICheck[]>();
    private readonly IDictionary<TEnum, BindableList<Issue>> issues = new Dictionary<TEnum, BindableList<Issue>>();

    protected EditorVerifier()
    {
        initializeCheckMappings();
        initializeIssues();
    }

    private void initializeCheckMappings()
    {
        foreach (var mode in Enum.GetValues<TEnum>())
        {
            checkMappings.Add(mode, CreateChecks(mode).ToArray());
        }
    }

    private void initializeIssues()
    {
        foreach (var mode in Enum.GetValues<TEnum>())
        {
            issues.Add(mode, new BindableList<Issue>());
        }
    }

    #region Checks

    protected abstract IEnumerable<ICheck> CreateChecks(TEnum type);

    protected IBindableList<Issue> GetIssueByType(TEnum type)
        => issues[type];

    protected void ClearChecks(TEnum type)
    {
        issues[type].Clear();
    }

    protected void AddChecks(TEnum type, IEnumerable<Issue> newIssues)
    {
        issues[type].AddRange(newIssues);
    }

    protected virtual TEnum ClassifyCheck(ICheck check)
    {
        foreach (var (type, checks) in checkMappings)
        {
            if (checks.Contains(check))
                return type;
        }

        throw new ArgumentOutOfRangeException();
    }

    protected virtual BeatmapVerifierContext CreateBeatmapVerifierContext(IBeatmap beatmap, WorkingBeatmap workingBeatmap) => new(beatmap, workingBeatmap);

    protected IEnumerable<Issue> CreateIssues(Action<BeatmapVerifierContext>? action = null)
    {
        var context = CreateBeatmapVerifierContext(beatmap, workingBeatmap.Value);
        action?.Invoke(context);
        return new EditorBeatmapVerifier(checkMappings.Values.SelectMany(x => x)).Run(context);
    }

    protected IEnumerable<Issue> CreateIssuesByType(TEnum type, BeatmapVerifierContext context)
        => new EditorBeatmapVerifier(checkMappings[type]).Run(context);

    #endregion

    private class EditorBeatmapVerifier : IBeatmapVerifier
    {
        private readonly IEnumerable<ICheck> checks;

        public EditorBeatmapVerifier(IEnumerable<ICheck> checks)
        {
            this.checks = checks;
        }

        public IEnumerable<Issue> Run(BeatmapVerifierContext context)
        {
            return checks.SelectMany(check => check.Run(context));
        }
    }
}
