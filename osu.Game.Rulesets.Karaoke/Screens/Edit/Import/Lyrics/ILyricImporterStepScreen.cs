// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

namespace osu.Game.Rulesets.Karaoke.Screens.Edit.Import.Lyrics;

public interface ILyricImporterStepScreen : IScreen
{
    string Title { get; }

    IconUsage Icon { get; }

    void ConfirmRollBackFromStep(ILyricImporterStepScreen fromScreen, Action<bool> callBack);
}
