// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja;
using osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Zh;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Screens.Edit;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Karaoke.Edit.Lyrics
{
    /// <summary>
    /// Handle view or edit time-tag in lyrics.
    /// Notice that <see cref="TimeTagManager"/> is not strictly needed, <see cref="LyricEditor"/> just not showing time-tag if not regist this manager.
    /// </summary>
    public class TimeTagManager : Component
    {
        [Resolved]
        private EditorBeatmap beatmap { get; set; }

        [Resolved(CanBeNull = true)]
        private IEditorChangeHandler changeHandler { get; set; }

        public Bindable<Tuple<TimeTagIndex, double?>> BindableCursorPosition { get; set; } = new Bindable<Tuple<TimeTagIndex, double?>>();

        /// <summary>
        /// Will auto-detect each <see cref="Lyric"/> 's <see cref="Lyric.TimeTags"/> and apply on them.
        /// </summary>
        public void AutoGenerateTimeTags()
        {
            var lyrics = beatmap.HitObjects.OfType<Lyric>().ToList();
            if (!lyrics.Any())
                return;

            changeHandler?.BeginChange();

            var selector = new TimeTagGeneratorSelector();

            foreach (var lyric in lyrics)
            {
                var timeTags = selector.GenerateTimeTags(lyric);
                lyric.TimeTags = timeTags;
            }

            changeHandler?.EndChange();
        }

        public bool MoveCursor(CursorAction action)
        {
            var currentTimeTag = BindableCursorPosition.Value;

            Tuple<TimeTagIndex, double?> nextTimeTag = null;
            switch (action)
            {
                case CursorAction.MoveUp:
                    nextTimeTag = getPreviousLyricTimeTag(currentTimeTag);
                    break;
                case CursorAction.MoveDown:
                    nextTimeTag = getNextLyricTimeTag(currentTimeTag);
                    break;
                case CursorAction.MoveLeft:
                    nextTimeTag = getPreviousTimeTag(currentTimeTag);
                    break;
                case CursorAction.MoveRight:
                    nextTimeTag = getNextTimeTag(currentTimeTag);
                    break;
                case CursorAction.First:
                    nextTimeTag = getFirstTimeTag(currentTimeTag);
                    break;
                case CursorAction.Last:
                    nextTimeTag = getLastTimeTag(currentTimeTag);
                    break;
            }

            if (nextTimeTag == null)
                return false;

            moveCursorTo(nextTimeTag);
            return true;
        }

        public bool MoveCursorToTargetPosition(Tuple<TimeTagIndex, double?> timeTag)
        {
            if (timeTagInLyric(timeTag) == null)
                return false;

            moveCursorTo(timeTag);
            return true;
        }

        private Lyric timeTagInLyric(Tuple<TimeTagIndex, double?> timeTag)
        {
            if (timeTag == null)
                return null;

            return beatmap.HitObjects.OfType<Lyric>().FirstOrDefault(x => x.TimeTags?.Contains(timeTag) ?? false);
        }

        private Tuple<TimeTagIndex, double?> getPreviousLyricTimeTag(Tuple<TimeTagIndex, double?> timeTag)
        {
            // todo : need to implement
            return null;
        }

        public Tuple<TimeTagIndex, double?> getNextLyricTimeTag(Tuple<TimeTagIndex, double?> timeTag)
        {
            // todo : need to implement
            return null;
        }

        private Tuple<TimeTagIndex, double?> getPreviousTimeTag(Tuple<TimeTagIndex, double?> timeTag)
        {
            var timeTags = beatmap.HitObjects.OfType<Lyric>().SelectMany(x => x.TimeTags).ToArray();
            return timeTags.GetPrevious(timeTag);
        }

        public Tuple<TimeTagIndex, double?> getNextTimeTag(Tuple<TimeTagIndex, double?> timeTag)
        {
            var timeTags = beatmap.HitObjects.OfType<Lyric>().SelectMany(x => x.TimeTags).ToArray();
            return timeTags.GetNext(timeTag);
        }

        private Tuple<TimeTagIndex, double?> getFirstTimeTag(Tuple<TimeTagIndex, double?> timeTag)
        {
            var timeTags = beatmap.HitObjects.OfType<Lyric>().SelectMany(x => x.TimeTags).ToArray();
            return timeTags.FirstOrDefault();
        }

        public Tuple<TimeTagIndex, double?> getLastTimeTag(Tuple<TimeTagIndex, double?> timeTag)
        {
            var timeTags = beatmap.HitObjects.OfType<Lyric>().SelectMany(x => x.TimeTags).ToArray();
            return timeTags.LastOrDefault();
        }

        private void moveCursorTo(Tuple<TimeTagIndex, double?> timeTag)
        {
            if (timeTag == null)
                return;

            BindableCursorPosition.Value = timeTag;
        }

        public class TimeTagGeneratorSelector
        {
            private readonly Lazy<JaTimeTagGenerator> jaTimeTagGenerator;
            private readonly Lazy<ZhTimeTagGenerator> zhTimeTagGenerator;

            public TimeTagGeneratorSelector()
            {
                jaTimeTagGenerator = new Lazy<JaTimeTagGenerator>(() =>
                {
                    // todo : get config from setting.
                    var config = new JaTimeTagGeneratorConfig();
                    return new JaTimeTagGenerator(config);
                });
                zhTimeTagGenerator = new Lazy<ZhTimeTagGenerator>(() =>
                {
                    // todo : get config from setting.
                    var config = new ZhTimeTagGeneratorConfig();
                    return new ZhTimeTagGenerator(config);
                });
            }

            public Tuple<TimeTagIndex, double?>[] GenerateTimeTags(Lyric lyric)
            {
                // lazy to generate language detector and apply it's setting
                switch (lyric.Language?.LCID)
                {
                    case 17:
                    case 1041:
                        return jaTimeTagGenerator.Value.CreateTimeTags(lyric);

                    case 1028:
                        return zhTimeTagGenerator.Value.CreateTimeTags(lyric);

                    default:
                        return null;
                }
            }
        }
    }

    public enum CursorAction
    {
        MoveUp,

        MoveDown,

        MoveLeft,

        MoveRight,

        First,

        Last,
    }
}
