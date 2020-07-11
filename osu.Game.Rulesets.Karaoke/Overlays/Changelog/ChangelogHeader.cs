// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Game.Overlays;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
    /// <summary>
    /// Change log header, display <see cref="KaraokeChangelogBuild"/> title
    /// </summary>
    public class ChangelogHeader : BreadcrumbControlOverlayHeader
    {
        public readonly Bindable<KaraokeChangelogBuild> Build = new Bindable<KaraokeChangelogBuild>();

        public Action ListingSelected;

        private const string listing_string = "listing";

        public ChangelogHeader()
        {
            TabControl.AddItem(listing_string);

            Current.ValueChanged += e =>
            {
                if (e.NewValue == listing_string)
                    ListingSelected?.Invoke();
            };

            Build.BindValueChanged(e =>
            {
                if (e.OldValue != null)
                    TabControl.RemoveItem(e.OldValue.DisplayVersion);

                if (e.NewValue != null)
                {
                    TabControl.AddItem(e.NewValue.DisplayVersion);
                    Current.Value = e.NewValue.DisplayVersion;
                }
                else
                {
                    Current.Value = listing_string;
                }
            });
        }

        protected override OverlayTitle CreateTitle() => new ChangelogHeaderTitle();

        private class ChangelogHeaderTitle : OverlayTitle
        {
            public ChangelogHeaderTitle()
            {
                Title = "changelog";
                IconTexture = "Icons/changelog";
            }
        }
    }
}
