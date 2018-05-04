using System.Collections.Concurrent;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using FrequencyDictionary.Services;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class LineProcessorTest
    {
        private LineProcessor _lineProcessor;
        private ConcurrentDictionary<string, int> _dict;
        [SetUp]
        public void SetUp()
        {
            _lineProcessor = new LineProcessor();
            _dict = new ConcurrentDictionary<string, int>();
        }

        [Test]
        public void CheckDictionaryNotContainsEmptyKeys()
        {
            var line = "word1 word2     word4";
            _lineProcessor.ProcessLine(line, _dict);
            Assert.True(!_dict.Keys.Any(string.IsNullOrWhiteSpace));
        }

        [Test]
        public void CheckDictionaryHasExpectedKeys()
        {
            const string line = "word word word1 1 2 1 word word3 word1";
            _lineProcessor.ProcessLine(line, _dict);
            CollectionAssert.AreEquivalent(new[] { "word", "word1", "1", "2", "word3" }, _dict.Keys);
        }

        [Test]
        public void CheckDictionaryCountsSameWords()
        {
            const string line = "word word word1 1 2 1 word word3 word1";
            var words = line.Split(' ');
            _lineProcessor.ProcessLine(line, _dict);
            foreach (var word in words)
            {
                Assert.AreEqual(GetWordCount(word, words), _dict[word]);
            }
        }

        private static int GetWordCount(string word, string[] words) => words.Count(s => s == word);
    }
}