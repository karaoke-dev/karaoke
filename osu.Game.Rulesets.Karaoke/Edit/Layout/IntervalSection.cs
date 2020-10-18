// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Layout
{
    internal class IntervalSection : LayoutSection
    {
        private LabelledSliderBar<int> lyricIntervalSliderBar;
        private LabelledSliderBar<int> rubyIntervalSliderBar;
        private LabelledSliderBar<int> romajiIntervalSliderBar;

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new[]
            {
                lyricIntervalSliderBar = new LabelledSliderBar<int>
                {
                    Label = "Lyrics interval",
                    Description = "Lyrics interval section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 30,
                        Value = 10,
                        Default = 10
                    }
                },
                rubyIntervalSliderBar = new LabelledSliderBar<int>
                {
                    Label = "Ruby interval",
                    Description = "Ruby interval section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 30,
                        Value = 10,
                        Default = 10
                    }
                },
                romajiIntervalSliderBar = new LabelledSliderBar<int>
                {
                    Name = "Romaji interval",
                    Description = "Romaji interval section",
                    Current = new BindableNumber<int>
                    {
                        MinValue = 0,
                        MaxValue = 30,
                        Value = 10,
                        Default = 10
                    }
                }
            };
        }
    }
}
