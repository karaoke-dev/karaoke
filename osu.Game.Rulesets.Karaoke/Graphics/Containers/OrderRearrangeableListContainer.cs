// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Specialized;
using osu.Game.Graphics.Containers;

namespace osu.Game.Rulesets.Karaoke.Graphics.Containers
{
    public abstract class OrderRearrangeableListContainer<TModel> : OsuRearrangeableListContainer<TModel>
    {
        public event Action<TModel, int> OnOrderChanged;

        protected OrderRearrangeableListContainer()
        {
            // this collection change event cannot directly regist in parent bindable.
            // So regist in here.
            Items.CollectionChanged += collectionChanged;
        }

        private void collectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                // should get the event if user change the position.
                case NotifyCollectionChangedAction.Move:
                    var item = (TModel)e.NewItems[0];
                    var newInex = e.NewStartingIndex;
                    OnOrderChanged?.Invoke(item, newInex);
                    break;
            }
        }
    }
}
