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

        [Resolved]
        private IEditorChangeHandler? changeHandler { get; set; }

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
                // todo: follow-up the discussion in the https://github.com/karaoke-dev/karaoke/pull/1669 after support the change handler for customized ruleset.
                if (changeHandler is TransactionalCommitComponent transactionalCommitComponent && !transactionalCommitComponent.TransactionActive)
                {
                    // should trigger the UpdateState() in the editor beatmap only if there's no active state.
                    beatmap.PerformOnSelection(h =>
                    {
                        if (h is T tHitObject)
                            action(tHitObject);
                    });
                }
                else
                {
                    // Just update the object property if already in the changing state.
                    // e.g. dragging.
                    beatmap.SelectedHitObjects.ForEach(h =>
                    {
                        if (h is T tHitObject)
                            action(tHitObject);
                    });
                }
            }
            catch
            {
                // We should make sure that editor beatmap will end the change if still changing.
                // will goes to here if have exception in the change handler.
                if (beatmap.TransactionActive)
                    beatmap.EndChange();

                throw;
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
                Lyric => false,
                Note note => note.ReferenceLyric != null && HitObjectWritableUtils.IsCreateOrRemoveNoteLocked(note.ReferenceLyric),
                _ => throw new InvalidCastException()
            };
        }

        private bool isRemoveObjectLocked<T>(T hitObject)
        {
            switch (hitObject)
            {
                case Lyric lyric:
                    bool hasReferenceLyric = EditorBeatmapUtils.GetAllReferenceLyrics(beatmap, lyric).Any();
                    return hasReferenceLyric || HitObjectWritableUtils.IsRemoveLyricLocked(lyric);

                case Note note:
                    return note.ReferenceLyric != null && HitObjectWritableUtils.IsCreateOrRemoveNoteLocked(note.ReferenceLyric);

                default:
                    throw new InvalidCastException();
            }
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
