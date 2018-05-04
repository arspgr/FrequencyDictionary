using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FrequencyDictionary.Services;
using NUnit.Framework;


namespace Tests
{
    [TestFixture]
    public class FileWriterServiceTest
    {
        [Test]
        public async Task WriteAsync_WritesExpectedFile()
        {
            var words = new Dictionary<string, int>
            {
                {"word1", 2},
                {"word", 6},
                {"word5", 8},
                {"word3", 1},
                {"word9", 9}
            };
            var fileWriter = new FileWriterService();
            var fileName = $"{AppDomain.CurrentDomain.BaseDirectory}/result.txt";
            await fileWriter.WriteAsync(words, fileName);

            var resultText = File.ReadAllText(fileName);

            Assert.AreEqual(
                $"{string.Join("\n", words.OrderByDescending(pair => pair.Value).Select(pair => $"{pair.Key},{pair.Value}"))}",
                resultText);

            File.Delete(fileName);
        }
    }
}