// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

public class APIChangelogIndex
{
    public List<APIChangelogBuild> Builds { get; set; } = new();

    public List<APIChangelogBuild> PreviewBuilds { get; set; } = new();

    public int[] Years { get; set; } = Array.Empty<int>();
}
