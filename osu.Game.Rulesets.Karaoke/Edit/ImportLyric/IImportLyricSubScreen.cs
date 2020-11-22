// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Sprites;
using System;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public interface IImportLyricSubScreen
    {
        ImportLyricStep Step { get; }

        string Title { get; }

        string ShortTitle { get; }

        IconUsage Icon { get; }

        void CanRollBack(IImportLyricSubScreen rollBackScreen, Action<bool> callBack);
    }
}
