// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas.Types;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas
{
    public class Singer : ISinger
    {
        public Singer()
        {
        }

        public Singer(int id)
        {
            ID = id;
        }

        public int ID { get; private set; }

        [JsonIgnore]
        public readonly Bindable<int> OrderBindable = new Bindable<int>();

        /// <summary>
        /// Order
        /// </summary>
        public int Order
        {
            get => OrderBindable.Value;
            set => OrderBindable.Value = value;
        }

        public string Name { get; set; }

        public string RomajiName { get; set; }

        public string EnglishName { get; set; }

        public Color4? Color { get; set; }

        public string Avatar { get; set; }

        public string Description { get; set; }
    }
}
