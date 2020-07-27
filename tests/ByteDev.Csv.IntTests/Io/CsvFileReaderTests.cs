using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ByteDev.Common.Collections;
using ByteDev.Csv.Io;
using ByteDev.Testing.TestBuilders.FileSystem;
using NUnit.Framework;

namespace ByteDev.Csv.IntTests.Io
{
    [TestFixture]
    public class CsvFileReaderTests : IoTestBase
    {
        private const string CsvHeader = "Name,Surname,Age";
        private const string Line1 = "John,Smith,50";
        private const string Line2 = "Peter,Jones,30";

        private const string Line1WithWhitespace = " John ,  Smith ,  50  ";
        private const string Line2WithWhitespace = "\tPeter  , Jones\t, 30 ";

        private CsvFileReader _sut;

        private void SetupWorkingDir(string methodName)
        {
            var type = MethodBase.GetCurrentMethod().DeclaringType;
            SetWorkingDir(type, methodName);
        }

        [SetUp]
        public void Setup()
        {
            _sut = new CsvFileReader();
        }

        [TestFixture]
        public class ReadFile : CsvFileReaderTests
        {
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();
            }

            [Test]
            public void WhenFileExists_ThenReturnFile()
            {
                var fileInfo = FileTestBuilder.InFileSystem
                    .WithFilePath(Path.Combine(WorkingDir, "Test1.csv"))
                    .WithText(CsvHeader + Environment.NewLine + Line1 + Environment.NewLine + Line2)
                    .Build();

                var result = _sut.ReadFile(fileInfo.FullName, new CsvFileReaderOptions());

                Assert.That(result.HasHeader, Is.True);
                Assert.That(result.Header.ToString(), Is.EqualTo(CsvHeader));

                Assert.That(result.HasBody, Is.True);
                Assert.That(result.Body.Lines.Count, Is.EqualTo(2));
                Assert.That(result.Body.Lines[0].Line, Is.EqualTo(Line1));
                Assert.That(result.Body.Lines[1].Line, Is.EqualTo(Line2));
            }
        }

        [TestFixture]
        public class ReadHeader : CsvFileReaderTests
        {
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();
            }

            [Test]
            public void WhenFileDoesNotExist_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => _sut.ReadHeader(Path.Combine(WorkingDir, Path.GetRandomFileName())));
            }

            [Test]
            public void WhenFileIsEmpty_ThenReturnEmptyHeader()
            {
                var fileInfo = FileTestBuilder.InFileSystem
                    .WithFilePath(Path.Combine(WorkingDir, "Test1.csv"))
                    .Build();

                var result = _sut.ReadHeader(fileInfo.FullName);

                Assert.That(result, Is.Null);
            }

            [Test]
            public void WhenFileHasHeader_ThenReturnHeader()
            {
                var fileInfo = FileTestBuilder.InFileSystem
                    .WithFilePath(Path.Combine(WorkingDir, "Test1.csv"))
                    .WithText(CsvHeader)
                    .Build();

                var result = _sut.ReadHeader(fileInfo.FullName);

                Assert.That(result.Values, Is.EquivalentTo(CsvHeader.Split(',')));
            }
        }

        [TestFixture]
        public class ReadBody : CsvFileReaderTests
        {
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();
            }

            [Test]
            public void WhenFileDoesNotExist_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => Act(Path.Combine(WorkingDir, Path.GetRandomFileName())));
            }

            [Test]
            public void WhenFileIsEmpty_ThenReturnEmpty()
            {
                var fileInfo = FileTestBuilder.InFileSystem
                    .WithFilePath(Path.Combine(WorkingDir, "Test1.csv"))
                    .Build();

                var result = Act(fileInfo.FullName);

                Assert.That(result.Count, Is.EqualTo(0));
            }

            [Test]
            public void WhenFileContainsJustHeader_ThenReturnEmpty()
            {
                var fileInfo = FileTestBuilder.InFileSystem
                    .WithFilePath(Path.Combine(WorkingDir, "Test1.csv"))
                    .WithText(CsvHeader)
                    .Build();

                var result = Act(fileInfo.FullName);

                Assert.That(result.Count, Is.EqualTo(0));
            }

            [Test]
            public void WhenFileContainsHeaderAndTwoLines_ThenReturnTwoLines()
            {
                var text = CsvHeader + "\n" + Line1 + "\n" + Line2;

                var fileInfo = FileTestBuilder.InFileSystem
                    .WithFilePath(Path.Combine(WorkingDir, "Test1.csv"))
                    .WithText(text)
                    .Build();

                var result = Act(fileInfo.FullName);

                Assert.That(result.First().Values, Is.EquivalentTo(Line1.Split(',')));
                Assert.That(result.Second().Values, Is.EquivalentTo(Line2.Split(',')));
            }

            [Test]
            public void WhenLinesHaveCr_ThenReturnTwoLines()
            {
                var text = CsvHeader + "\r" + Line1 + "\r" + Line2;

                var fileInfo = FileTestBuilder.InFileSystem
                    .WithFilePath(Path.Combine(WorkingDir, "Test1.csv"))
                    .WithText(text)
                    .Build();

                var result = Act(fileInfo.FullName);

                Assert.That(result.First().Values, Is.EquivalentTo(Line1.Split(',')));
                Assert.That(result.Second().Values, Is.EquivalentTo(Line2.Split(',')));
            }

            [Test]
            public void WhenTrimIsEnabled_ThenReturnTrimmedValues()
            {
                var text = CsvHeader + "\n" + Line1WithWhitespace + "\n" + Line2WithWhitespace;

                var fileInfo = FileTestBuilder.InFileSystem
                    .WithFilePath(Path.Combine(WorkingDir, "Test1.csv"))
                    .WithText(text)
                    .Build();

                var result = Act(fileInfo.FullName, new CsvFileReaderOptions { EnableTrimValues = true });

                Assert.That(result.First().Values, Is.EquivalentTo(Line1.Split(',')));
                Assert.That(result.Second().Values, Is.EquivalentTo(Line2.Split(',')));
            }

            private IList<CsvFileLine> Act(string fileName, CsvFileReaderOptions options = null)
            {
                if (options == null)
                {
                    options = new CsvFileReaderOptions();
                }
                return _sut.ReadBody(fileName, options);
            }
        }
    }
}
