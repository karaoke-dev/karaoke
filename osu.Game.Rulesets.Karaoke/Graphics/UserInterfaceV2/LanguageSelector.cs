// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Karaoke.Graphics.Containers;
using osu.Game.Rulesets.Karaoke.Utils;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Karaoke.Graphics.UserInterfaceV2;

public partial class LanguageSelector : CompositeDrawable, IHasCurrentValue<CultureInfo?>
{
    private readonly LanguageSelectionSearchTextBox filter;
    private readonly RearrangeableLanguageListContainer languageList;

    private readonly BindableWithCurrent<CultureInfo?> current = new();

    public Bindable<CultureInfo?> Current
    {
        get => current.Current;
        set => current.Current = value;
    }

    public override bool AcceptsFocus => true;

    public override bool RequestsFocus => true;

    public LanguageSelector()
    {
        InternalChild = new GridContainer
        {
            RelativeSizeAxes = Axes.Both,
            RowDimensions = new[]
            {
                new Dimension(GridSizeMode.Absolute, 40),
                new Dimension(),
            },
            Content = new[]
            {
                new Drawable[]
                {
                    filter = new LanguageSelectionSearchTextBox
                    {
                        RelativeSizeAxes = Axes.X,
                    },
                },
                new Drawable[]
                {
                    languageList = new RearrangeableLanguageListContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        RequestSelection = item =>
                        {
                            Current.Value = item.CultureInfo;
                        },
                    },
                },
            },
        };

        filter.Current.BindValueChanged(e => languageList.Filter(e.NewValue));
        Current.BindValueChanged(e =>
        {
            // we need to wait until language list loaded.
            Schedule(() =>
            {
                languageList.SelectedSet.Value = new LanguageModel(e.NewValue);
            });
        }, true);

        reloadLanguageList();
    }

    protected override void OnFocus(FocusEvent e)
    {
        base.OnFocus(e);

        GetContainingInputManager().ChangeFocus(filter);
    }

    private bool enableEmptyOption;

    public bool EnableEmptyOption
    {
        get => enableEmptyOption;
        set
        {
            enableEmptyOption = value;

            reloadLanguageList();
        }
    }

    private void reloadLanguageList()
    {
        var languages = CultureInfoUtils.GetAvailableLanguages().Select(x => new LanguageModel(x));
        languageList.Items.Clear();

        if (EnableEmptyOption)
        {
            languageList.Items.Insert(0, new LanguageModel(null));
        }

        languageList.Items.AddRange(languages);
    }

    private partial class LanguageSelectionSearchTextBox : SearchTextBox
    {
        protected override Color4 SelectionColour => Color4.Gray;

        public LanguageSelectionSearchTextBox()
        {
            PlaceholderText = "type in keywords...";
        }
    }

    private partial class RearrangeableLanguageListContainer : RearrangeableTextFlowListContainer<LanguageModel>
    {
        protected override DrawableTextListItem CreateDrawable(LanguageModel item)
            => new DrawableLanguageListItem(item);

        private partial class DrawableLanguageListItem : DrawableTextListItem
        {
            public DrawableLanguageListItem(LanguageModel item)
                : base(item)
            {
            }

            public override IEnumerable<LocalisableString> FilterTerms
            {
                get
                {
                    var cultureInfo = Model.CultureInfo;

                    yield return new LocalisableString(CultureInfoUtils.GetLanguageDisplayText(cultureInfo));

                    if (cultureInfo == null)
                    {
                        yield return new LocalisableString(string.Empty);
                    }
                    else
                    {
                        yield return new LocalisableString(cultureInfo.Name);
                        yield return new LocalisableString(cultureInfo.DisplayName);
                        yield return new LocalisableString(cultureInfo.EnglishName);
                    }
                }
            }

            protected override void CreateDisplayContent(OsuTextFlowContainer textFlowContainer, LanguageModel model)
            {
                textFlowContainer.AddText(CultureInfoUtils.GetLanguageDisplayText(model.CultureInfo));
            }
        }
    }

    /// <summary>
    ///  use this struct to warp the <see cref="CultureInfo"/> because <see cref="RearrangeableLanguageListContainer"/> is not able support null value.
    /// </summary>
    private struct LanguageModel
    {
        public LanguageModel(CultureInfo? cultureInfo)
        {
            CultureInfo = cultureInfo;
        }

        public CultureInfo? CultureInfo { get; }
    }
}
