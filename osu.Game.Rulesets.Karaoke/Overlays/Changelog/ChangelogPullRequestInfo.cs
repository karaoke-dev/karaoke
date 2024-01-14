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

    public PullRequestInfo[] PullRequests { get; set; } = Array.Empty<PullRequestInfo>();

    public UserInfo[] Users { get; set; } = Array.Empty<UserInfo>();

    public class PullRequestInfo
    {
        public int Number { get; set; }

        public string Url { get; set; } = string.Empty;
    }

    public class UserInfo
    {
        public string UserName { get; set; } = string.Empty;

        public string UserUrl => $"https://github.com/{UserName}";
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
    /// <param name="text">Link text</param>
    /// <param name="url">Link url</param>
    /// <returns></returns>
    public static ChangelogPullRequestInfo? GetPullRequestInfoFromLink(string text, string url)
    {
        if (!repo_urls.ContainsKey(text))
            return null;

        const string pull_request_key = "pull_request";
        const string username_key = "username";
        const string pull_request_regex = $"#(?<{pull_request_key}>[0-9]+)|@(?<{username_key}>[0-9A-z]+)";

        // note: should have at least one pr number or one username,
        var result = Regex.Matches(url, pull_request_regex, RegexOptions.IgnoreCase);
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
            PullRequests = prs.Select(x => generatePullRequestInfo(repo_urls[text], x)).ToArray(),
            Users = usernames.Select(generateUserInfo).ToArray(),
        };

        static PullRequestInfo generatePullRequestInfo(string url, string pr) =>
            new()
            {
                Number = int.Parse(pr),
                Url = new Uri(new Uri(url), $"pull/{pr}").AbsoluteUri,
            };

        static UserInfo generateUserInfo(string username) =>
            new()
            {
                UserName = username,
            };
    }
}
