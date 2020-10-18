// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2
{
    /// <summary>
    /// Will be replaced after has official one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LabelledDropdown<T> : LabelledComponent<Dropdown<T>, T>
    {
        public LabelledDropdown()
            : base(true)
        {
        }

        public IEnumerable<T> Items
        {
            get => Component.Items;
            set => Component.Items = value;
        }

        protected override Dropdown<T> CreateComponent()
            => new OsuDropdown<T>
            {
                RelativeSizeAxes = Axes.X,
            };
    }
}
