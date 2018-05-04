using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using FrequencyDictionary.Services;
using FrequencyDictionary.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FileProcessorTest
    {
        private FileProcessor _fileProcessor;
        private Mock<ILineProcessor> _mockLineProcessor;
        private readonly string _defaultFileName = $"{AppDomain.CurrentDomain.BaseDirectory}/testFile.txt";
        [SetUp]
        public void SetUp()
        {
            _mockLineProcessor = new Mock<ILineProcessor>();
            _fileProcessor = new FileProcessor(_mockLineProcessor.Object);
        }

        [Test]
        public void Process_CheckProcessLineCalls()
        {
            const string text = "word1 word2 word3 word1\nword3 word4 word1 word2\nword5 word7";
            InFileContext(text, _defaultFileName,
                () =>
                {
                    _fileProcessor.Process(_defaultFileName);
                    _mockLineProcessor.Verify(processor =>
                            processor.ProcessLine(It.IsAny<string>(), It.IsAny<ConcurrentDictionary<string, int>>()),
                        Times.Exactly(text.Split('\n').Count(s => !string.IsNullOrEmpty(s))));
                });
        }

        [Test]
        public void GetWordsDictionary_ReturnsActualDictionary()
        {
            const string text = "word1 word2 word3 word1\nword3 word4 word1 word2\nword5 word7";
            InFileContext(text, _defaultFileName,
                () =>
                {
                    _fileProcessor = new FileProcessor(new LineProcessor());
                    _fileProcessor.Process(_defaultFileName);
                    var result = _fileProcessor.GetWordsDictionary();
                    var expectedWords = text.Split('\n').SelectMany(s => s.Split(' '))
                        .Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                    CollectionAssert.AreEquivalent(expectedWords.Distinct(), result.Keys);

                    foreach (var word in expectedWords)
                    {
                        Assert.AreEqual(GetWordCount(word, expectedWords.ToArray()), result[word]);
                    }
                });
        }

        private static int GetWordCount(string word, string[] words) => words.Count(s => s == word);

        private static void InFileContext(string text, string fileName, Action act)
        {
            using (var file = File.Create(fileName))
            {
                var bytes = Encoding.GetEncoding("Windows-1251").GetBytes(text);
                file.Write(bytes, 0, bytes.Length);
            }

            act();

            File.Delete(fileName);
        }
 
    }
}