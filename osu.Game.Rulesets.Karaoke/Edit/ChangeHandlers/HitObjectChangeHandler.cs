// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Caching;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers
{
    public abstract class HitObjectChangeHandler<THitObject> : Component where THitObject : HitObject
    {
        private readonly Cached changingCache = new();

        [Resolved]
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

        protected void PerformOnSelection(Action<THitObject> action)
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
                    if (h is THitObject tHitObject)
                        action.Invoke(tHitObject);
                });
            }
            finally
            {
                changingCache.Validate();
            }
        }

        protected void AddRange(IEnumerable<HitObject> hitObjects) => beatmap.AddRange(hitObjects);

        protected virtual void Add(THitObject hitObject) => beatmap.Add(hitObject);

        protected void Insert(int index, THitObject hitObject) => beatmap.Insert(index, hitObject);

        protected void Remove(THitObject hitObject) => beatmap.Remove(hitObject);

        protected void RemoveRange(IEnumerable<HitObject> hitObjects) => beatmap.RemoveRange(hitObjects);
    }
}
