// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Skinning;
using osu.Game.Rulesets.Karaoke.Skinning.Components;
using osu.Game.Skinning;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout
{
    public class LayoutManager : Component
    {
        public readonly BindableList<KaraokeLayout> Layouts = new BindableList<KaraokeLayout>();

        public readonly IDictionary<int, string> PreviewFontSelections = new Dictionary<int, string>();

        public readonly IBindable<LyricLine> PreviewLyricLine = new Bindable<LyricLine>();

        public readonly IBindable<float> PreviewPreviewRatio = new Bindable<float>();

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

            var skinLookups = source.GetConfig<KaraokeIndexLookup, IDictionary<int, string>>(KaraokeIndexLookup.Style)?.Value;
            foreach (var skinLookup in skinLookups)
            {
                PreviewFontSelections.Add(skinLookup.Key, skinLookup.Value);
            }
        }
    }
}
