// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.ComponentModel;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;

namespace osu.Game.Rulesets.Karaoke.Skinning.Groups
{
    public abstract class BaseGroup<THitObject> : IGroup where THitObject : KaraokeHitObject
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool InTheGroup(KaraokeHitObject hitObject, ElementType elementType)
        {
            bool accepted = isTypeAccepted(hitObject, elementType);
            return accepted && InTheGroup(hitObject as THitObject);
        }

        protected abstract bool InTheGroup(THitObject hitObject);

        private static bool isTypeAccepted(KaraokeHitObject hitObject, ElementType elementType)
        {
            switch (elementType)
            {
                case ElementType.LyricConfig:
                case ElementType.LyricLayout:
                case ElementType.LyricStyle:
                    return hitObject is Lyric;

                case ElementType.NoteStyle:
                    return hitObject is Note;

                default:
                    throw new InvalidEnumArgumentException(nameof(elementType));
            }
        }
    }
}
