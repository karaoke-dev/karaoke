// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.RubyTags;

public abstract class RubyTagGenerator<TConfig> : LyricPropertyGenerator<RubyTag[], TConfig>
    where TConfig : RubyTagGeneratorConfig, new()
{
    protected RubyTagGenerator(TConfig config)
        : base(config)
    {
    }

    protected override LocalisableString? GetInvalidMessageFromItem(Lyric item)
    {
        if (string.IsNullOrWhiteSpace(item.Text))
            return "Lyric should not be empty.";

        return null;
    }
}
