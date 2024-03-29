// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Threading.Tasks;
using Octokit;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests;

public class GetChangelogBuildRequest : GithubAPIRequest<APIChangelogBuild>
{
    private readonly APIChangelogBuild originBuild;

    public GetChangelogBuildRequest(APIChangelogBuild originBuild)
        : base(ChangelogRequestUtils.ORGANIZATION_NAME)
    {
        this.originBuild = originBuild;
    }

    protected override async Task<APIChangelogBuild> Perform(IGitHubClient client)
    {
        string contentString = await ChangelogRequestUtils.GetChangelogContent(client, originBuild.Version).ConfigureAwait(false);
        return originBuild.CreateBuildWithContent(contentString);
    }
}
