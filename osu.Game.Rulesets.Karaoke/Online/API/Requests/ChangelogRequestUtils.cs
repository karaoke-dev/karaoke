// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Octokit;
using osu.Game.Rulesets.Karaoke.Extensions;

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests;

public static class ChangelogRequestUtils
{
    public const string ORGANIZATION_NAME = "karaoke-dev";

    private const string project_name = $"{ORGANIZATION_NAME}.github.io";
    private const string branch_name = "master";
    private const string changelog_path = "content/changelog";

    public static Task<IReadOnlyList<RepositoryContent>> GetAllChangelogs(IGitHubClient client)
    {
        return client
               .Repository
               .Content
               .GetAllContents(ORGANIZATION_NAME, project_name, changelog_path);
    }

    public static string GetDocumentUrl(RepositoryContent content)
        => $"https://raw.githubusercontent.com/{ORGANIZATION_NAME}/{project_name}/{branch_name}/{content.Path}/";

    public static string GetRootUrl(RepositoryContent content)
        => content.HtmlUrl;

    public static string GetVersion(RepositoryContent content)
        => content.Name;

    public static DateTimeOffset GetPublishDateFromName(RepositoryContent content)
    {
        string? name = content.Name;
        var regex = new Regex("(?<year>[-0-9]+).(?<month>[-0-9]{2})(?<day>[-0-9]{2})");
        var result = regex.Match(name);
        if (!result.Success)
            return DateTimeOffset.MaxValue;

        int year = result.GetGroupValue<int>("year");
        int month = result.GetGroupValue<int>("month");
        int day = result.GetGroupValue<int>("day");

        return new DateTimeOffset(new DateTime(year, month, day));
    }

    public static async Task<string> GetChangelogContent(IGitHubClient client, string version)
    {
        string changeLogPath = $"{changelog_path}/{version}/index.md";
        byte[]? content = await client
                                .Repository
                                .Content
                                .GetRawContent(ORGANIZATION_NAME, project_name, changeLogPath)
                                .ConfigureAwait(false);

        // convert the content to a string
        return System.Text.Encoding.UTF8.GetString(content);
    }
}
