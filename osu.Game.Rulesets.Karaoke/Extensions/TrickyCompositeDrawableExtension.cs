// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Reflection;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Extensions
{
    /// <summary>
    /// It's a tricky extension to get all non-public methods.
    /// Should be removed eventually.
    /// </summary>
    public static class TrickyCompositeDrawableExtension
    {
        public static IReadOnlyList<Drawable> GetInternalChildren(this CompositeDrawable compositeDrawable)
        {
            // see this shit to access internal property.
            // https://stackoverflow.com/a/7575615/4105113
            var prop = compositeDrawable.GetType().GetProperty("InternalChildren", BindingFlags.Instance |
                                                                                   BindingFlags.NonPublic |
                                                                                   BindingFlags.Public);
            if (prop == null)
                return null;

            return (IReadOnlyList<Drawable>)prop.GetValue(compositeDrawable);
        }
    }
}
