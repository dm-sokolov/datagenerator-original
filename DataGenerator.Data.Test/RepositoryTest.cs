using NUnit.Framework;

namespace DataGenerator.Data.Test
{
    [TestFixture]
    public class RepositoryTest
    {
        [Test]
        [TestCase("150  talk [tLk]n- разговор;v-говорить 24.999", "talk")]
        [TestCase("145  stop [stOp] n- остановка; v- останавливать(ся)   25.670", "stop")]
        [TestCase("138  start [stRt] n- начало; v- начинать(ся) 26.488", "start")]
        [TestCase("17  do [dH] v- (did; done [dAn]) делать  278.380", null)]
        public void GetLogin_IsMatch(string line, string expectedResult)
        {
            Repository repository = new Repository();

            string result = repository.GetLogin(line);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
