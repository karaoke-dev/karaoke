// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests;

public abstract class GithubChangeLogAPIRequest<T> : GithubAPIRequest<T> where T : class
{
    protected GithubChangeLogAPIRequest()
        : base(ChangelogRequestUtils.ORGANIZATION_NAME)
    {
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
