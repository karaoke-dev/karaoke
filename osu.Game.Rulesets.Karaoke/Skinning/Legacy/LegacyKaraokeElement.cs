// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning.Legacy
{
    /// <summary>
    /// A karaoke legacy skin element.
    /// </summary>
    public class LegacyKaraokeElement : CompositeDrawable
    {
        [Resolved(CanBeNull = true)]
        [CanBeNull]
        protected KaraokePlayfield Playfield { get; private set; }

        /// <summary>
        /// Retrieve a per-column-count skin configuration.
        /// </summary>
        /// <param name="skin">The skin from which configuration is retrieved.</param>
        /// <param name="lookup">The value to retrieve.</param>
        /// <param name="index">If not null, denotes the index of the column to which the entry applies.</param>
        protected virtual IBindable<T> GetKaraokeSkinConfig<T>(ISkin skin, LegacyKaraokeSkinConfigurationLookups lookup, int? index = null)
            => skin.GetConfig<KaraokeSkinConfigurationLookup, T>(
                new KaraokeSkinConfigurationLookup(Playfield?.NotePlayfield.Columns ?? 4, lookup, index));
    }
}
