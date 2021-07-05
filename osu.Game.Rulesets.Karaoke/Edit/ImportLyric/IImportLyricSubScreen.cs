// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public interface IImportLyricSubScreen : IScreen
    {
        ImportLyricStep Step { get; }

        string Title { get; }

        string ShortTitle { get; }

        IconUsage Icon { get; }

        void CanRollBack(IImportLyricSubScreen rollBackScreen, Action<bool> callBack);
    }
}
