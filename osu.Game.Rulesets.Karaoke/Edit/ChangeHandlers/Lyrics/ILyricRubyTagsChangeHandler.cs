// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers.Lyrics;

public interface ILyricRubyTagsChangeHandler : ILyricListPropertyChangeHandler<RubyTag>
{
    void SetIndex(RubyTag rubyTag, int? startIndex, int? endIndex);

    void ShiftingIndex(IEnumerable<RubyTag> rubyTags, int offset);

    void SetText(RubyTag rubyTag, string text);
}
