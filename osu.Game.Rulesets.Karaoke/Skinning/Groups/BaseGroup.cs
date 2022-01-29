// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;

namespace osu.Game.Rulesets.Karaoke.Skinning.Groups
{
    public abstract class BaseGroup<THitObject> : IGroup where THitObject : KaraokeHitObject
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public IEnumerable<KaraokeHitObject> GetGroupHitObjects(IBeatmap beatmap, IKaraokeSkinElement element)
        {
            // get processable hit objects type first.
            var acceptedHitObjects = filterAcceptedHitObjects(beatmap, element).OfType<THitObject>();

            // then get the objects by customized group.
            return acceptedHitObjects.Where(InTheGroup);
        }

        protected abstract bool InTheGroup(THitObject hitObject);

        private IEnumerable<KaraokeHitObject> filterAcceptedHitObjects(IBeatmap beatmap, IKaraokeSkinElement element)
        {
            var karaokeHitObjects = beatmap.HitObjects.OfType<KaraokeHitObject>();

            switch (element)
            {
                case LyricConfig:
                case LyricLayout:
                case LyricStyle:
                    return karaokeHitObjects.OfType<Lyric>();

                case NoteStyle:
                    return karaokeHitObjects.OfType<Note>();
            }

            return karaokeHitObjects;
        }
    }
}
