using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenerator.Data;
using NUnit.Framework;

namespace DataGenerator.BL.Test
{
    [TestFixture]
    public class ScriptGeneratorTest
    {
        private IScriptGenerator _generator;

        [SetUp]
        public void Init()
        {
            _generator = new ScriptGenerator(new RepositoryMock());
        }

        [Test]
        public void GenerateUser_NameRequered()
        {
            IScriptGenerator generator = new ScriptGenerator(new RepositoryMock());

            UserEntity entity = generator.GenerateUser();
            string name = entity.Name;

            Assert.That(name, Is.Not.Empty);
        }

        [Test]
        public void GenerateUser_SurnameRequered()
        {
            IScriptGenerator generator = new ScriptGenerator(new RepositoryMock());

            UserEntity entity = generator.GenerateUser();
            string surname = entity.Surname;

            Assert.That(surname, Is.Not.Empty);
        }

        [Test]
        public void GenerateUser_LoginRequered()
        {
            IScriptGenerator generator = new ScriptGenerator(new RepositoryMock());

            UserEntity entity = generator.GenerateUser();
            string login = entity.Login;

            Assert.That(login, Is.Not.Empty);
        }

        [Test]
        public void GenerateUser_PasswordRequered()
        {
            IScriptGenerator generator = new ScriptGenerator(new RepositoryMock());

            UserEntity entity = generator.GenerateUser();
            string password = entity.Password;

            Assert.That(password, Is.Not.Empty);
        }

        [Test]
        public void GenerateUser_EmailRequered()
        {
            IScriptGenerator generator = new ScriptGenerator(new RepositoryMock());

            UserEntity entity = generator.GenerateUser();
            string email = entity.Email;

            Assert.That(email, Is.Not.Empty);
        }

        [Test]
        public void GenerateUser_RegistrationDatePeriod()
        {
            IScriptGenerator generator = new ScriptGenerator(new RepositoryMock());

            UserEntity entity = generator.GenerateUser();
            DateTime registrationDate = entity.RegistrationDate;

            Assert.That(registrationDate, Is.InRange(new DateTime(2010, 1, 1), new DateTime(2016, 2, 29)));
        }

        [Test]
        public void GenerateUser_GetValueLine()
        {
            UserEntity user = new UserEntity()
            {
                Name = "Петр",
                Surname = "Петров",
                Patronymic = "Петрович",
                Email = "petr@gmail.com",
                Login = "petr",
                Password = "12345",
                RegistrationDate = new DateTime(2016, 1, 1)
            };

            IScriptGenerator generator = new ScriptGenerator(new RepositoryMock());

            string expectedResult = @"VALUES ('Петр', 'Петров', 'Петрович', 'petr@gmail.com', 'petr', '12345', '20160101')";

            string result = generator.GetValueLine(user);

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GenerateUser_GetInsertLine()
        {
            IScriptGenerator generator = new ScriptGenerator(new RepositoryMock());

            string expectedResult = @"INSERT INTO BlogUser (Name, Surname, Patronymic, Email, UserLogin, Password, RegistrationDate)";

            string result = generator.GetInsertLine();

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GenerateUser_CreateScript()
        {
            IScriptGenerator generator = new ScriptGenerator(new RepositoryMock());

            string expectedResult =
                @"INSERT INTO BlogUser (Name, Surname, Patronymic, Email, UserLogin, Password, RegistrationDate)" +
                Environment.NewLine +
                "VALUES ('Иван', 'Иванов', 'Иванович', 'ivan@gmail.com', 'ivan', '12345', '20160101')";

            string result = generator.CreateScript(1);

            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
