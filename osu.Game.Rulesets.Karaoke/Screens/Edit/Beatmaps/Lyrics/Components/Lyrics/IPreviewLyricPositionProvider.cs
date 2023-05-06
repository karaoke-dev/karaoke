// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Beatmaps.Lyrics.Components.Lyrics;

public interface IPreviewLyricPositionProvider
{
    int? GetCharIndexByPosition(float position);

    RectangleF GetRectByCharIndex(int charIndex);

    int GetCharIndicatorByPosition(float position);

    RectangleF GetRectByCharIndicator(int charIndex);

    RectangleF GetTextTagByPosition(ITextTag textTag);

    TimeTag? GetTimeTagByPosition(float position);

    Vector2 GetPositionByTimeTag(TimeTag timeTag);

    // todo: will remove this method eventually.
    TextIndex GetHoverIndex(float position);

    // todo: will remove this method eventually.
    Vector2 GetTextIndexPosition(TextIndex index);
}
