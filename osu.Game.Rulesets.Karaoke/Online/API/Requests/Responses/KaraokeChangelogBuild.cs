// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses
{
    public class KaraokeChangelogBuild
    {
        /// <summary>
        /// The URL of the loaded document.
        /// </summary>
        public string DocumentUrl => $"https://raw.githubusercontent.com/osu-Karaoke/osu-Karaoke.github.io/master/{Path}/";

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
        public string ReadmeDownloadUrl => $"https://raw.githubusercontent.com/osu-Karaoke/osu-Karaoke.github.io/master/{Path}/README.md";

        /// <summary>
        /// Display version
        /// </summary>
        public string DisplayVersion { get; set; }
    }
}
