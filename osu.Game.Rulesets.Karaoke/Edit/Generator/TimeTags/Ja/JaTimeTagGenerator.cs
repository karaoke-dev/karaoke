// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.TimeTags.Ja
{
    public class JaTimeTagGenerator : TimeTagGenerator<JaTimeTagGeneratorConfig>
    {
        public JaTimeTagGenerator(JaTimeTagGeneratorConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// Thanks to RhythmKaTTE's author writing this logic into C#
        /// http://juna-idler.blogspot.com/2016/05/rhythmkatte-version-01.html
        /// </summary>
        protected override void TimeTagLogic(Lyric lyric, List<Tuple<TimeTagIndex, double?>> timeTags)
        {
            var text = lyric.Text;

            for (var i = 1; i < text.Length; i++)
            {
                var timeTag = TimeTagsUtils.Create(new TimeTagIndex(i, TimeTagIndex.IndexState.Start), null);

                var c = text[i];
                var pc = text[i - 1];

                if (char.IsWhiteSpace(c) && Config.CheckWhiteSpace)
                {
                    // 空白文字の連続は無条件で無視 (Unconditionally ignore a series of whitespace characters)
                    if (char.IsWhiteSpace(pc))
                        continue;

                    if (CharUtils.IsLatin(pc))
                    {
                        if (Config.CheckWhiteSpaceAlphabet)
                            timeTags.Add(timeTag);
                    }
                    else if (char.IsDigit(pc))
                    {
                        if (Config.CheckWhiteSpaceDigit)
                            timeTags.Add(timeTag);
                    }
                    else if (CharUtils.IsAsciiSymbol(pc))
                    {
                        if (Config.CheckWhiteSpaceAsciiSymbol)
                            timeTags.Add(timeTag);
                    }
                    else
                    {
                        timeTags.Add(timeTag);
                    }
                }
                else if (CharUtils.IsLatin(c) || char.IsNumber(c) || CharUtils.IsAsciiSymbol(c))
                {
                    if (char.IsWhiteSpace(pc) || !CharUtils.IsLatin(pc) && !char.IsNumber(pc) && !CharUtils.IsAsciiSymbol(pc))
                    {
                        timeTags.Add(timeTag);
                    }
                }
                else if (char.IsWhiteSpace(pc))
                {
                    timeTags.Add(timeTag);
                }
                else
                {
                    switch (c)
                    {
                        case 'ゃ':
                        case 'ゅ':
                        case 'ょ':
                        case 'ャ':
                        case 'ュ':
                        case 'ョ':
                        case 'ぁ':
                        case 'ぃ':
                        case 'ぅ':
                        case 'ぇ':
                        case 'ぉ':
                        case 'ァ':
                        case 'ィ':
                        case 'ゥ':
                        case 'ェ':
                        case 'ォ':
                        case 'ー':
                        case '～':
                            break;

                        case 'ん':
                            if (Config.Checkん)
                            {
                                timeTags.Add(timeTag);
                            }

                            break;

                        case 'っ':
                            if (Config.Checkっ)
                            {
                                timeTags.Add(timeTag);
                            }

                            break;

                        default:
                            timeTags.Add(timeTag);
                            break;
                    }
                }
            }
        }
    }
}
