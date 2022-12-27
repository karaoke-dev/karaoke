// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Karaoke.Beatmaps.Metadatas;

public class PageInfo : IDeepCloneable<PageInfo>
{
    [JsonIgnore]
    public IBindable<int> PagesVersion => pagesVersion;

    private readonly Bindable<int> pagesVersion = new();

    public BindableList<Page> Pages = new();

    [JsonIgnore]
    public List<Page> SortedPages { get; private set; } = new();

    public PageInfo()
    {
        Pages.CollectionChanged += (_, args) =>
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Debug.Assert(args.NewItems != null);

                    foreach (var c in args.NewItems.Cast<Page>())
                        c.TimeBindable.ValueChanged += timeValueChanged;
                    break;

                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Remove:
                    Debug.Assert(args.OldItems != null);

                    foreach (var c in args.OldItems.Cast<Page>())
                        c.TimeBindable.ValueChanged -= timeValueChanged;
                    break;
            }

            onPageChanged();

            void timeValueChanged(ValueChangedEvent<double> e) => onPageChanged();
        };

        void onPageChanged()
        {
            SortedPages = Pages.OrderBy(x => x.Time).ToList();
            pagesVersion.Value++;
        }
    }

    public Page? GetPageAt(double time)
    {
        if (SortedPages.Count < 2)
            return null;

        var page = SortedPages.LastOrDefault(x => x.Time <= time);

        // should not be able to get the page if time exceed the last page.
        var lastPage = SortedPages.LastOrDefault();
        if (page == lastPage && page?.Time < time)
            return null;

        return page;
    }

    public int? GetPageIndexAt(double time)
    {
        var page = GetPageAt(time);
        if (page == null)
            return null;

        return SortedPages.FindIndex(x => x == page);
    }

    public int? GetPageOrder(Page page)
    {
        int index = SortedPages.IndexOf(page);
        return index == -1 ? null : index + 1;
    }

    public PageInfo DeepClone()
    {
        var controlPointInfo = (PageInfo)Activator.CreateInstance(GetType())!;

        return controlPointInfo;
    }
}
