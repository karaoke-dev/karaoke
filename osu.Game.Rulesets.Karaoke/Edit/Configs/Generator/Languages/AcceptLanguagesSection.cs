// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Edit.Generator.Languages;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Configs.Generator.Languages
{
    public class AcceptLanguagesSection : GeneratorConfigSection<LanguageDetectorConfig>
    {
        protected override string Title => "Accept languages";

        private readonly Bindable<CultureInfo[]> bindableCultureInfo = new Bindable<CultureInfo[]>();

        public AcceptLanguagesSection(Bindable<LanguageDetectorConfig> current)
            : base(current)
        {
            bindableCultureInfo.BindArrayChanged(n =>
            {
                foreach (var cultureInfo in n)
                {
                    Add(new RemovableLabelledLanguageSelector
                    {
                        Current =
                        {
                            Value = cultureInfo,
                            Disabled = true
                        }
                    });
                }
            }, r =>
            {
                RemoveAll(d =>
                {
                    if (d is RemovableLabelledLanguageSelector selector)
                        return r.Contains(selector.Current.Value);

                    return false;
                });
            });

            Add(new CreateLanguageButton
            {
                Text = "Add new language",
                LanguageSelected = e =>
                {
                    var languageList = bindableCultureInfo.Value?.ToList() ?? new List<CultureInfo>();
                    if (languageList.Contains(e))
                        return;

                    languageList.Add(e);
                    bindableCultureInfo.Value = languageList.ToArray();
                }
            });

            RegisterConfig(bindableCultureInfo, nameof(LanguageDetectorConfig.AcceptLanguages));
        }

        public class RemovableLabelledLanguageSelector : LabelledLanguageSelector
        {
            // todo : add delete button.
        }

        public class CreateLanguageButton : OsuButton
        {
            [Resolved]
            protected LanguageSelectionDialog LanguageSelectionDialog { get; private set; }

            private readonly Bindable<CultureInfo> current = new Bindable<CultureInfo>();

            public Action<CultureInfo> LanguageSelected;

            public CreateLanguageButton()
            {
                RelativeSizeAxes = Axes.X;
                Content.CornerRadius = 15;

                Action = () =>
                {
                    LanguageSelectionDialog.Current = current;
                    LanguageSelectionDialog.Show();
                };

                current.BindValueChanged(e => LanguageSelected?.Invoke(e.NewValue));
            }
        }
    }
}
