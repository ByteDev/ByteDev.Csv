using System;
using System.Collections.Generic;
using ByteDev.Csv.Io;
using NUnit.Framework;

namespace ByteDev.Csv.UnitTests.Io
{
    [TestFixture]
    public class CsvFileWriterTests
    {
        private CsvFileWriter _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CsvFileWriter();
        }

        [TestFixture]
        public class WriteHeader : CsvFileWriterTests
        {
            [Test]
            public void WhenFileIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.WriteHeader(null, new CsvFileLine("name")));
            }

            [Test]
            public void WhenHeaderIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.WriteHeader("file.csv", null));
            }   
        }

        [TestFixture]
        public class AppendLine : CsvFileWriterTests
        {
            [Test]
            public void WhenFileIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.AppendLine(null, new CsvFileLine("name")));
            }

            [Test]
            public void WhenHeaderIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.AppendLine("file.csv", null));
            }
        }

        [TestFixture]
        public class AppendLines : CsvFileWriterTests
        {
            [Test]
            public void WhenFileIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.AppendLines(null, new List<CsvFileLine>()));
            }

            [Test]
            public void WhenHeaderIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.AppendLines("file.csv", null));
            }
        }
    }
}