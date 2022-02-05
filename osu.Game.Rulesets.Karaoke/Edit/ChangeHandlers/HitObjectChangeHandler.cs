// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers
{
    public abstract class HitObjectChangeHandler<THitObject> : Component where THitObject : HitObject
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        protected IEnumerable<THitObject> HitObjects => beatmap.HitObjects.OfType<THitObject>();

        protected void PerformOnSelection(Action<THitObject> action) => beatmap.PerformOnSelection(h =>
        {
            if (h is THitObject tHitObject)
                action.Invoke(tHitObject);
        });

        protected void AddRange(IEnumerable<HitObject> hitObjects) => beatmap.AddRange(hitObjects);

        protected virtual void Add(THitObject hitObject) => beatmap.Add(hitObject);

        public void Insert(int index, THitObject hitObject) => beatmap.Insert(index, hitObject);

        protected void Remove(THitObject hitObject) => beatmap.Remove(hitObject);

        protected void RemoveRange(IEnumerable<HitObject> hitObjects) => beatmap.RemoveRange(hitObjects);
    }
}
