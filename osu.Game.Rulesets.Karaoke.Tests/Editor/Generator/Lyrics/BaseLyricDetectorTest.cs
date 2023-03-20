// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Edit.Generator;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Lyrics;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.Generator.Lyrics;

public abstract class BaseLyricDetectorTest<TDetector, TObject, TConfig>
    : BasePropertyDetectorTest<TDetector, Lyric, TObject, TConfig>
    where TDetector : LyricPropertyDetector<TObject, TConfig>
    where TConfig : GeneratorConfig, new()
{
}
