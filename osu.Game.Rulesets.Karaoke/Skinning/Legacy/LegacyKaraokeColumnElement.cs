// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.UI.Scrolling;
using osu.Game.Skinning;

namespace osu.Game.Rulesets.Karaoke.Skinning.Legacy
{
    public class LegacyKaraokeColumnElement : LegacyKaraokeElement
    {
        protected ScrollingNotePlayfield NotePlayfield => Playfield?.NotePlayfield;

        // TODO : get current index
        protected override IBindable<T> GetKaraokeSkinConfig<T>(ISkin skin, LegacyKaraokeSkinConfigurationLookups lookup, int? index = null)
            => base.GetKaraokeSkinConfig<T>(skin, lookup, index);
    }
}
