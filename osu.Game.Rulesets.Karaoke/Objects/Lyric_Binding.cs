// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Specialized;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Objects
{
    public partial class Lyric
    {
        private void initInternalBindingEvent()
        {
            TimeTagsBindable.CollectionChanged += (_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var c in args.NewItems.Cast<TimeTag>())
                            c.Changed += invalidate;
                        break;

                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var c in args.OldItems.Cast<TimeTag>())
                            c.Changed -= invalidate;
                        break;
                }

                void invalidate() => TimeTagsVersion.Value++;
            };

            RubyTagsBindable.CollectionChanged += (_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var c in args.NewItems.Cast<RubyTag>())
                            c.Changed += invalidate;
                        break;

                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var c in args.OldItems.Cast<RubyTag>())
                            c.Changed -= invalidate;
                        break;
                }

                void invalidate() => RubyTagsVersion.Value++;
            };

            RomajiTagsBindable.CollectionChanged += (_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var c in args.NewItems.Cast<RomajiTag>())
                            c.Changed += invalidate;
                        break;

                    case NotifyCollectionChangedAction.Reset:
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var c in args.OldItems.Cast<RomajiTag>())
                            c.Changed -= invalidate;
                        break;
                }

                void invalidate() => RomajiTagsVersion.Value++;
            };

            ReferenceLyricConfigBindable.ValueChanged += e =>
            {
                if (e.OldValue != null)
                {
                    e.OldValue.Changed -= invalidate;
                }

                if (e.NewValue != null)
                {
                    e.NewValue.Changed += invalidate;
                }

                void invalidate() => ReferenceLyricConfigVersion.Value++;
            };
        }
    }
}
