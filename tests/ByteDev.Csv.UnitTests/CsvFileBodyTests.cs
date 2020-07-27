using NUnit.Framework;

namespace ByteDev.Csv.UnitTests
{
    [TestFixture]
    public class CsvFileBodyTests
    {
        private CsvFileBody _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CsvFileBody();
        }

        [TestFixture]
        public class Lines : CsvFileBodyTests
        {
            [Test]
            public void WhenSetToNull_ThenReturnEmpty()
            {
                _sut.Lines = null;

                var result = _sut.Lines;

                Assert.That(result, Is.Empty);
            }
        }

        [TestFixture]
        public class Contains : CsvFileBodyTests
        {
            private readonly CsvFileLine _csvFileLine = new CsvFileLine("John,Smith,30");

            [Test]
            public void WhenLineIsNull_ThenReturnFalse()
            {
                var result = Act(null);

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenHasNoLines_ThenReturnFalse()
            {
                var result = Act();

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenDoesNotHaveLine_ThenReturnFalse()
            {
                _sut.Lines.Add(new CsvFileLine("John,Jones,20"));

                var result = Act();

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenHasLine_ThenReturnTrue()
            {
                _sut.Lines.Add(_csvFileLine);

                var result = Act();

                Assert.That(result, Is.True);
            }

            private bool Act()
            {
                return Act(_csvFileLine);
            }

            private bool Act(CsvFileLine line)
            {
                return _sut.Contains(line);
            }
        }
    }
}