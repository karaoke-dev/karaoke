// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Skinning;
using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout
{
    public class LayoutManager : Component
    {
        public readonly BindableList<KaraokeLayout> Layouts = new BindableList<KaraokeLayout>();

        public readonly Bindable<KaraokeLayout> CurrentLayout = new Bindable<KaraokeLayout>();

        public readonly IDictionary<int, string> PreviewFontSelections = new Dictionary<int, string>();

        public readonly Bindable<LyricLine> PreviewLyricLine = new Bindable<LyricLine>();

        public readonly Bindable<DisplayRatio> PreviewPreviewRatio = new Bindable<DisplayRatio>();

        [Resolved]
        private ISkinSource source { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            var layoutLookups = source.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Layout)?.Value;
            foreach (var layoutLookup in layoutLookups)
            {
                var lookup = new KaraokeSkinLookup(KaraokeSkinConfiguration.LyricLayout, layoutLookup.Key);
                var layout = source.GetConfig<KaraokeSkinLookup, KaraokeLayout>(lookup)?.Value;
                if (layout != null)
                    Layouts.Add(layout);
            }

            CurrentLayout.Value = Layouts.FirstOrDefault();

            var skinLookups = source.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Style)?.Value;
            foreach (var skinLookup in skinLookups)
            {
                PreviewFontSelections.Add(skinLookup.Key, skinLookup.Value);
            }
        }

        public void ApplyCurrenyLayoutChange(Action<KaraokeLayout> action)
        {
            action?.Invoke(CurrentLayout.Value);
            CurrentLayout.TriggerChange();
        }

        public void ChangeCurrenyLayout(KaraokeLayout layout)
        {
            CurrentLayout.Value = layout;
        }
    }

    public struct DisplayRatio
    {
        public float Width { get; set; }

        public float Height { get; set; }

        public bool isValid()
            => Width > 0 && Height > 0;
    }
}
