// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.Romanization.Ja;

public class JaRomanizationGeneratorConfig : RomanizationGeneratorConfig
{
    [ConfigSource("Uppercase", "Export romanization with uppercase.")]
    public Bindable<bool> Uppercase { get; } = new BindableBool();
}
