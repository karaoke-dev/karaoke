// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Ja;
using Lucene.Net.Analysis.TokenAttributes;
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

        public override RomajiTag[] CreateRubyTags(Lyric lyric)
        {
            var text = lyric.Text;
            var tags = new List<RomajiTag>();

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

                // Convert to romaji.
                var romaji = JpStringUtils.ToRomaji(katakana);
                if (Config.Uppercase)
                    romaji = romaji.ToUpper();


                // Make tag
                tags.Add(new RomajiTag
                {
                    Text = romaji,
                    StartIndex = offsetAtt.StartOffset,
                    EndIndex = offsetAtt.EndOffset
                });
            }

            // Dispose
            tokenStream.End();
            tokenStream.Dispose();

            return tags.ToArray();
        }
    }
}
