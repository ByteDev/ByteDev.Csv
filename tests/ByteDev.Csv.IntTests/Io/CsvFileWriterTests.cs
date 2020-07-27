using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ByteDev.Common.Collections;
using ByteDev.Csv.Io;
using NUnit.Framework;

namespace ByteDev.Csv.IntTests.Io
{
    [TestFixture]
    public class CsvFileWriterTests : IoTestBase
    {
        private static readonly CsvFileLine Header = new CsvFileLine("Name,Surname,Age");
        private static readonly CsvFileLine NewHeader = new CsvFileLine("First Name,Second Name,IQ");

        private static readonly CsvFileLine Line1 = new CsvFileLine("John1,Smith1,30");
        private static readonly CsvFileLine Line2 = new CsvFileLine("John2,Smith2,31");

        private CsvFileWriter _sut;

        private ICsvFileReader _csvFileReader;
        private string _file;

        private void SetupWorkingDir(string methodName)
        {
            var type = MethodBase.GetCurrentMethod().DeclaringType;
            SetWorkingDir(type, methodName);
        }

        private static void AssertBodyFirstLine(CsvFileBody body)
        {
            Assert.That(body.Lines.First().Line, Is.EqualTo(Line1.Line));
        }

        private static void AssertBodySecondLine(CsvFileBody body)
        {
            Assert.That(body.Lines.Second().Line, Is.EqualTo(Line2.Line));
        }

        private CsvFile ReadCsvFile(bool hasHeader = true)
        {
            return _csvFileReader.ReadFile(_file, new CsvFileReaderOptions { HasHeader = hasHeader });
        }

        [SetUp]
        public void SetUp()
        {
            _csvFileReader = new CsvFileReader();

            _sut = new CsvFileWriter();
        }

        [TestFixture]
        public class Write : CsvFileWriterTests
        {
            [SetUp]
            public new void SetUp()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();

                _file = Path.Combine(WorkingDir, "Write.csv");
            }

            [Test]
            public void WhenFileNotExists_ThenHeaderAndBodyWritten()
            {
                var csvFile = new CsvFile(Header, new CsvFileBody(new List<CsvFileLine> {Line1, Line2}));

                Act(csvFile);

                var actual = ReadCsvFile();

                Assert.That(actual.Header.Line, Is.EqualTo(Header.Line));
                Assert.That(actual.Body.Lines.First().Line, Is.EqualTo(Line1.Line));
                Assert.That(actual.Body.Lines.Second().Line, Is.EqualTo(Line2.Line));
            }

            [Test]
            public void WhenFileExists_ThenReplaceFile()
            {
                _sut.WriteHeader(_file, Header);
                _sut.AppendLine(_file, Line1);

                var csvFile = new CsvFile(Header, new CsvFileBody(new List<CsvFileLine> {Line1, Line2}));

                Act(csvFile);

                var actual = ReadCsvFile();

                Assert.That(actual.Header.Line, Is.EqualTo(Header.Line));
                Assert.That(actual.Body.Lines.First().Line, Is.EqualTo(Line1.Line));
                Assert.That(actual.Body.Lines.Second().Line, Is.EqualTo(Line2.Line));
            }

            private void Act(CsvFile csvFile)
            {
                _sut.Write(_file, csvFile);
            }
        }
        
        [TestFixture]
        public class WriteHeader : CsvFileWriterTests
        {
            [SetUp]
            public new void SetUp()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();

                _file = Path.Combine(WorkingDir, "WriteHeader.csv");
            }

            [Test]
            public void WhenFileNotExists_ThenHeaderIsWritten()
            {
                Act(Header);

                var csvFile = ReadCsvFile();

                Assert.That(csvFile.Header.Line, Is.EqualTo(Header.Line));
            }

            [Test]
            public void WhenFileExists_ThenReplaceTheHeader()
            {
                _sut.WriteHeader(_file, Header);
                _sut.AppendLine(_file, Line1);
                _sut.AppendLine(_file, Line2);

                Act(NewHeader);

                var csvFile = ReadCsvFile();

                Assert.That(csvFile.Header.Line, Is.EqualTo(NewHeader.Line));
                Assert.That(csvFile.Body.Lines.First().Line, Is.EqualTo(Line1.Line));
                Assert.That(csvFile.Body.Lines.Second().Line, Is.EqualTo(Line2.Line));
            }

            private void Act(CsvFileLine header)
            {
                _sut.WriteHeader(_file, header);
            }
        }

        [TestFixture]
        public class WriteBody : CsvFileWriterTests
        {
            [SetUp]
            public new void SetUp()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();

                _file = Path.Combine(WorkingDir, "Write.csv");
            }

            [Test]
            public void WhenFileNotExists_ThenBodyIsWritten()
            {
                Act(new List<CsvFileLine> { Line1, Line2 });

                var csvFile = ReadCsvFile(false);

                Assert.That(csvFile.Body.Lines.First().Line, Is.EqualTo(Line1.Line));
                Assert.That(csvFile.Body.Lines.Second().Line, Is.EqualTo(Line2.Line));
            }

            [Test]
            public void WhenFileExists_ThenReplaceBody()
            {
                _sut.WriteHeader(_file, Header);
                _sut.AppendLine(_file, Line1);
                _sut.AppendLine(_file, Line2);

                Act(new List<CsvFileLine> { Line1 });

                var csvFile = ReadCsvFile();

                Assert.That(csvFile.Header.Line, Is.EqualTo(Header.Line));
                Assert.That(csvFile.Body.Lines.Single().Line, Is.EqualTo(Line1.Line));
            }

            private void Act(IList<CsvFileLine> lines)
            {
                Act(new CsvFileBody(lines));
            }

            private void Act(CsvFileBody body)
            {
                _sut.WriteBody(_file, body);
            }
        }

        [TestFixture]
        public class AppendLine : CsvFileWriterTests
        {
            [SetUp]
            public new void SetUp()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();

                _file = Path.Combine(WorkingDir, "AppendLine.csv");
            }

            [Test]
            public void WhenFileNotExists_ThenWriteNewLine()
            {
                Act(Line1);

                var file = ReadCsvFile(false);

                AssertBodyFirstLine(file.Body);
            }

            [Test]
            public void WhenFileExists_ThenAppendNewLine()
            {
                _sut.AppendLine(_file, Line1);

                Act(Line2);

                var file = ReadCsvFile(false);

                AssertBodyFirstLine(file.Body);
                AssertBodySecondLine(file.Body);
            }

            private void Act(CsvFileLine line)
            {
                _sut.AppendLine(_file, line);
            }
        }

        [TestFixture]
        public class AppendLines : CsvFileWriterTests
        {
            [SetUp]
            public new void SetUp()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();

                _file = Path.Combine(WorkingDir, "AppendLines.csv");
            }

            [Test]
            public void WhenFileNotExists_ThenWriteLines()
            {
                var lines = new List<CsvFileLine> { Line1, Line2 };

                Act(lines);

                var file = ReadCsvFile(false);

                AssertBodyFirstLine(file.Body);
                AssertBodySecondLine(file.Body);
            }

            [Test]
            public void WhenFileExists_ThenAppendLines()
            {
                _sut.WriteHeader(_file, Header);

                Act(new List<CsvFileLine> { Line1, Line2 });

                var file = ReadCsvFile();

                AssertBodyFirstLine(file.Body);
                AssertBodySecondLine(file.Body);
            }

            private void Act(IList<CsvFileLine> lines)
            {
                _sut.AppendLines(_file, lines);
            }
        }
    }
}