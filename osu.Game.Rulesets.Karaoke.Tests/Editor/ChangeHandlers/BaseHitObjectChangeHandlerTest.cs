// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers
{
    public abstract class BaseHitObjectChangeHandlerTest<TChangeHandler, THitObject> : BaseChangeHandlerTest<TChangeHandler>
        where TChangeHandler : HitObjectChangeHandler<THitObject>, new() where THitObject : HitObject
    {
        private EditorBeatmap editorBeatmap;

        [BackgroundDependencyLoader]
        private void load()
        {
            editorBeatmap = Dependencies.Get<EditorBeatmap>();
        }

        [SetUp]
        public virtual void SetUp()
        {
            AddStep("Setup", () =>
            {
                editorBeatmap.Clear();
                editorBeatmap.SelectedHitObjects.Clear();
            });
        }

        protected void PrepareHitObject(HitObject hitObject, bool selected = true)
            => PrepareHitObjects(new[] { hitObject }, selected);

        protected void PrepareHitObjects(IEnumerable<HitObject> selectedHitObjects, bool selected = true)
        {
            AddStep("Prepare testing hit objects", () =>
            {
                var hitobjects = selectedHitObjects.ToList();
                editorBeatmap.AddRange(hitobjects);

                if (selected)
                {
                    editorBeatmap.SelectedHitObjects.AddRange(hitobjects);
                }
            });
        }

        protected void AssertHitObject(Action<THitObject> assert)
        {
            AddStep("Is result matched", () =>
            {
                foreach (var hitObject in editorBeatmap.HitObjects.OfType<THitObject>())
                {
                    assert(hitObject);
                }
            });

            // even if there's no property changed in the lyric editor, should still trigger the change handler.
            // because every change handler call should cause one undo step.
            // also, technically should not call the change handler if there's no possible to change the properties.
            AssertTransactionOnlyTriggerOnce();
        }

        protected void AssertHitObjects(Action<IEnumerable<THitObject>> assert)
        {
            AddStep("Is result matched", () =>
            {
                assert(editorBeatmap.HitObjects.OfType<THitObject>());
            });

            // even if there's no property changed in the lyric editor, should still trigger the change handler.
            // because every change handler call should cause one undo step.
            // also, technically should not call the change handler if there's no possible to change the properties.
            AssertTransactionOnlyTriggerOnce();
        }

        protected void AssertSelectedHitObject(Action<THitObject> assert)
        {
            AddStep("Is result matched", () =>
            {
                foreach (var hitObject in editorBeatmap.SelectedHitObjects.OfType<THitObject>())
                {
                    assert(hitObject);
                }
            });

            // even if there's no property changed in the lyric editor, should still trigger the change handler.
            // because every change handler call should cause one undo step.
            // also, technically should not call the change handler if there's no possible to change the properties.
            AssertTransactionOnlyTriggerOnce();
        }

        protected void AssertSelectedHitObjects(Action<IEnumerable<THitObject>> assert)
        {
            AddStep("Is result matched", () =>
            {
                assert(editorBeatmap.SelectedHitObjects.OfType<THitObject>());
            });

            // even if there's no property changed in the lyric editor, should still trigger the change handler.
            // because every change handler call should cause one undo step.
            // also, technically should not call the change handler if there's no possible to change the properties.
            AssertTransactionOnlyTriggerOnce();
        }
    }
}
