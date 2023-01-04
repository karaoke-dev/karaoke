// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages.Classic;

public class ClassicStageInfo : StageInfo
{
    #region Style

    /// <summary>
    /// Default <see cref="Lyric"/> and <see cref="Note"/>'s style.
    /// Will use this style as default if there's no mapping result in the <see cref="StyleMappings"/>
    /// </summary>
    public ClassicStyle DefaultStyle { get; set; } = new(0);

    /// <summary>
    /// All available <see cref="Lyric"/> and <see cref="Note"/>'s style.
    /// </summary>
    public IList<ClassicStyle> AvailableStyles { get; set; } = new List<ClassicStyle>();

    /// <summary>
    /// Mapping between <see cref="Lyric.ID"/>, <see cref="Note.ReferenceLyricId"/> and <see cref="ClassicStyle.ID"/>
    /// </summary>
    public IDictionary<int, int> StyleMappings { get; set; } = new Dictionary<int, int>();

    public void AddStyle(Action<ClassicStyle>? action = null)
    {
        int id = getNewStyleId();
        var style = new ClassicStyle(id);

        action?.Invoke(style);
        AvailableStyles.Add(style);

        int getNewStyleId()
        {
            if (AvailableStyles.Count == 0)
                return 1;

            return AvailableStyles.Max(x => x.ID) + 1;
        }
    }

    public void EditStyle(int? id, Action<ClassicStyle> action)
    {
        var style = getStyleById(id);
        action.Invoke(style);

        ClassicStyle getStyleById(int? id) =>
            id == null
                ? DefaultStyle
                : AvailableStyles.First(x => x.ID == id);
    }

    public IEnumerable<int> GetMappingLyricIds(ClassicStyle style)
        => StyleMappings.Where(x => x.Value == style.ID).Select(x => x.Key).ToArray();

    public void RemoveStyle(ClassicStyle style)
    {
        var mappingLyricIds = GetMappingLyricIds(style);

        foreach (int lyricId in mappingLyricIds)
        {
            StyleMappings.Remove(lyricId);
        }

        AvailableStyles.Remove(style);
    }

    public void ApplyStyleMapping(ClassicStyle style, Lyric lyric)
    {
        int kay = lyric.ID;
        int value = style.ID;

        if (StyleMappings.ContainsKey(kay))
        {
            StyleMappings[kay] = value;
        }
        else
        {
            StyleMappings.TryAdd(kay, value);
        }
    }

    public void ClearStyleMapping(Lyric lyric)
    {
        StyleMappings.Remove(lyric.ID);
    }

    #endregion

    #region Layout

    /// <summary>
    /// The definition for the <see cref="Lyric"/>.
    /// Like the line height or font size.
    /// </summary>
    public ClassicLyricLayoutDefinition LyricLayoutDefinition { get; set; } = new();

    /// <summary>
    /// Default <see cref="Lyric"/> layout.
    /// Will use this layout as default if there's no mapping result in the <see cref="LyricLayoutMappings"/>
    /// </summary>
    public ClassicLyricLayout DefaultLyricLayout { get; set; } = new(0);

    /// <summary>
    /// All available lyric layout.
    /// </summary>
    public IList<ClassicLyricLayout> AvailableLyricLayouts { get; set; } = new List<ClassicLyricLayout>();

    /// <summary>
    /// Mapping between <see cref="Lyric.ID"/> and <see cref="ClassicLyricLayout.ID"/>
    /// </summary>
    public IDictionary<int, int> LyricLayoutMappings { get; set; } = new Dictionary<int, int>();

    public void AddLyricLayout(Action<ClassicLyricLayout>? action = null)
    {
        int id = getNewLyricLayoutId();
        var lyricLayout = new ClassicLyricLayout(id);

        action?.Invoke(lyricLayout);
        AvailableLyricLayouts.Add(lyricLayout);

        int getNewLyricLayoutId()
        {
            if (AvailableLyricLayouts.Count == 0)
                return 1;

            return AvailableLyricLayouts.Max(x => x.ID) + 1;
        }
    }

    public void EditLyricLayout(int? id, Action<ClassicLyricLayout> action)
    {
        var lyricLayout = getLyricLayoutById(id);
        action.Invoke(lyricLayout);

        ClassicLyricLayout getLyricLayoutById(int? id) =>
            id == null
                ? DefaultLyricLayout
                : AvailableLyricLayouts.First(x => x.ID == id);
    }

    public IEnumerable<int> GetMappingLyricIds(ClassicLyricLayout lyricLayout)
        => LyricLayoutMappings.Where(x => x.Value == lyricLayout.ID).Select(x => x.Key).ToArray();

    public void RemoveLyricLayout(ClassicLyricLayout layout)
    {
        var mappingLyricIds = GetMappingLyricIds(layout);

        foreach (int lyricId in mappingLyricIds)
        {
            LyricLayoutMappings.Remove(lyricId);
        }

        AvailableLyricLayouts.Remove(layout);
    }

    public void ApplyLyricLayoutMapping(ClassicLyricLayout layout, Lyric lyric)
    {
        int kay = lyric.ID;
        int value = layout.ID;

        if (LyricLayoutMappings.ContainsKey(kay))
        {
            LyricLayoutMappings[kay] = value;
        }
        else
        {
            LyricLayoutMappings.TryAdd(kay, value);
        }
    }

    public void ClearLyricLayoutMapping(Lyric lyric)
    {
        LyricLayoutMappings.Remove(lyric.ID);
    }

    #endregion

    #region Stage element

    protected override IEnumerable<IStageElement> GetLyricStageElements(Lyric lyric)
    {
        int id = lyric.ID;

        // get the style element.
        if (StyleMappings.TryGetValue(id, out int styleId))
        {
            var matchedStyle = AvailableStyles.FirstOrDefault(x => x.ID == styleId);
            if (matchedStyle != null)
                yield return matchedStyle;
            else
                yield return DefaultStyle;
        }
        else
        {
            yield return DefaultStyle;
        }

        // get the layout element.
        if (LyricLayoutMappings.TryGetValue(id, out int layoutId))
        {
            var matchedLayout = AvailableLyricLayouts.FirstOrDefault(x => x.ID == layoutId);
            if (matchedLayout != null)
                yield return matchedLayout;
            else
                yield return DefaultLyricLayout;
        }
        else
        {
            yield return DefaultLyricLayout;
        }
    }

    protected override IEnumerable<IStageElement> GetNoteStageElements(Note note)
    {
        int? id = note.ReferenceLyricId;
        if (id == null)
            yield break;

        // get the style element.
        if (StyleMappings.TryGetValue(id.Value, out int styleId))
        {
            var matchedStyle = AvailableStyles.FirstOrDefault(x => x.ID == styleId);
            if (matchedStyle != null)
                yield return matchedStyle;
            else
                yield return DefaultStyle;
        }
        else
        {
            yield return DefaultStyle;
        }
    }

    protected override IEnumerable<object> ConvertToLyricStageAppliers(IEnumerable<IStageElement> elements)
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerable<object> ConvertToNoteStageAppliers(IEnumerable<IStageElement> elements)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
