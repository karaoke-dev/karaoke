// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Primitives;
using osu.Game.Rulesets.Karaoke.Objects;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Content.Components.Lyrics;

public interface IPreviewLyricPositionProvider
{
    int? GetCharIndexByPosition(float position);

    RectangleF GetRectByCharIndex(int charIndex);

    int GetCharIndicatorByPosition(float position);

    RectangleF GetRectByCharIndicator(int gapIndex);

    RectangleF? GetRubyTagByPosition(RubyTag rubyTag);

    TimeTag? GetTimeTagByPosition(float position);

    Vector2 GetPositionByTimeTag(TimeTag timeTag);
}
