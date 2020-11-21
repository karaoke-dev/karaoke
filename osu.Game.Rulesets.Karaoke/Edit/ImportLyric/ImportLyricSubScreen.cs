// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Screens;

namespace osu.Game.Rulesets.Karaoke.Edit.ImportLyric
{
    public abstract class ImportLyricSubScreen : OsuScreen, IImportLyricSubScreen
    {
        [Cached]
        private readonly Bindable<ImportLyricStep> Step = new Bindable<ImportLyricStep>();

        public abstract string ShortTitle { get; }
    }
}
