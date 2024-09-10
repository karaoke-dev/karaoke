// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Extensions.Color4Extensions;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog;

public class ChangelogBadgeInfo
{
    // follow the definition in the https://github.com/karaoke-dev/karaoke-dev.github.io/blob/master/layouts/partials/script/badge.html
    private static readonly IDictionary<string, string> colour_mappings = new Dictionary<string, string>
    {
        { "outdated", "#808080" },
        { "rejected", "#FF0000" },
    };

    public Color4 Color { get; init; } = Color4.White;

    public string Text { get; init; } = string.Empty;

    /// <summary>
    /// Trying to parse the badge from the text.
    /// </summary>
    /// <example>
    /// [outdated]<br/>
    /// [rejected]
    /// </example>
    /// <param name="text">Link text</param>
    /// <returns></returns>
    public static ChangelogBadgeInfo? GetBadgeInfoFromLink(string text)
    {
        if (!colour_mappings.TryGetValue(text, out string? repoUrl))
            return null;

        return new ChangelogBadgeInfo
        {
            Text = text,
            Color = Color4Extensions.FromHex(repoUrl),
        };
    }
}
