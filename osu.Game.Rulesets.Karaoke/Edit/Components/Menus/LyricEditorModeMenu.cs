// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics;

namespace osu.Game.Rulesets.Karaoke.Edit.Components.Menus
{
    public class LyricEditorModeMenu : EnumMenu<LyricEditorMode>
    {
        public LyricEditorModeMenu(Bindable<LyricEditorMode> config, string text)
            : base(config, text)
        {
        }

        protected override LyricEditorMode[] ValidEnums => new[]
        {
            LyricEditorMode.View,
            LyricEditorMode.Manage,
            LyricEditorMode.Typing,
            LyricEditorMode.Language,
            LyricEditorMode.EditRuby,
            LyricEditorMode.EditRomaji,
            LyricEditorMode.CreateTimeTag,
            LyricEditorMode.EditNote,
            LyricEditorMode.Singer,
        };

        protected override string GetName(LyricEditorMode selection)
        {
            switch (selection)
            {
                case LyricEditorMode.View:
                    return "View";

                case LyricEditorMode.Manage:
                    return "Manage lyrics";

                case LyricEditorMode.Typing:
                    return "Typing";

                case LyricEditorMode.Language:
                    return "Select language";

                case LyricEditorMode.EditRuby:
                    return "Edit ruby";

                case LyricEditorMode.EditRomaji:
                    return "Edit romaji";

                case LyricEditorMode.CreateTimeTag:
                case LyricEditorMode.RecordTimeTag:
                case LyricEditorMode.AdjustTimeTag:
                    return "Edit time tag";

                case LyricEditorMode.EditNote:
                    return "Edit note";

                case LyricEditorMode.Singer:
                    return "Select singer";

                default:
                    throw new ArgumentOutOfRangeException(nameof(selection));
            }
        }
    }
}
