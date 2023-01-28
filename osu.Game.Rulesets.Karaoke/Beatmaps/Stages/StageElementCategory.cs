// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Objects.Types;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Stages;

/// <summary>
/// It's a category to record the list of <typeparamref name="TStageElement"/> and handle the mapping by several rules.
/// </summary>
public abstract class StageElementCategory<TStageElement, THitObject>
    where TStageElement : class, IStageElement, IComparable<TStageElement>
    where THitObject : KaraokeHitObject, IHasPrimaryKey
{
    /// <summary>
    /// Default value.
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
    /// Mapping between <typeparamref name="THitObject.ID"/> and <typeparamref name="TStageElement.ID"/>
    /// This is the 1st mapping roles.
    /// </summary>
    public IDictionary<int, int> Mappings { get; protected set; } = new Dictionary<int, int>();

    protected StageElementCategory()
    {
        DefaultElement = CreateElement(0);

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

    protected abstract TStageElement CreateElement(int id);

    public void AddElement(Action<TStageElement>? action = null)
    {
        int id = getNewElementId();
        var element = CreateElement(id);

        action?.Invoke(element);
        AvailableElements.Add(element);

        int getNewElementId()
        {
            if (AvailableElements.Count == 0)
                return 1;

            return AvailableElements.Max(x => x.ID) + 1;
        }
    }

    public void EditElement(int? id, Action<TStageElement> action)
    {
        var element = getElementById(id);
        action.Invoke(element);

        TStageElement getElementById(int? elementID) =>
            elementID == null
                ? DefaultElement
                : AvailableElements.First(x => x.ID == elementID);
    }

    public void RemoveElement(TStageElement element)
    {
        RemoveElementFromMapping(element);

        AvailableElements.Remove(element);
    }

    public void AddToMapping(TStageElement element, THitObject hitObject)
    {
        int key = hitObject.ID;
        int value = element.ID;

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

        foreach (int objectId in objectIds)
        {
            Mappings.Remove(objectId);
        }

        IEnumerable<int> getMappingHitObjectIds(TStageElement stageElement)
            => Mappings.Where(x => x.Value == stageElement.ID).Select(x => x.Key).ToArray();
    }

    public void ClearUnusedMapping(Func<int, bool> checkExist)
    {
        var unusedIds = Mappings.Select(x => x.Key).Where(x => !checkExist(x));

        foreach (int hitObjectId in unusedIds)
        {
            Mappings.Remove(hitObjectId);
        }
    }

    #endregion

    #region Query

    public TStageElement GetElementByItem(THitObject hitObject)
    {
        int id = hitObject.ID;

        if (!Mappings.TryGetValue(id, out int styleId))
            return DefaultElement;

        var matchedStyle = AvailableElements.FirstOrDefault(x => x.ID == styleId);
        return matchedStyle ?? DefaultElement;
    }

    #endregion
}
