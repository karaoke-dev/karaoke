// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

/// <summary>
/// This section handle display the list of <typeparamref name="TModel"/>.
/// And able to create or remove the <typeparamref name="TModel"/> in here.
/// </summary>
/// <typeparam name="TModel"></typeparam>
[Cached(typeof(ISectionItemsEditorProvider))]
public abstract partial class SectionItemsEditor<TModel> : CompositeDrawable, ISectionItemsEditorProvider where TModel : class
{
    protected readonly IBindableList<TModel> Items = new BindableList<TModel>();

    private readonly Dictionary<TModel, Drawable> itemMap = new();

    private readonly FillFlowContainer content;

    protected SectionItemsEditor()
    {
        RelativeSizeAxes = Axes.X;
        AutoSizeAxes = Axes.Y;

        InternalChild = content = new FillFlowContainer
        {
            RelativeSizeAxes = Axes.X,
            AutoSizeAxes = Axes.Y,
            Spacing = new Vector2(10),
            LayoutEasing = Easing.Out,
        };

        Items.BindCollectionChanged((_, args) =>
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Debug.Assert(args.NewItems != null);
                    addItems(args.NewItems);

                    break;

                case NotifyCollectionChangedAction.Remove:
                    Debug.Assert(args.OldItems != null);
                    removeItems(args.OldItems);

                    break;
            }
        });

        addCreateButton();
    }

    public void RedrewContent()
    {
        clearDrawable();

        addItems(Items.ToList());

        addCreateButton();
    }

    private void clearDrawable()
    {
        content.Clear();
        itemMap.Clear();
    }

    private void addItems(IList items)
    {
        bool enableAddAnimation = items.Count == 1;
        content.LayoutDuration = enableAddAnimation ? 100 : 0;

        foreach (var item in items.Cast<TModel>())
        {
            var drawable = CreateDrawable(item);
            if (drawable == null)
                continue;

            content.Add(drawable);
            itemMap.Add(item, drawable);
        }
    }

    private void removeItems(IList items)
    {
        foreach (var item in items.Cast<TModel>())
        {
            if (!itemMap.TryGetValue(item, out var drawable))
                continue;

            content.Remove(drawable, true);
            itemMap.Remove(item);
        }
    }

    private void addCreateButton()
    {
        var button = CreateCreateNewItemButton();
        if (button == null)
            return;

        content.Insert(int.MaxValue, button);
    }

    public void UpdateDisplayOrder(Drawable drawable, int order)
    {
        float newPosition = order;
        content.SetLayoutPosition(drawable, newPosition);
    }

    /// <summary>
    /// Create editable drawable for the item.
    /// Return null if the item is not editable.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    protected abstract Drawable? CreateDrawable(TModel item);

    protected abstract EditorSectionButton? CreateCreateNewItemButton();
}
