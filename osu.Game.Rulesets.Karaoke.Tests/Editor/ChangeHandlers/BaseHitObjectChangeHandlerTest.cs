// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers;

public abstract partial class BaseHitObjectChangeHandlerTest<TChangeHandler, THitObject> : BaseChangeHandlerTest<TChangeHandler>
    where TChangeHandler : HitObjectChangeHandler<THitObject>, new() where THitObject : HitObject
{
    protected void AssertHitObject(Action<THitObject> assert)
    {
        AssertHitObject<THitObject>(assert);
    }

    protected void AssertHitObjects(Action<IEnumerable<THitObject>> assert)
    {
        AssertHitObjects<THitObject>(assert);
    }

    protected void AssertSelectedHitObject(Action<THitObject> assert)
    {
        AssertSelectedHitObjects(hitObjects =>
        {
            foreach (var hitObject in hitObjects)
            {
                assert(hitObject);
            }
        });
    }

    protected void AssertSelectedHitObjects(Action<IEnumerable<THitObject>> assert)
    {
        AddStep("Is result matched", () =>
        {
            var editorBeatmap = Dependencies.Get<EditorBeatmap>();
            assert(editorBeatmap.SelectedHitObjects.OfType<THitObject>());
        });

        AssertStatus();
    }
}
