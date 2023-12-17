// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Edit.Checks.Components;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;
using osu.Game.Rulesets.Karaoke.Edit;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Stages.Classic;
using osu.Game.Rulesets.Objects;

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

    public static IReadOnlyDictionary<ICheck, IEnumerable<IssueTemplate>> GetAllAvailableIssueTemplates()
    {
        return GetAllAvailableChecks().ToDictionary(k => k, v => v.PossibleTemplates);
    }

    public static IReadOnlyDictionary<ICheck, Issue[]> CreateAllAvailableIssues()
    {
        return GetAllAvailableIssueTemplates().ToDictionary(k => k.Key,
            v => v.Value.Select(createIssueByIssueTemplate).ToArray());

        Issue createIssueByIssueTemplate(IssueTemplate issueTemplate)
        {
            var method = issueTemplate.GetType().GetMethod("Create");
            if (method == null)
                throw new MissingMethodException("Test method is not exist.");

            object[] paramsInfo = method.GetParameters().Select(generateDefaultTypeByParameterInfo).ToArray();
            if (method.Invoke(issueTemplate, paramsInfo) is not Issue issue)
                throw new InvalidOperationException("Issue should not be null.");

            return issue;
        }

        object generateDefaultTypeByParameterInfo(ParameterInfo parameterInfos) =>
            parameterInfos.ParameterType switch
            {
                // Metadata in the beatmap.
                Type t when ReferenceEquals(t, typeof(Page)) => new Page(),
                Type t when ReferenceEquals(t, typeof(ClassicLyricTimingPoint)) => new ClassicLyricTimingPoint(),
                Type t when ReferenceEquals(t, typeof(IEnumerable<HitObject>)) => new List<HitObject>(),
                // Hit-object.
                Type t when ReferenceEquals(t, typeof(Lyric)) => new Lyric(),
                Type t when ReferenceEquals(t, typeof(Note)) => new Note(),
                Type t when ReferenceEquals(t, typeof(RubyTag)) => new RubyTag(),
                Type t when ReferenceEquals(t, typeof(TimeTag)) => new TimeTag(new TextIndex(), 0),
                Type t when ReferenceEquals(t, typeof(RomajiTag)) => new RomajiTag(),
                // Other type.
                Type t when ReferenceEquals(t, typeof(int)) => 0,
                Type t when ReferenceEquals(t, typeof(CultureInfo)) => new CultureInfo("Ja-jp"),
                _ => throw new InvalidOperationException(),
            };
    }
}
