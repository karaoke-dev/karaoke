// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Edit.ChangeHandlers;

/// <summary>
/// This interface is defined checking able to generate or detect the property, and make the change for the property.
/// </summary>
/// <typeparam name="TEnum"></typeparam>
public interface IEnumAutoGenerateChangeHandler<in TEnum> where TEnum : Enum
{
    bool CanGenerate(TEnum property);

    void AutoGenerate(TEnum property);
}

/// <summary>
/// This interface is defined checking able to generate or detect the property, and make the change for the property.
/// </summary>
/// <typeparam name="TType"></typeparam>
public interface IAutoGenerateChangeHandler<in TType>
{
    bool CanGenerate<T>() where T : TType;

    void AutoGenerate<T>() where T : TType;
}

/// <summary>
/// This interface is defined checking able to generate or detect the property, and make the change for the property.
/// </summary>
public interface IAutoGenerateChangeHandler
{
    bool CanGenerate();

    void AutoGenerate();
}
