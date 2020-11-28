// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the GPL Licence.
// See the LICENCE file in the repository root for full licence text.

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.OpenNlp;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Util;
using System.IO;

namespace osu.Game.Rulesets.Karaoke.Edit.Generator.Languages
{
    public class LanguageDetector
    {
        public LanguageDetector()
        {
            // this is copied from :
            // https://fabian-kostadinov.github.io/2018/09/08/introduction-to-lucene-opennlp-part1/
            var version = LuceneVersion.LUCENE_48;

            var text = "The quick brown fox jumped over the lazy dogs.";

            var analyzer = new StandardAnalyzer(version);

            // We're using a string reader to return a token stream. This allows us to observe the tokens
            // while they are being processed by our analyzer.
            TokenStream stream = analyzer.GetTokenStream("field", new StringReader(text));

            // CharTermAttribute will contain the actual token word
            CharTermAttribute termAtt = stream.AddAttribute<CharTermAttribute>();

            // TypeAttribute will contain the OpenNLP POS (Treebank II) tag
            TypeAttribute typeAtt = stream.AddAttribute<TypeAttribute>();

            try
            {
                stream.Reset();

                // Print all tokens until stream is exhausted
                while (stream.IncrementToken()) {
                    var result = termAtt.ToString() + ": " + typeAtt.Type;
                }

                stream.End();

            }
            finally
            {
                stream.Dispose();
            }
        }

        /*
        public class OpenNLPAnalyzer : Analyzer
        {
            protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
            {
                Tokenizer tokenizer = new OpenNLPTokenizer(reader);

                TokenFilter tokenFilter = new OpenNLPPOSFilter();
                return new TokenStreamComponents(tokenizer, tokenFilter);
            }
        }
        */
    }
}
