// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Karaoke.Skinning.Metadatas;

namespace osu.Game.Rulesets.Karaoke.Skinning.Mappings
{
    public interface IStyleMappingRole
    {
        LyricStyle LyricStyle { get; set; }

        LyricStyle NoteStyle { get; set; }
    }
}
