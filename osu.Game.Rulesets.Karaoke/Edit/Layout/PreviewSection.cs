// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;
using System;
using System.ComponentModel;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout
{
    internal class PreviewSection : LayoutSection
    {
        protected override string Title => "Preview(Won't be saved)";

        [BackgroundDependencyLoader]
        private void load(LayoutManager manager)
        {
            Children = new Drawable[]
            {
                new LabelledDropdown<PreviewRatio>
                {
                    Label = "Ratio",
                    Description = "Adjust to see different preview ratio.",
                    Items = (PreviewRatio[])Enum.GetValues(typeof(PreviewRatio)),
                },
                new LabelledDropdown<PreviewSample>
                {
                    Label = "Lyric",
                    Description = "Select different lyric to check layout is valid.",
                    Items = (PreviewSample[])Enum.GetValues(typeof(PreviewSample))
                },
                new LabelledDropdown<string>
                {
                    Label = "Style",
                    Description = "Select different style to check layout is valid.",
                    Items = manager.PreviewFontSelections.Select(x => x.Value).ToArray()
                },
            };
        }

        internal enum PreviewRatio
        {
            [Description("16:9")]
            WideScreen,

            [Description("4:3")]
            LegacyScreen,
        }

        internal enum PreviewSample
        {
            [Description("Small lyric")]
            SampeSmall,

            [Description("Medium lyric")]
            SampleMedium,

            [Description("Large lyric")]
            SampleLarge
        }
    }
}
