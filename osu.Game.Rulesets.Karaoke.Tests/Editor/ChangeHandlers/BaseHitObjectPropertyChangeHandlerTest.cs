// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Karaoke.Tests.Editor.ChangeHandlers
{
    public abstract class BaseHitObjectPropertyChangeHandlerTest<TChangeHandler, THitObject> : BaseHitObjectChangeHandlerTest<TChangeHandler, THitObject>
        where TChangeHandler : HitObjectPropertyChangeHandler<THitObject>, new() where THitObject : HitObject
    {
        protected void TriggerHandlerChangedWithChangeForbiddenException(Action<TChangeHandler> c)
        {
            TriggerHandlerChangedWithException<HitObjectPropertyChangeHandler<THitObject>.ChangeForbiddenException>(c);
        }
    }
}
