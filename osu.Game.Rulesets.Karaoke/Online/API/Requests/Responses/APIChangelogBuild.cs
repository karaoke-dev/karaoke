// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Text.RegularExpressions;

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

public class APIChangelogBuild
{
    /// <summary>
    /// The URL of the loaded document.
    /// </summary>
    public string DocumentUrl { get; set; } = null!;

    /// <summary>
    /// The base URL for all root-relative links.
    /// </summary>
    public string RootUrl { get; set; } = null!;

    /// <summary>
    /// Version number
    /// </summary>
    /// <example>2023.0123</example>
    /// <example>2023.1111</example>
    public string Version { get; set; } = null!;

    /// <summary>
    /// Display version
    /// </summary>
    public string DisplayVersion => Version;

    /// <summary>
    /// Might be preview or detail markdown content.
    /// And the content is markdown format.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Version
    /// </summary>
    public VersionNavigation Versions { get; } = new();

    /// <summary>
    /// Created date.
    /// </summary>
    public DateTimeOffset PublishedAt { get; set; }

    public class VersionNavigation
    {
        /// <summary>
        /// Next version
        /// </summary>
        public APIChangelogBuild? Next { get; set; }

        /// <summary>
        /// Previous version
        /// </summary>
        public APIChangelogBuild? Previous { get; set; }
    }

    public override string ToString() => $"Karaoke! {DisplayVersion}";

    public APIChangelogBuild CreateBuildWithContent(string content)
    {
        return new APIChangelogBuild
        {
            DocumentUrl = DocumentUrl,
            RootUrl = RootUrl,
            Version = Version,
            Content = content,
            Versions =
            {
                Previous = Versions.Previous,
                Next = Versions.Next,
            },
            PublishedAt = PublishedAt,
        };
    }

    public string? GetFormattedContent()
    {
        if (Content == null)
            return null;

        // for able to parsing the badge, need to replace the " [content] " with " [content](content) ";
        const string pattern = @"(?<=\s)\[(.*?)\](?=\s)";
        return Regex.Replace(Content, pattern, m => $"[{m.Groups[1].Value}]({m.Groups[1].Value})");
    }
}
