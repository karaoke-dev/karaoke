// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Extensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Skinning.Default
{
    /// <summary>
    /// This component is focusing on display ruby and romaji.
    /// </summary>
    public class DefaultLyricPiece : DefaultLyricPiece<LyricSpriteText>
    {
        private const int whole_chunk_index = -1;

        public DefaultLyricPiece(Lyric hitObject, int chunkIndex = whole_chunk_index)
            : base(hitObject, chunkIndex)
        {
        }
    }

    /// <summary>
    /// This component is focusing on display ruby and romaji.
    /// </summary>
    public abstract class DefaultLyricPiece<T> : KaraokeSpriteText<T> where T : LyricSpriteText, new()
    {
        private const int whole_chunk_index = -1;

        public readonly IBindable<string> TextBindable = new Bindable<string>();
        public readonly IBindable<TimeTag[]> TimeTagsBindable = new Bindable<TimeTag[]>();
        public readonly IBindable<RubyTag[]> RubyTagsBindable = new Bindable<RubyTag[]>();
        public readonly IBindable<RomajiTag[]> RomajiTagsBindable = new Bindable<RomajiTag[]>();

        private readonly Lyric hitObject;
        private readonly int chunkIndex;

        protected DefaultLyricPiece(Lyric hitObject, int chunkIndex = whole_chunk_index)
        {
            this.hitObject = hitObject;
            this.chunkIndex = chunkIndex;

            TextBindable.BindValueChanged(text => applyText(text.NewValue));
            TimeTagsBindable.BindValueChanged(timeTags => applyTimeTag(timeTags.NewValue));
            RubyTagsBindable.BindValueChanged(rubyTags => applyRuby(rubyTags.NewValue));
            RubyTagsBindable.BindArrayChanged(addItems =>
            {
                foreach (var obj in addItems)
                {
                    obj.StartIndexBindable.BindValueChanged(_ => applyRuby(RubyTagsBindable.Value));
                    obj.EndIndexBindable.BindValueChanged(_ => applyRuby(RubyTagsBindable.Value));
                    obj.TextBindable.BindValueChanged(_ => applyRuby(RubyTagsBindable.Value));
                }
            }, removedItems =>
            {
                foreach (var obj in removedItems)
                {
                    obj.StartIndexBindable.UnbindEvents();
                    obj.EndIndexBindable.UnbindEvents();
                    obj.TextBindable.UnbindEvents();
                }
            });
            RomajiTagsBindable.BindValueChanged(romajiTags => applyRomaji(romajiTags.NewValue));
            RomajiTagsBindable.BindArrayChanged(addItems =>
            {
                foreach (var obj in addItems)
                {
                    obj.StartIndexBindable.BindValueChanged(_ => applyRomaji(RomajiTagsBindable.Value));
                    obj.EndIndexBindable.BindValueChanged(_ => applyRomaji(RomajiTagsBindable.Value));
                    obj.TextBindable.BindValueChanged(_ => applyRomaji(RomajiTagsBindable.Value));
                }
            }, removedItems =>
            {
                foreach (var obj in removedItems)
                {
                    obj.StartIndexBindable.UnbindEvents();
                    obj.EndIndexBindable.UnbindEvents();
                    obj.TextBindable.UnbindEvents();
                }
            });

            TextBindable.BindTo(hitObject.TextBindable);
            TimeTagsBindable.BindTo(hitObject.TimeTagsBindable);
            RubyTagsBindable.BindTo(hitObject.RubyTagsBindable);
            RomajiTagsBindable.BindTo(hitObject.RomajiTagsBindable);
        }

        private void applyText(string text)
        {
            if (chunkIndex == whole_chunk_index)
            {
                Text = text;
            }
            else
            {
                throw new NotImplementedException("Chunk lyric will be available until V2");
            }
        }

        private void applyTimeTag(TimeTag[] timeTags)
        {
            if (chunkIndex == whole_chunk_index)
            {
                TimeTags = TimeTagsUtils.ToDictionary(timeTags);
            }
            else
            {
                throw new NotImplementedException("Chunk lyric will be available until V2");
            }
        }

        private void applyRuby(IEnumerable<RubyTag> rubyTags)
        {
            if (chunkIndex == whole_chunk_index)
            {
                Rubies = DisplayRuby ? rubyTags?.Select(x => new PositionText(x.Text, x.StartIndex, x.EndIndex)).ToArray() : null;
            }
            else
            {
                throw new NotImplementedException("Chunk lyric will be available until V2");
            }
        }

        private void applyRomaji(IEnumerable<RomajiTag> romajiTags)
        {
            if (chunkIndex == whole_chunk_index)
            {
                Romajies = DisplayRomaji ? romajiTags?.Select(x => new PositionText(x.Text, x.StartIndex, x.EndIndex)).ToArray() : null;
            }
            else
            {
                throw new NotImplementedException("Chunk lyric will be available until V2");
            }
        }

        private bool displayRuby = true;

        public bool DisplayRuby
        {
            get => displayRuby;
            set
            {
                if (displayRuby == value)
                    return;

                displayRuby = value;
                Schedule(() => applyRuby(hitObject.RubyTags));
            }
        }

        private bool displayRomaji = true;

        public bool DisplayRomaji
        {
            get => displayRomaji;
            set
            {
                if (displayRomaji == value)
                    return;

                displayRomaji = value;
                Schedule(() => applyRomaji(hitObject.RomajiTags));
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            TextBindable.UnbindFrom(hitObject.TextBindable);
            TimeTagsBindable.UnbindFrom(hitObject.TimeTagsBindable);
            RubyTagsBindable.UnbindFrom(hitObject.RubyTagsBindable);
            RomajiTagsBindable.UnbindFrom(hitObject.RomajiTagsBindable);

            base.Dispose(isDisposing);
        }
    }
}
