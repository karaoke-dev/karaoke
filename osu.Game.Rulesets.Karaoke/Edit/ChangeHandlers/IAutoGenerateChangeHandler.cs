// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;

/// <summary>
/// This interface is defined checking able to generate or detect the property, and make the change for the property.
/// </summary>
/// <typeparam name="TEnum"></typeparam>
public interface IAutoGenerateChangeHandler<in TEnum> where TEnum : struct, Enum
{
    bool CanGenerate(TEnum category);

    void AutoGenerate(TEnum category);
}

/// <summary>
/// This interface is defined checking able to generate or detect the property, and make the change for the property.
/// </summary>
public interface IAutoGenerateChangeHandler
{
    bool CanGenerate();

    void AutoGenerate();
}
