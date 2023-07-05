// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Game.Rulesets.Karaoke.Beatmaps;

public interface IHasPrimaryKey
{
    ElementId ID { get; }
}

public static class PrimaryKeyObjectExtension
{
    public static TObject ChangeId<TObject>(this TObject obj, ElementId id)
        where TObject : IHasPrimaryKey
    {
        // get id from the obj and override the id.
        var propertyInfo = obj.GetType().GetProperty(nameof(IHasPrimaryKey.ID));
        if (propertyInfo == null)
            throw new InvalidOperationException();

        propertyInfo.SetValue(obj, id);

        return obj;
    }
}
