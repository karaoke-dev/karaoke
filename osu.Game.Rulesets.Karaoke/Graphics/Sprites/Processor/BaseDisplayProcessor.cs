// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Extensions.EnumExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites.Processor;

public abstract class BaseDisplayProcessor : IDisposable
{
    public Action<IReadOnlyList<PositionText>>? TopTextChanged;
    public Action<string>? CenterTextChanged;
    public Action<IReadOnlyList<PositionText>>? BottomTextChanged;
    public Action<IReadOnlyDictionary<double, TextIndex>>? TimeTagsChanged;

    private readonly Lyric lyric;
    private readonly LyricDisplayProperty displayProperty;
    private readonly AvailableProperty availableProperty;

    protected BaseDisplayProcessor(Lyric lyric, LyricDisplayProperty displayProperty)
    {
        this.lyric = lyric;
        this.displayProperty = displayProperty;

        availableProperty = new AvailableProperty(lyric);

        ProcessBindableChange(availableProperty);
    }

    protected abstract void ProcessBindableChange(AvailableProperty property);

    public void UpdateAll()
    {
        UpdateTopText();
        UpdateCenterText();
        UpdateBottomText();
        UpdateTimeTags();
    }

    protected void UpdateTopText()
    {
        if (!displayProperty.HasFlagFast(LyricDisplayProperty.TopText))
            return;

        TopTextChanged?.Invoke(CalculateTopTexts(lyric).ToArray());
    }

    protected void UpdateCenterText()
    {
        CenterTextChanged?.Invoke(CalculateCenterText(lyric));
    }

    protected void UpdateBottomText()
    {
        if (!displayProperty.HasFlagFast(LyricDisplayProperty.BottomText))
            return;

        BottomTextChanged?.Invoke(CalculateBottomTexts(lyric).ToArray());
    }

    protected void UpdateTimeTags()
    {
        TimeTagsChanged?.Invoke(CalculateTimeTags(lyric));
    }

    protected abstract IEnumerable<PositionText> CalculateTopTexts(Lyric lyric);

    protected abstract string CalculateCenterText(Lyric lyric);

    protected abstract IEnumerable<PositionText> CalculateBottomTexts(Lyric lyric);

    protected abstract IReadOnlyDictionary<double, TextIndex> CalculateTimeTags(Lyric lyric);

    public void Dispose()
    {
        availableProperty.Dispose();
    }

    protected class AvailableProperty : IDisposable
    {
        public readonly IBindableList<RubyTag> RubyTagsBindable = new BindableList<RubyTag>();
        public readonly IBindable<int> RubyTagsVersion = new Bindable<int>();
        public readonly IBindable<string> TextBindable = new Bindable<string>();
        public readonly IBindableList<TimeTag> TimeTagsBindable = new BindableList<TimeTag>();
        public readonly IBindable<int> TimeTagsRomajiVersion = new Bindable<int>();
        public readonly IBindable<int> TimeTagsTimingVersion = new Bindable<int>();

        public AvailableProperty(Lyric lyric)
        {
            RubyTagsBindable.BindTo(lyric.RubyTagsBindable);
            RubyTagsVersion.BindTo(lyric.RubyTagsVersion);
            TextBindable.BindTo(lyric.TextBindable);
            TimeTagsBindable.BindTo(lyric.TimeTagsBindable);
            TimeTagsRomajiVersion.BindTo(lyric.TimeTagsRomajiVersion);
            TimeTagsTimingVersion.BindTo(lyric.TimeTagsTimingVersion);
        }

        public void Dispose()
        {
            RubyTagsBindable.UnbindAll();
            RubyTagsVersion.UnbindAll();
            TextBindable.UnbindAll();
            TimeTagsBindable.UnbindAll();
            TimeTagsRomajiVersion.UnbindAll();
            TimeTagsTimingVersion.UnbindAll();
        }
    }
}
