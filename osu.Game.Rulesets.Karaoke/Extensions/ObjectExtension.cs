// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    public static class ObjectExtension
    {
        public static T CallMethod<T>(this object obj, string methodName)
        {
            return (T)obj.GetType().GetMethod(methodName).Invoke(obj, null);
        }

        public static T CallMethod<T, P>(this object obj, string methodName, P param)
        {
            return obj.CallMethod<T>(methodName, new object[] { param });
        }

        public static T CallMethod<T>(this object obj, string methodName, object[] Params)
        {
            return (T)obj.GetType().GetMethod(methodName).Invoke(obj, Params);
        }
    }
}
