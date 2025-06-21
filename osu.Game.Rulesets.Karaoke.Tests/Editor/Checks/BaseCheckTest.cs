// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Checks.Components;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Checks;

public abstract class BaseCheckTest<TCheck> where TCheck : class, ICheck, new()
{
    private TCheck check = null!;

    [SetUp]
    public void Setup()
    {
        check = new TCheck();

        // check template in the list should not be duplicated.
        var possibleTemplates = check.PossibleTemplates;
        Assert.That(possibleTemplates.Count(), Is.EqualTo(possibleTemplates.Select(x => x.GetType()).Distinct().Count()));
    }

    protected void AssertOk(BeatmapVerifierContext context)
    {
        Assert.That(Run(context), Is.Empty);
    }

    protected void AssertNotOk<TIssueTemplate>(BeatmapVerifierContext context)
        where TIssueTemplate : IssueTemplate
    {
        AssertNotOk<Issue, TIssueTemplate>(context);
    }

    protected void AssertNotOk<TIssue, TIssueTemplate>(BeatmapVerifierContext context)
        where TIssue : Issue
        where TIssueTemplate : IssueTemplate
    {
        var issues = Run(context).ToList();

        // should make sure that only has one issue.
        Assert.That(issues.Single().GetType(), Is.EqualTo(typeof(TIssue)));
        Assert.That(issues.Single().Template.GetType(), Is.EqualTo(typeof(TIssueTemplate)));

        // should make sure that issue template is in the list。
        Assert.That(check.PossibleTemplates.OfType<TIssueTemplate>().Single(), Is.Not.Null);
    }

    protected IEnumerable<Issue> Run(BeatmapVerifierContext context)
    {
        return check.Run(context);
    }
}
