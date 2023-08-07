// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Objects.Types;

public interface IHasWorkingProperty<TWorkingProperty, TFillProperty> : IHasWorkingProperty<TFillProperty>
    where TWorkingProperty : struct, Enum
{
    bool InvalidateWorkingProperty(TWorkingProperty workingProperty);

    TWorkingProperty[] GetAllInvalidWorkingProperties();
}

public interface IHasWorkingProperty<in TFillProperty>
{
    void ValidateWorkingProperty(TFillProperty fillProperty);
}
