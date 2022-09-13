// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Caching;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Utils;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers
{
    public abstract class HitObjectChangeHandler<THitObject> : Component where THitObject : HitObject
    {
        private readonly Cached changingCache = new();

        [Resolved, AllowNull]
        private EditorBeatmap beatmap { get; set; }

        protected IEnumerable<THitObject> HitObjects => beatmap.HitObjects.OfType<THitObject>();

        protected HitObjectChangeHandler()
        {
            changingCache.Validate();
        }

        protected void CheckExactlySelectedOneHitObject()
        {
            if (beatmap.SelectedHitObjects.OfType<THitObject>().Count() != 1)
                throw new InvalidOperationException($"Should be exactly one {nameof(THitObject)} being selected.");
        }

        protected virtual void PerformOnSelection(Action<THitObject> action)
            => PerformOnSelection<THitObject>(action);

        protected void PerformOnSelection<T>(Action<T> action) where T : HitObject
        {
            if (!changingCache.IsValid)
                throw new NotSupportedException("Cannot trigger the change while applying another change.");

            if (beatmap.SelectedHitObjects.Count == 0)
                throw new NotSupportedException($"Should contain at least one selected {nameof(THitObject)}");

            changingCache.Invalidate();

            try
            {
                beatmap.PerformOnSelection(h =>
                {
                    if (h is T tHitObject)
                        action.Invoke(tHitObject);
                });
            }
            finally
            {
                changingCache.Validate();
            }
        }

        protected void AddRange<T>(IEnumerable<T> hitObjects) where T : HitObject => hitObjects.ForEach(Add);

        protected virtual void Add<T>(T hitObject) where T : HitObject
        {
            bool containsInBeatmap = HitObjects.Any(x => x == hitObject);
            if (containsInBeatmap)
                throw new InvalidOperationException("Seems this hit object is already in the beatmap.");

            if (isCreateObjectLocked(hitObject))
                throw new AddOrRemoveForbiddenException();

            beatmap.Add(hitObject);
        }

        protected virtual void Insert<T>(int index, T hitObject) where T : HitObject
        {
            bool containsInBeatmap = HitObjects.Any(x => x == hitObject);
            if (containsInBeatmap)
                throw new InvalidOperationException("Seems this hit object is already in the beatmap.");

            if (isCreateObjectLocked(hitObject))
                throw new AddOrRemoveForbiddenException();

            beatmap.Insert(index, hitObject);
        }

        protected void RemoveRange<T>(IEnumerable<T> hitObjects) where T : HitObject => hitObjects.ForEach(Remove);

        protected void Remove<T>(T hitObject) where T : HitObject
        {
            if (isRemoveObjectLocked(hitObject))
                throw new AddOrRemoveForbiddenException();

            beatmap.Remove(hitObject);
        }

        private bool isCreateObjectLocked<T>(T hitObject)
        {
            return hitObject switch
            {
                Lyric lyric => HitObjectWritableUtils.IsRemoveLyricLocked(lyric),
                Note note => note.ReferenceLyric != null && HitObjectWritableUtils.IsCreateOrRemoveNoteLocked(note.ReferenceLyric),
                _ => throw new InvalidCastException()
            };
        }

        private bool isRemoveObjectLocked<T>(T hitObject)
        {
            return hitObject switch
            {
                Lyric lyric => HitObjectWritableUtils.IsRemoveLyricLocked(lyric),
                Note note => note.ReferenceLyric != null && HitObjectWritableUtils.IsCreateOrRemoveNoteLocked(note.ReferenceLyric),
                _ => throw new InvalidCastException()
            };
        }

        public class AddOrRemoveForbiddenException : Exception
        {
            public AddOrRemoveForbiddenException()
                : base("Should not add or remove the hit-object.")
            {
            }
        }
    }
}
