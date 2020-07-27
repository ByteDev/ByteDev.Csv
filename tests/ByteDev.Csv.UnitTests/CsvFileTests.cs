using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ByteDev.Csv.UnitTests
{
    [TestFixture]
    public class CsvFileTests
    {
        private readonly CsvFileLine _validLine = new CsvFileLine("Name,Age");

        [TestFixture]
        public class HasHeader : CsvFileTests
        {
            [Test]
            public void WhenHeaderIsNull_ThenReturnFalse()
            {
                var sut = new CsvFile();

                Assert.That(sut.HasHeader, Is.False);
            }

            [Test]
            public void WhenHeaderIsNotNull_ThenReturnTrue()
            {
                var sut = new CsvFile(_validLine);

                Assert.That(sut.HasHeader, Is.True);
            }
        }
        
        [TestFixture]
        public class VerifyBody : CsvFileTests
        {
            [Test]
            public void WhenHasNoBody_ThenThrowException()
            {
                var sut = new CsvFile(new CsvFileLine("Name,Age,Height"));

                Assert.Throws<InvalidOperationException>(() => sut.VerifyBody());
            }

            [Test]
            public void WhenHasBody_AndNoHeader_ThenReturn()
            {
                var sut = new CsvFile(new CsvFileBody(new List<CsvFileLine> {new CsvFileLine("John,30")}));

                sut.VerifyBody();

                // We are asserting that no exception will be thrown
            }

            [Test]
            public void WhenHeaderHasMoreValuesThanBody_ThenThrowException()
            {
                var sut = new CsvFile(new CsvFileLine("Name,Age,Height"), 
                    new CsvFileBody(new List<CsvFileLine> {new CsvFileLine("John,30")}));

                Assert.Throws<InvalidOperationException>(() => sut.VerifyBody());
            }

            [Test]
            public void WhenHeaderHasLessValuesThanBody_ThenThrowException()
            {
                var sut = new CsvFile(new CsvFileLine("Name"), 
                    new CsvFileBody(new List<CsvFileLine> {new CsvFileLine("John,30")}));

                Assert.Throws<InvalidOperationException>(() => sut.VerifyBody());
            }
        }
    }
}