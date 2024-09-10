// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using osu.Game.Rulesets.Karaoke.Extensions;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog;

public class ChangelogPullRequestInfo
{
    private static readonly IDictionary<string, string> repo_urls = new Dictionary<string, string>
    {
        { "karaoke", "https://github.com/karaoke-dev/karaoke/" },
        { "edge", "https://github.com/karaoke-dev/karaoke/" },
        { "github.io", "https://github.com/karaoke-dev/karaoke-dev.github.io/" },
        { "launcher", "https://github.com/karaoke-dev/launcher/" },
        { "sample", "https://github.com/karaoke-dev/sample-beatmap/" },
        { "microphone-package", "https://github.com/karaoke-dev/osu-framework-microphone/" },
        { "font-package", "https://github.com/karaoke-dev/osu-framework-font/" },
    };

    public string RepoName { get; init; } = string.Empty;

    public PullRequestInfo[] PullRequests { get; init; } = Array.Empty<PullRequestInfo>();

    public UserInfo[] Users { get; init; } = Array.Empty<UserInfo>();

    public readonly struct PullRequestInfo
    {
        public PullRequestInfo(string repoName, int number)
        {
            Number = number;
            Url = new Uri(new Uri(repo_urls[repoName]), $"pull/{number}").AbsoluteUri;
        }

        public int Number { get; } = 0;

        public string Url { get; } = string.Empty;
    }

    public readonly struct UserInfo
    {
        public UserInfo(string useName)
        {
            UserName = useName;
        }

        public string UserName { get; } = string.Empty;

        public string Url => $"https://github.com/{UserName}";
    }

    /// <summary>
    /// Trying to parse the pull-request info from the url.
    /// </summary>
    /// <example>
    /// #2152@andy840119<br/>
    /// #2152<br/>
    /// #2152#2153<br/>
    /// @andy840119<br/>
    /// @andy@andy840119
    /// </example>
    /// <param name="repo">Link text</param>
    /// <param name="info">Link url</param>
    /// <returns></returns>
    public static ChangelogPullRequestInfo? GetPullRequestInfoFromLink(string repo, string info)
    {
        if (!repo_urls.ContainsKey(repo))
            return null;

        const string pull_request_key = "pull_request";
        const string username_key = "username";
        const string pull_request_regex = $"#(?<{pull_request_key}>[0-9]+)|@(?<{username_key}>[0-9A-z]+)";

        // note: should have at least one pr number or one username,
        var result = Regex.Matches(info, pull_request_regex, RegexOptions.IgnoreCase);
        if (!result.Any())
            return null;

        // get pull-request or username
        var prs = result.Select(x => x.GetGroupValue<string>(pull_request_key))
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Distinct();
        var usernames = result.Select(x => x.GetGroupValue<string>(username_key))
                              .Where(x => !string.IsNullOrEmpty(x))
                              .Distinct();

        return new ChangelogPullRequestInfo
        {
            RepoName = repo,
            PullRequests = prs.Select(pr => new PullRequestInfo(repo, int.Parse(pr))).ToArray(),
            Users = usernames.Select(userName => new UserInfo(userName)).ToArray(),
        };
    }
}
