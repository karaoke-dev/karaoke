// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Threading.Tasks;
using Octokit;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests;

public abstract class GithubChangeLogAPIRequest<T> : GithubAPIRequest<T> where T : class
{
    protected const string ORGANIZATION_NAME = "karaoke-dev";
    protected const string PROJECT_NAME = $"{ORGANIZATION_NAME}.github.io";
    protected const string BRANCH_NAME = "master";
    protected const string CHANGELOG_PATH = "content/changelog";

    protected GithubChangeLogAPIRequest()
        : base(ORGANIZATION_NAME)
    {
    }

    protected static async Task<string> GetChangelogContent(IGitHubClient client, string version)
    {
        string changeLogPath = $"{CHANGELOG_PATH}/{version}/index.md";
        byte[]? content = await client
                                .Repository
                                .Content
                                .GetRawContent(ORGANIZATION_NAME, PROJECT_NAME, changeLogPath)
                                .ConfigureAwait(false);

        // convert the content to a string
        return System.Text.Encoding.UTF8.GetString(content);
    }

    protected static APIChangelogBuild CreateBuildWithContent(APIChangelogBuild originBuild, string content)
    {
        return new APIChangelogBuild
        {
            DocumentUrl = originBuild.DocumentUrl,
            RootUrl = originBuild.RootUrl,
            Version = originBuild.Version,
            Content = content,
            Versions =
            {
                Previous = originBuild.Versions.Previous,
                Next = originBuild.Versions.Next,
            },
            PublishedAt = originBuild.PublishedAt,
        };
    }
}
