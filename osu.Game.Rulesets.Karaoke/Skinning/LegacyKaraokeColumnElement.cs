// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.UI;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning
{
    public class LegacyKaraokeColumnElement : LegacyKaraokeElement
    {
        protected NotePlayfield NotePlayfield => Playfield.NotePlayfield;

        // TODO : get current index
        protected override IBindable<T> GetKaraokeSkinConfig<T>(ISkin skin, LegacyKaraokeSkinConfigurationLookups lookup, int? index = null)
            => base.GetKaraokeSkinConfig<T>(skin, lookup, index);
    }
}
