// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Globalization;
using osu.Framework.Bindables;
using osu.Game.Graphics.UserInterfaceV2;

namespace osu.Game.Rulesets.Karaoke.Edit.Setup.Components;

public partial class LabelledLanguageList : LabelledDrawable<LanguageList>
{
    public LabelledLanguageList()
        : base(true)
    {
    }

    public BindableList<CultureInfo> Languages => Component.Languages;

    protected override LanguageList CreateComponent() => new();
}
