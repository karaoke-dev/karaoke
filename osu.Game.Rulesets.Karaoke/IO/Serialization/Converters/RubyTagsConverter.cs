// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class RubyTagsConverter : SortableJsonConvertor<RubyTag>
    {
        protected override IList<RubyTag> GetSortedValue(IEnumerable<RubyTag> objects)
            => TextTagsUtils.Sort(objects);
    }
}
