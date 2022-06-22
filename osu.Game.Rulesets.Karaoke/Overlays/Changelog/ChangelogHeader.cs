// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Localisation;
using osu.Game.Overlays;
using osu.Game.Resources.Localisation.Web;
using osu.Game.Rulesets.Karaoke.Online.API.Requests.Responses;

namespace osu.Game.Rulesets.Karaoke.Overlays.Changelog
{
    /// <summary>
    /// Change log header, display <see cref="APIChangelogBuild"/> title
    /// </summary>
    public class ChangelogHeader : BreadcrumbControlOverlayHeader
    {
        public readonly Bindable<APIChangelogBuild> Build = new();

        public Action ListingSelected;

        public static LocalisableString ListingString => LayoutStrings.HeaderChangelogIndex;

        public ChangelogHeader()
        {
            TabControl.AddItem(ListingString);

            Current.ValueChanged += e =>
            {
                if (e.NewValue == ListingString)
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
                    Current.Value = ListingString;
                }
            });
        }

        protected override OverlayTitle CreateTitle() => new ChangelogHeaderTitle();

        private class ChangelogHeaderTitle : OverlayTitle
        {
            public ChangelogHeaderTitle()
            {
                Title = PageTitleStrings.MainChangelogControllerDefault;
                Description = NamedOverlayComponentStrings.ChangelogDescription;
                IconTexture = "Icons/Hexacons/devtools";
            }
        }
    }
}
