// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Helper;

public class TestCaseCheckHelper
{
    public static IEnumerable<ICheck> GetAllAvailableChecks()
    {
        var beatmapVerifier = new KaraokeBeatmapVerifier();
        var field = typeof(KaraokeBeatmapVerifier).GetField("checks", BindingFlags.Instance | BindingFlags.NonPublic);
        if (field == null)
            throw new ArgumentNullException(nameof(field));

        return (List<ICheck>)field.GetValue(beatmapVerifier)!;
    }

    public static IEnumerable<IssueTemplate> GetAllAvailableIssueTemplates()
    {
        return GetAllAvailableChecks().SelectMany(x => x.PossibleTemplates);
    }
}
