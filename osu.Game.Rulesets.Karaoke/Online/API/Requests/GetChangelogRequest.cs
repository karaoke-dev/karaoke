// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Octokit;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests;

public class GetChangelogRequest : GithubChangeLogAPIRequest<APIChangelogIndex>
{
    protected override async Task<APIChangelogIndex> Perform(IGitHubClient client)
    {
        var builds = await getAllBuilds(client).ConfigureAwait(false);
        int[] years = generateYears(builds);

        return new APIChangelogIndex
        {
            Years = years,
            Builds = builds,
        };
    }

    private static async Task<List<APIChangelogBuild>> getAllBuilds(IGitHubClient client)
    {
        var reposAscending = await client
                                   .Repository
                                   .Content
                                   .GetAllContents(ORGANIZATION_NAME, PROJECT_NAME, CHANGELOG_PATH)
                                   .ConfigureAwait(false);

        var builds = reposAscending
                     .Reverse()
                     .Where(x => x.Type == ContentType.Dir)
                     .Select(createBuild)
                     .ToList();

        // adjust the mapping of previous/next versions by hand.
        foreach (var build in builds)
        {
            build.Versions.Previous = builds.GetPrevious(build);
            build.Versions.Next = builds.GetNext(build);
        }

        return builds;
    }

    private static APIChangelogBuild createBuild(RepositoryContent content)
    {
        return new APIChangelogBuild(ORGANIZATION_NAME, PROJECT_NAME)
        {
            RootUrl = content.HtmlUrl,
            Path = content.Path,
            Version = content.Name,
            PublishedAt = getPublishDateFromName(content.Name),
        };

        static DateTimeOffset getPublishDateFromName(string name)
        {
            var regex = new Regex("(?<year>[-0-9]+).(?<month>[-0-9]{2})(?<day>[-0-9]{2})");
            var result = regex.Match(name);
            if (!result.Success)
                return DateTimeOffset.MaxValue;

            int year = result.GetGroupValue<int>("year");
            int month = result.GetGroupValue<int>("month");
            int day = result.GetGroupValue<int>("day");

            return new DateTimeOffset(new DateTime(year, month, day));
        }
    }

    private static int[] generateYears(IEnumerable<APIChangelogBuild> builds)
        => builds.Select(x => x.PublishedAt.Year).Distinct().ToArray();
}
