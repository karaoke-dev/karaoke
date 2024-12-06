// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters;

public class KaraokeSkinElementConverter : GenericTypeConverter<IKaraokeSkinElement, ElementType>
{
    protected override Type GetTypeByName(ElementType name)
        => GetObjectType(name);

    protected override ElementType GetNameByType(MemberInfo type)
        => GetElementType(type);

    public static ElementType GetElementType(MemberInfo elementType) =>
        elementType switch
        {
            _ when elementType == typeof(LyricFontInfo) => ElementType.LyricFontInfo,
            _ when elementType == typeof(LyricStyle) => ElementType.LyricStyle,
            _ when elementType == typeof(NoteStyle) => ElementType.NoteStyle,
            _ => throw new NotSupportedException(),
        };

    public static Type GetObjectType(ElementType elementType) =>
        elementType switch
        {
            ElementType.LyricFontInfo => typeof(LyricFontInfo),
            ElementType.LyricStyle => typeof(LyricStyle),
            ElementType.NoteStyle => typeof(NoteStyle),
            _ => throw new NotSupportedException(),
        };
}
