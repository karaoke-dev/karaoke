// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using osu.Game.Rulesets.Karaoke.Skinning.Elements;

namespace osu.Game.Rulesets.Karaoke.IO.Serialization.Converters
{
    public class KaraokeSkinElementConvertor : GenericTypeConvertor<IKaraokeSkinElement, ElementType>
    {
        protected override Type GetTypeByName(ElementType name)
            => GetObjectType(name);

        protected override ElementType GetNameByType(MemberInfo type)
            => GetElementType(type);

        public static ElementType GetElementType(MemberInfo elementType) =>
            elementType switch
            {
                var type when type == typeof(LyricConfig) => ElementType.LyricConfig,
                var type when type == typeof(LyricLayout) => ElementType.LyricLayout,
                var type when type == typeof(LyricStyle) => ElementType.LyricStyle,
                var type when type == typeof(NoteStyle) => ElementType.NoteStyle,
                _ => throw new NotSupportedException()
            };

        public static Type GetObjectType(ElementType elementType) =>
            elementType switch
            {
                ElementType.LyricConfig => typeof(LyricConfig),
                ElementType.LyricLayout => typeof(LyricLayout),
                ElementType.LyricStyle => typeof(LyricStyle),
                ElementType.NoteStyle => typeof(NoteStyle),
                _ => throw new NotSupportedException()
            };
    }
}
