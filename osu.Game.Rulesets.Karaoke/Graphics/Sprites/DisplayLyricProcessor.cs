// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Extensions.EnumExtensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Graphics.Sprites.Processor;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites;

public class DisplayLyricProcessor : IDisposable
{
    public Action<IReadOnlyList<PositionText>>? TopTextChanged;
    public Action<string>? CenterTextChanged;
    public Action<IReadOnlyList<PositionText>>? BottomTextChanged;
    public Action<IReadOnlyDictionary<double, TextIndex>>? TimeTagsChanged;

    private BaseDisplayProcessor? processor;

    private readonly Lyric lyric;

    public DisplayLyricProcessor(Lyric lyric)
    {
        this.lyric = lyric;
        reloadProcessor();
    }

    private LyricDisplayType displayType = LyricDisplayType.Lyric;

    public LyricDisplayType DisplayType
    {
        get => displayType;
        set
        {
            if (displayType == value)
                return;

            displayType = value;
            reloadProcessor();
        }
    }

    private LyricDisplayProperty displayProperty = LyricDisplayProperty.Both;

    public LyricDisplayProperty DisplayProperty
    {
        get => displayProperty;
        set
        {
            if (displayProperty == value)
                return;

            displayProperty = value;
            reloadProcessor();
        }
    }

    private void reloadProcessor()
    {
        // re-create the processor.
        processor?.Dispose();
        processor = GetLyricDisplayProcessor(lyric, DisplayType, DisplayProperty);
        processor = DisplayType switch
        {
            LyricDisplayType.Lyric => new LyricFirstDisplayProcessor(lyric, DisplayProperty),
            LyricDisplayType.RomanisedSyllable => new RomanisedSyllableFirstDisplayProcessor(lyric, DisplayProperty),
            _ => throw new ArgumentOutOfRangeException(),
        };

        // pass the change event.
        processor.TopTextChanged = x => TopTextChanged?.Invoke(x);
        processor.CenterTextChanged = x => CenterTextChanged?.Invoke(x);
        processor.BottomTextChanged = x => BottomTextChanged?.Invoke(x);
        processor.TimeTagsChanged = x => TimeTagsChanged?.Invoke(x);

        // trigger update all after update the processor.
        UpdateAll();
    }

    /// <summary>
    /// Should call this method after Action is bind outside.
    /// </summary>
    public void UpdateAll()
    {
        if (processor == null)
            throw new InvalidOperationException("Processor should not be null.");

        processor.UpdateAll();

        // should trigger top text update even not display.
        if (!displayProperty.HasFlagFast(LyricDisplayProperty.TopText))
        {
            TopTextChanged?.Invoke(Array.Empty<PositionText>());
        }

        // should trigger bottom text update even not display.
        if (!displayProperty.HasFlagFast(LyricDisplayProperty.BottomText))
        {
            BottomTextChanged?.Invoke(Array.Empty<PositionText>());
        }
    }

    public void Dispose()
    {
        processor?.Dispose();
    }

    public static BaseDisplayProcessor GetLyricDisplayProcessor(Lyric lyric, LyricDisplayType displayType, LyricDisplayProperty displayProperty) =>
        displayType switch
        {
            LyricDisplayType.Lyric => new LyricFirstDisplayProcessor(lyric, displayProperty),
            LyricDisplayType.RomanisedSyllable => new RomanisedSyllableFirstDisplayProcessor(lyric, displayProperty),
            _ => throw new ArgumentOutOfRangeException(),
        };
}
