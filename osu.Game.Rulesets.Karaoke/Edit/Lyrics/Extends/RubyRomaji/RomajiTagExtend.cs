// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Karaoke.Edit.Lyrics.States.Modes;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics.Extends.RubyRomaji
{
    public class RomajiTagExtend : TextTagExtend
    {
        public RomajiTagExtend()
        {
            EditMode.BindValueChanged(e =>
            {
                switch (e.NewValue)
                {
                    case TextTagEditMode.Generate:
                        Children = new Drawable[]
                        {
                            new RomajiTagEditModeSection(),
                            new RomajiTagAutoGenerateSection(),
                        };
                        break;

                    case TextTagEditMode.Edit:
                        Children = new Drawable[]
                        {
                            new RomajiTagEditModeSection(),
                            new RomajiTagEditSection(),
                        };
                        break;

                    case TextTagEditMode.Verify:
                        Children = new Drawable[]
                        {
                            new RomajiTagEditModeSection(),
                            new RomajiTagIssueSection(),
                        };
                        break;

                    default:
                        return;
                }
            }, true);
        }

        [BackgroundDependencyLoader]
        private void load(IEditRomajiModeState romajiModeState)
        {
            EditMode.BindTo(romajiModeState.BindableEditMode);
        }
    }
}
