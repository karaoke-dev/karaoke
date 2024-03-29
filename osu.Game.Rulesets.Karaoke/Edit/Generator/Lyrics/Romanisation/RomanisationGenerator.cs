// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanisation;

public abstract class RomanisationGenerator<TConfig> : LyricPropertyGenerator<IReadOnlyDictionary<TimeTag, RomanisationGenerateResult>, TConfig>
    where TConfig : RomanisationGeneratorConfig, new()
{
    protected RomanisationGenerator(TConfig config)
        : base(config)
    {
    }

    protected override LocalisableString? GetInvalidMessageFromItem(Lyric item)
    {
        if (string.IsNullOrWhiteSpace(item.Text))
            return "Lyric should not be empty.";

        if (item.TimeTags.FirstOrDefault()?.Index != new TextIndex())
            return "Should have at least one index and that index should at the start of the lyric.";

        return null;
    }
}
