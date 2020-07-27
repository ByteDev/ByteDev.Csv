using System;
using System.Linq;
using ByteDev.Common.Collections;
using NUnit.Framework;

namespace ByteDev.Csv.UnitTests
{
    [TestFixture]
    public class CsvFileLineTests
    {
        private static readonly string[] Values = { "John", "Smith", "30" };

        private const string Line = "John,Smith,30";

        [TestFixture]
        public class ConstructorValues : CsvFileLineTests
        {
            [Test]
            public void WhenValuesIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => { var l = new CsvFileLine(null as string[]); });
            }

            [Test]
            public void WhenValuesIsEmpty_ThenValuesReturnsEmpty()
            {
                string[] values = new string[0];

                var sut = new CsvFileLine(values);

                var result = sut.Values;

                Assert.That(result.Length, Is.EqualTo(0));
            }

            [Test]
            public void WhenValuesIsEmpty_ThenLineReturnsEmpty()
            {
                var sut = new CsvFileLine(string.Empty);

                var result = sut.Line;

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenValidValues_ThenSetValues()
            {
                var sut = new CsvFileLine(Values);

                var result = sut.Values;

                Assert.That(result.First(), Is.EqualTo(Values.First()));
                Assert.That(result.Second(), Is.EqualTo(Values.Second()));
                Assert.That(result.Third(), Is.EqualTo(Values.Third()));
            }

            [Test]
            public void WhenValidValues_ThenSetLine()
            {
                var sut = new CsvFileLine(Values);

                var result = sut.Line;
                
                Assert.That(result, Is.EqualTo(Line));
            }

            [Test]
            public void WhenOneValue_ThenSetValuesAndLine()
            {
                var sut = new CsvFileLine(new []{"John"});

                Assert.That(sut.Values.Single(), Is.EqualTo("John"));
                Assert.That(sut.Line, Is.EqualTo("John"));
            }

            [Test]
            public void WhenValuesContainCommas_ThenTreatCommasAsEscaped()
            {
                var sut = new CsvFileLine(new[] { ",", ",,", ",,," });

                Assert.That(sut.Values.First(), Is.EqualTo(","));
                Assert.That(sut.Values.Second(), Is.EqualTo(",,"));
                Assert.That(sut.Values.Third(), Is.EqualTo(",,,"));

                Assert.That(sut.Line, Is.EqualTo("\",\",\",,\",\",,,\""));
            }

            [Test]
            public void WhenValueIsEscapedWithDoubleQuotes_ThenTreatAsSingleValue()
            {
                var sut = new CsvFileLine(new []{ "list, of, items", "222" });

                Assert.That(sut.Values.First(), Is.EqualTo("list, of, items"));
                Assert.That(sut.Values.Second(), Is.EqualTo("222"));

                Assert.That(sut.Line, Is.EqualTo("\"list, of, items\",222"));
            }
        }

        [TestFixture]
        public class ConstructorLine : CsvFileLineTests
        {
            [Test]
            public void WhenLineIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => { new CsvFileLine(null as string); });
            }

            [Test]
            public void WhenLineIsEmpty_ThenValuesReturnsEmpty()
            {
                var sut = new CsvFileLine(string.Empty);

                var result = sut.Values;

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenLineIsEmpty_ThenLineReturnsEmpty()
            {
                var sut = new CsvFileLine(string.Empty);

                var result = sut.Line;

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenValidLine_ThenSetValues()
            {
                var sut = new CsvFileLine(Line);

                var result = sut.Values;

                Assert.That(result.First(), Is.EqualTo(Values.First()));
                Assert.That(result.Second(), Is.EqualTo(Values.Second()));
                Assert.That(result.Third(), Is.EqualTo(Values.Third()));
            }
            
            [Test]
            public void WhenValidLine_ThenSetLine()
            {
                var sut = new CsvFileLine(Line);

                var result = sut.Line;

                Assert.That(result, Is.EqualTo(Line));
            }

            [Test]
            public void WhenOneValueInLineString_ThenSetValuesAndLine()
            {
                var sut = new CsvFileLine("John");

                Assert.That(sut.Values.Single(), Is.EqualTo("John"));
                Assert.That(sut.Line, Is.EqualTo("John"));
            }

            [Test]
            public void WhenValuesContainCommas_ThenTreatCommasAsEscaped()
            {
                var sut = new CsvFileLine("\",\",\",,\",\",,,\"");

                Assert.That(sut.Values.First(), Is.EqualTo(","));
                Assert.That(sut.Values.Second(), Is.EqualTo(",,"));
                Assert.That(sut.Values.Third(), Is.EqualTo(",,,"));

                Assert.That(sut.Line, Is.EqualTo("\",\",\",,\",\",,,\""));
            }

            [Test]
            public void WhenValueIsEscapedWithDoubleQuotes_ThenTreatAsSingleValue()
            {
                var sut = new CsvFileLine("\"list, of, items 1\",222");

                Assert.That(sut.Values.First(), Is.EqualTo("list, of, items 1"));
                Assert.That(sut.Values.Second(), Is.EqualTo("222"));

                Assert.That(sut.Line, Is.EqualTo("\"list, of, items 1\",222"));
            }

            [Test]
            public void WhenValueHasOpenEscapedQuoteOnly_ThenTreatNotAsSingleValue()
            {
                var sut = new CsvFileLine("\"list,of,items");

                Assert.That(sut.Values.First(), Is.EqualTo("\"list"));
                Assert.That(sut.Values.Second(), Is.EqualTo("of"));
                Assert.That(sut.Values.Third(), Is.EqualTo("items"));


                Assert.That(sut.Line, Is.EqualTo("\"list,of,items"));
            }
        }

        [TestFixture]
        public class IsEmpty : CsvFileLineTests
        {
            [Test]
            public void WhenIsEmpty_ThenReturnTrue()
            {
                var sut = new CsvFileLine(string.Empty);

                var result = sut.IsEmpty;

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenIsNotEmpty_ThenReturnFalse()
            {
                var sut = new CsvFileLine(Line);

                var result = sut.IsEmpty;

                Assert.That(result, Is.False);
            }
        }

        [TestFixture]
        public class ValuesCount : CsvFileLineTests
        {
            [Test]
            public void WhenEmpty_ThenReturnZero()
            {
                var sut = new CsvFileLine(string.Empty);

                var result = sut.ValuesCount;

                Assert.That(result, Is.EqualTo(0));
            }
        }

        [TestFixture]
        public class EqualsOverride : CsvFileLineTests
        {
            private CsvFileLine _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new CsvFileLine(Line);
            }

            [Test]
            public void WhenLineIsNull_ThenReturnFalse()
            {
                var result = Act(null);

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenLinesHaveRefEquality_ThenReturnTrue()
            {
                var line = _sut;

                var result = Act(line);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenIsNotOfSameType_ThenReturnFalse()
            {
                var obj = new object();

                var result = Act(obj);

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenLinesAreEqual_ThenReturnTrue()
            {
                var line = new CsvFileLine(Line);

                var result = Act(line);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenLinesAreNotEqual_ThenReturnFalse()
            {
                var line = new CsvFileLine(Line + ",Blah");

                var result = Act(line);

                Assert.That(result, Is.False);
            }

            private bool Act(object line)
            {
                return _sut.Equals(line);
            }
        }

        [TestFixture]
        public class EquatableEquals
        {
            private CsvFileLine _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new CsvFileLine(Line);
            }

            [Test]
            public void WhenLineIsNull_ThenReturnFalse()
            {
                var result = Act(null);

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenLinesHaveRefEquality_ThenReturnTrue()
            {
                var line = _sut;

                var result = Act(line);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenLinesAreEqual_ThenReturnTrue()
            {
                var line = new CsvFileLine(Line);

                var result = Act(line);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenLinesAreNotEqual_ThenReturnFalse()
            {
                var line = new CsvFileLine(Line + ",Blah");

                var result = Act(line);

                Assert.That(result, Is.False);
            }

            private bool Act(CsvFileLine line)
            {
                return _sut.Equals(line);
            }
        }
    }
}