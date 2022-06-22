// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Graphics.Sprites
{
    public class DrawableLyricSpriteText : LyricSpriteText
    {
        private readonly IBindable<string> textBindable = new Bindable<string>();
        private readonly IBindable<int> rubyTagsVersion = new Bindable<int>();
        private readonly IBindableList<RubyTag> rubyTagsBindable = new BindableList<RubyTag>();
        private readonly IBindable<int> romajiTagsVersion = new Bindable<int>();
        private readonly IBindableList<RomajiTag> romajiTagsBindable = new BindableList<RomajiTag>();

        public DrawableLyricSpriteText(Lyric lyric)
        {
            textBindable.BindValueChanged(text => { Text = text.NewValue; }, true);
            rubyTagsVersion.BindValueChanged(_ => updateRubies());
            rubyTagsBindable.BindCollectionChanged((_, _) => updateRubies());
            romajiTagsVersion.BindValueChanged(_ => updateRubies());
            romajiTagsBindable.BindCollectionChanged((_, _) => updateRomajies());

            textBindable.BindTo(lyric.TextBindable);
            rubyTagsVersion.BindTo(lyric.RubyTagsVersion);
            rubyTagsBindable.BindTo(lyric.RubyTagsBindable);
            romajiTagsVersion.BindTo(lyric.RomajiTagsVersion);
            romajiTagsBindable.BindTo(lyric.RomajiTagsBindable);
        }

        private void updateRubies()
        {
            Rubies = rubyTagsBindable.Select(TextTagUtils.ToPositionText).ToArray();
        }

        private void updateRomajies()
        {
            Romajies = romajiTagsBindable.Select(TextTagUtils.ToPositionText).ToArray();
        }
    }
}
