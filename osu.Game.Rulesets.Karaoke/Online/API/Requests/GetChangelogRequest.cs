// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests;

public class GetChangelogRequest : GithubAPIRequest<APIChangelogIndex>
{
    public GetChangelogRequest()
        : base(ChangelogRequestUtils.ORGANIZATION_NAME)
    {
    }

    protected override async Task<APIChangelogIndex> Perform(IGitHubClient client)
    {
        var builds = await getAllBuilds(client).ConfigureAwait(false);
        var previewBuilds = (await Task.WhenAll(builds.Take(5).Select(x => createPreviewBuild(client, x))).ConfigureAwait(false)).ToList();
        int[] years = generateYears(builds);

        return new APIChangelogIndex
        {
            Years = years,
            Builds = builds,
            PreviewBuilds = previewBuilds,
        };
    }

    private static async Task<List<APIChangelogBuild>> getAllBuilds(IGitHubClient client)
    {
        var reposAscending = await ChangelogRequestUtils.GetAllChangelogs(client).ConfigureAwait(false);

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
        return new APIChangelogBuild
        {
            DocumentUrl = ChangelogRequestUtils.GetDocumentUrl(content),
            RootUrl = ChangelogRequestUtils.GetRootUrl(content),
            Version = ChangelogRequestUtils.GetVersion(content),
            PublishedAt = ChangelogRequestUtils.GetPublishDateFromName(content),
        };
    }

    private static async Task<APIChangelogBuild> createPreviewBuild(IGitHubClient client, APIChangelogBuild originBuild)
    {
        string contentString = await ChangelogRequestUtils.GetChangelogContent(client, originBuild.Version).ConfigureAwait(false);
        return originBuild.CreateBuildWithContent(contentString);
    }

    private static int[] generateYears(IEnumerable<APIChangelogBuild> builds)
        => builds.Select(x => x.PublishedAt.Year).Distinct().ToArray();
}
