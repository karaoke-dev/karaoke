// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses
{
    public class APIChangelogBuild
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="organization">Account or organization name</param>
        /// <param name="project">Project name</param>
        /// <param name="branch">Branch name</param>
        public APIChangelogBuild(string organization, string project, string branch = "master")
        {
            OrganizationName = organization;
            ProjectName = project;
            Branch = branch;
            Versions = new VersionNavigation();
        }

        /// <summary>
        /// Organization name
        /// </summary>
        public string OrganizationName { get; }

        /// <summary>
        /// Project name
        /// </summary>
        public string ProjectName { get; }

        /// <summary>
        /// Branch name
        /// </summary>
        public string Branch { get; }

        /// <summary>
        /// The URL of the loaded document.
        /// </summary>
        public string DocumentUrl => $"https://raw.githubusercontent.com/{OrganizationName}/{ProjectName}/{Branch}/{Path}/";

        /// <summary>
        /// The base URL for all root-relative links.
        /// </summary>
        public string RootUrl { get; set; }

        /// <summary>
        /// Path of the project
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Path to download readme url
        /// </summary>
        public string ReadmeDownloadUrl => $"{DocumentUrl}index.md";

        /// <summary>
        /// Display version
        /// </summary>
        public string DisplayVersion { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public VersionNavigation Versions { get; }

        /// <summary>
        /// Created date.
        /// </summary>
        public DateTimeOffset PublishedAt { get; set; }

        public class VersionNavigation
        {
            /// <summary>
            /// Next version
            /// </summary>
            public APIChangelogBuild Next { get; set; }

            /// <summary>
            /// Previous version
            /// </summary>
            public APIChangelogBuild Previous { get; set; }
        }
    }
}
