using System;
using System.IO;
using FrequencyDictionary.Validators;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FileExistsValidatorTest
    {
        private FileExistsValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new FileExistsValidator();
        }

        [Test]
        public void FileExists_Success()
        {
            var fileName = $"{AppDomain.CurrentDomain.BaseDirectory}/test.txt";
            using (File.Create(fileName))
                Assert.DoesNotThrow(() => _validator.CheckFileExists(fileName));
            File.Delete(fileName);
        }

        [Test]
        public void FileNotExists_ThrowsException()
        {
            var fileName = $"{AppDomain.CurrentDomain.BaseDirectory}/{nameof(FileNotExists_ThrowsException)}.txt";
            Assert.Throws<FileNotFoundException>(() => _validator.CheckFileExists(fileName));
        }
    }
}
