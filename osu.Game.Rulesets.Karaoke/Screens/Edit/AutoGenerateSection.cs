// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit;

public abstract partial class AutoGenerateSection : EditorSection
{
    protected sealed override LocalisableString Title => "Auto generate";

    protected AutoGenerateSection()
    {
        Children = new[]
        {
            CreateAutoGenerateSubsection(),
        };
    }

    protected abstract AutoGenerateSubsection CreateAutoGenerateSubsection();
}
