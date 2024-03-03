// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanisation.Ja;

public class JaRomanisationGeneratorConfig : RomanisationGeneratorConfig
{
    [ConfigSource("Uppercase", "Export romanisation with uppercase.")]
    public Bindable<bool> Uppercase { get; } = new BindableBool();
}
