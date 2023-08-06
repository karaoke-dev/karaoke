// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Beatmaps;
using osu.Game.Rulesets.Karaoke.Objects;

namespace osu.Game.Rulesets.Karaoke.Stages;

/// <summary>
/// It's a category to record the list of <typeparamref name="TStageElement"/> and handle the mapping by several rules.
/// </summary>
public abstract class StageElementCategory<TStageElement, THitObject>
    where TStageElement : StageElement, IComparable<TStageElement>, new()
    where THitObject : KaraokeHitObject, IHasPrimaryKey
{
    /// <summary>
    /// Default value.<br/>
    /// Will use this value as default if there's no mapping result in the <typeparamref name="TStageElement"/>
    /// </summary>
    public TStageElement DefaultElement { get; protected set; }

    [JsonIgnore]
    public IBindable<int> ElementsVersion => elementsVersion;

    private readonly Bindable<int> elementsVersion = new();

    /// <summary>
    /// All available elements.
    /// </summary>
    public BindableList<TStageElement> AvailableElements { get; protected set; } = new();

    [JsonIgnore]
    public List<TStageElement> SortedElements { get; private set; } = new();

    /// <summary>
    /// Mapping between <typeparamref name="THitObject.ID"/> and <typeparamref name="TStageElement.ID"/><br/>
    /// This is the 1st mapping roles.
    /// </summary>
    public IDictionary<ElementId, ElementId> Mappings { get; protected set; } = new Dictionary<ElementId, ElementId>();

    protected StageElementCategory()
    {
        DefaultElement = new TStageElement();

        AvailableElements.CollectionChanged += (_, args) =>
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Debug.Assert(args.NewItems != null);

                    foreach (var c in args.NewItems.Cast<TStageElement>())
                        c.OrderVersion.ValueChanged += orderValueChanged;
                    break;

                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Remove:
                    Debug.Assert(args.OldItems != null);

                    foreach (var c in args.OldItems.Cast<TStageElement>())
                        c.OrderVersion.ValueChanged -= orderValueChanged;
                    break;
            }

            onElementOrderChanged();

            void orderValueChanged(ValueChangedEvent<int> e) => onElementOrderChanged();
        };

        void onElementOrderChanged()
        {
            SortedElements = AvailableElements.OrderBy(x => x).ToList();
            elementsVersion.Value++;
        }
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

    public void ClearElements()
    {
        Mappings.Clear();
        AvailableElements.Clear();
    }

    public void AddToMapping(TStageElement element, THitObject hitObject)
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

    public void RemoveHitObjectFromMapping(THitObject hitObject)
    {
        Mappings.Remove(hitObject.ID);
    }

    public void RemoveElementFromMapping(TStageElement element)
    {
        var objectIds = getMappingHitObjectIds(element);

        foreach (var objectId in objectIds)
        {
            Mappings.Remove(objectId);
        }

        IEnumerable<ElementId> getMappingHitObjectIds(TStageElement stageElement)
            => Mappings.Where(x => x.Value == stageElement.ID).Select(x => x.Key).ToArray();
    }

    public void ClearUnusedMapping(Func<ElementId, bool> checkExist)
    {
        var unusedIds = Mappings.Select(x => x.Key).Where(x => !checkExist(x));

        foreach (var hitObjectId in unusedIds)
        {
            Mappings.Remove(hitObjectId);
        }
    }

    #endregion

    #region Query

    public TStageElement GetElementByItem(THitObject hitObject)
    {
        var id = hitObject.ID;

        if (!Mappings.TryGetValue(id, out var elementId))
            return DefaultElement;

        var matchedElements = AvailableElements.FirstOrDefault(x => x.ID == elementId);
        return matchedElements ?? DefaultElement;
    }

    public IEnumerable<ElementId> GetHitObjectIdsByElement(TStageElement element)
    {
        return Mappings.Where(x => x.Value == element.ID).Select(x => x.Key);
    }

    public int? GetElementOrder(TStageElement element)
    {
        int index = SortedElements.IndexOf(element);
        return index == -1 ? null : index + 1;
    }

    #endregion
}
