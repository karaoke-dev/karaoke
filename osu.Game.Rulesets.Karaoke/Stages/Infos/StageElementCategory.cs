// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Stages.Infos;

/// <summary>
/// It's a category to record the list of <typeparamref name="TStageElement"/> and handle the mapping by default role.<br/>
/// Can add more customised role by inherit this class.<br/>
/// </summary>
public abstract class StageElementCategory<TStageElement, THitObject>
    where TStageElement : StageElement, new()
    where THitObject : KaraokeHitObject, IHasPrimaryKey
{
    /// <summary>
    /// Default value.<br/>
    /// Will use this value as default if there's no mapping result in the <typeparamref name="TStageElement"/>
    /// </summary>
    public TStageElement DefaultElement { get; protected set; }

    /// <summary>
    /// All available elements.
    /// </summary>
    public BindableList<TStageElement> AvailableElements { get; protected set; } = new();

    /// <summary>
    /// Mapping between <typeparamref name="THitObject.ID"/> and <typeparamref name="TStageElement.ID"/><br/>
    /// This is the 1st mapping roles.
    /// </summary>
    public IDictionary<ElementId, ElementId> Mappings { get; protected set; } = new Dictionary<ElementId, ElementId>();

    protected StageElementCategory()
    {
        DefaultElement = new TStageElement();
    }

    #region Edit

    public TStageElement AddElement(Action<TStageElement>? action = null)
    {
        var element = new TStageElement();

        action?.Invoke(element);
        AvailableElements.Add(element);

        return element;
    }

    public void EditElement(ElementId? id, Action<TStageElement> action)
    {
        var element = getElementById(id);
        action(element);

        TStageElement getElementById(ElementId? elementID) =>
            elementID == null
                ? DefaultElement
                : AvailableElements.First(x => x.ID == elementID);
    }

    public void RemoveElement(TStageElement element)
    {
        RemoveElementFromMapping(element);

        AvailableElements.Remove(element);
    }

    public virtual void ClearElements()
    {
        Mappings.Clear();
        AvailableElements.Clear();
    }

    public virtual void AddToMapping(TStageElement element, THitObject hitObject)
    {
        var key = hitObject.ID;
        var value = element.ID;

        if (!AvailableElements.Contains(element))
            throw new InvalidOperationException();

        if (element == DefaultElement)
            throw new InvalidOperationException();

        if (!Mappings.TryAdd(key, value))
        {
            Mappings[key] = value;
        }
    }

    public virtual void RemoveHitObjectFromMapping(THitObject hitObject)
    {
        Mappings.Remove(hitObject.ID);
    }

    public virtual void RemoveElementFromMapping(TStageElement element)
    {
        var objectIds = getMappingHitObjectIds(element);

        foreach (var objectId in objectIds)
        {
            Mappings.Remove(objectId);
        }

        IEnumerable<ElementId> getMappingHitObjectIds(TStageElement stageElement)
            => Mappings.Where(x => x.Value == stageElement.ID).Select(x => x.Key).ToArray();
    }

    public virtual void ClearUnusedMapping(Func<ElementId, bool> checkExist)
    {
        var unusedIds = Mappings.Select(x => x.Key).Where(x => !checkExist(x));

        foreach (var hitObjectId in unusedIds)
        {
            Mappings.Remove(hitObjectId);
        }
    }

    #endregion

    #region Query

    public virtual TStageElement GetElementByItem(THitObject hitObject)
    {
        var id = hitObject.ID;

        if (!Mappings.TryGetValue(id, out var elementId))
            return DefaultElement;

        var matchedElements = AvailableElements.FirstOrDefault(x => x.ID == elementId);
        return matchedElements ?? DefaultElement;
    }

    public virtual IEnumerable<ElementId> GetHitObjectIdsByElement(TStageElement element)
    {
        return Mappings.Where(x => x.Value == element.ID).Select(x => x.Key);
    }

    #endregion
}
