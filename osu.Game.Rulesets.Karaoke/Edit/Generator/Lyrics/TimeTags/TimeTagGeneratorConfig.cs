// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics.TimeTags;

public abstract class TimeTagGeneratorConfig : GeneratorConfig
{
    protected const string CATEGORY_CHECK_CHARACTER = "Character checking";
    protected const string CATEGORY_CHECK_LINE_END = "Line end checking";
    protected const string CATEGORY_CHECK_WHITE_SPACE = "White space checking";

    /// <summary>
    /// Will create a <see cref="TimeTag"/> at the first of the lyric if only contains spacing in the <see cref="Lyric"/>.
    /// </summary>
    [ConfigCategory(CATEGORY_CHECK_CHARACTER)]
    [ConfigSource("Check blank line", "Check blank line or not.")]
    public Bindable<bool> CheckBlankLine { get; } = new BindableBool();

    /// <summary>
    /// Add end <see cref="TimeTag"/> at the end of the <see cref="Lyric"/>.
    /// </summary>
    [ConfigCategory(CATEGORY_CHECK_LINE_END)]
    [ConfigSource("Use key-up time tag in line end", "Use key-up time tag in line end")]
    public Bindable<bool> CheckLineEndKeyUp { get; } = new BindableBool();

    /// <summary>
    /// Will add the <see cref="TimeTag"/> if meet the spacing.
    /// </summary>
    [ConfigCategory(CATEGORY_CHECK_WHITE_SPACE)]
    [ConfigSource("Check white space", "Check white space")]
    public Bindable<bool> CheckWhiteSpace { get; } = new BindableBool();

    /// <summary>
    /// Add the end <see cref="TimeTag"/> instead.
    /// This feature will work only if enable the <see cref="CheckWhiteSpace"/>.
    /// </summary>
    [ConfigCategory(CATEGORY_CHECK_WHITE_SPACE)]
    [ConfigSource("Use key-up", "Use key-up")]
    public Bindable<bool> CheckWhiteSpaceKeyUp { get; } = new BindableBool();
}
