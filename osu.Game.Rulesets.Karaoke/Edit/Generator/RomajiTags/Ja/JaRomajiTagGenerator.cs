// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Ja;
using Lucene.Net.Analysis.TokenAttributes;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Karaoke.Objects;
using osu.Game.Rulesets.Karaoke.Utils;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.RomajiTags.Ja
{
    public class JaRomajiTagGenerator : RomajiTagGenerator<JaRomajiTagGeneratorConfig>
    {
        private readonly Analyzer analyzer;

        public JaRomajiTagGenerator(JaRomajiTagGeneratorConfig config)
            : base(config)
        {
            analyzer = Analyzer.NewAnonymous((fieldName, reader) =>
            {
                Tokenizer tokenizer = new JapaneseTokenizer(reader, null, true, JapaneseTokenizerMode.SEARCH);
                return new TokenStreamComponents(tokenizer, new JapaneseReadingFormFilter(tokenizer, false));
            });
        }

        public override RomajiTag[] CreateRomajiTags(Lyric lyric)
        {
            var text = lyric.Text;
            var processingTags = new List<RomajiTagGeneratorPatameter>();

            // Tokenize the text
            var tokenStream = analyzer.GetTokenStream("dummy", new StringReader(text));

            // Get result and offset
            var result = tokenStream.GetAttribute<ICharTermAttribute>();
            var offsetAtt = tokenStream.GetAttribute<IOffsetAttribute>();

            // Reset the stream and convert all result
            tokenStream.Reset();

            while (true)
            {
                // Read next token
                tokenStream.ClearAttributes();
                tokenStream.IncrementToken();

                // Get parsed result, result is Katakana.
                var katakana = result.ToString();
                if (string.IsNullOrEmpty(katakana))
                    break;

                var parentText = text[offsetAtt.StartOffset..offsetAtt.EndOffset];
                var fromKanji = JpStringUtils.ToKatakana(katakana) != JpStringUtils.ToKatakana(parentText);

                // Convert to romaji.
                var romaji = JpStringUtils.ToRomaji(katakana);
                if (Config.Uppercase)
                    romaji = romaji.ToUpper();

                // Make tag
                processingTags.Add(new RomajiTagGeneratorPatameter
                {
                    FromKanji = fromKanji,
                    RomajiTag = new RomajiTag
                    {
                        Text = romaji,
                        StartIndex = offsetAtt.StartOffset,
                        EndIndex = offsetAtt.EndOffset
                    }
                });
            }

            // Dispose
            tokenStream.End();
            tokenStream.Dispose();

            var romajiTags = new List<RomajiTag>();

            foreach (var processingTag in processingTags)
            {
                // combine romajies of they are not from kanji.
                var previousProcessingTag = processingTags.GetPrevious(processingTag);
                var fromKanji = processingTag.FromKanji;

                if (previousProcessingTag != null && !fromKanji)
                {
                    var combinedRomajiTag = TextTagsUtils.Combine(previousProcessingTag.RomajiTag, processingTag.RomajiTag);
                    romajiTags.Remove(previousProcessingTag.RomajiTag);
                    romajiTags.Add(combinedRomajiTag);
                }
                else
                {
                    romajiTags.Add(processingTag.RomajiTag);
                }
            }

            return romajiTags.ToArray();
        }

        internal class RomajiTagGeneratorPatameter
        {
            public bool FromKanji { get; set; }

            public RomajiTag RomajiTag { get; set; }
        }
    }
}
