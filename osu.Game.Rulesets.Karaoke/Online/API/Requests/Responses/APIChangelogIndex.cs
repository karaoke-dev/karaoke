// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

public class APIChangelogIndex
{
    /// <summary>
    /// All available builds with no content.
    /// </summary>
    public List<APIChangelogBuild> Builds { get; set; } = new();

    /// <summary>
    /// All preview builds display in the main page.
    /// </summary>
    public List<APIChangelogBuild> PreviewBuilds { get; set; } = new();

    /// <summary>
    /// All available years that will be shown in the sidebar.
    /// </summary>
    public int[] Years { get; set; } = Array.Empty<int>();
}
