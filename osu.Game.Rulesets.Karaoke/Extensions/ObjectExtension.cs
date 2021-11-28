// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    public static class ObjectExtension
    {
        public static TType CallMethod<TType>(this object obj, string methodName)
        {
            return (TType)obj.GetType().GetMethod(methodName)?.Invoke(obj, null);
        }

        public static TType CallMethod<TType, TProperty>(this object obj, string methodName, TProperty param)
        {
            return obj.CallMethod<TType>(methodName, new object[] { param });
        }

        public static TType CallMethod<TType>(this object obj, string methodName, object[] @params)
        {
            return (TType)obj.GetType().GetMethod(methodName)?.Invoke(obj, @params);
        }
    }
}
