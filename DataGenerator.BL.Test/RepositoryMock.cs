using System;
using DataGenerator.Data;

namespace DataGenerator.BL.Test
{
    public class RepositoryMock : IRepository
    {
        public void Init()
        {
            throw new NotImplementedException();
        }

        public IdentityInfo GetRandomName()
        {
            return new IdentityInfo {Identity = "Иван", Gender = Gender.Male};
        }

        public string GetRandomSurname(Gender gender)
        {
            return "Иванов";
        }

        public string GetRandomPatronymic(Gender gender)
        {
            return "Иванович";
        }

        public string GetRandomLogin()
        {
            return "ivan";
        }

        public string GetRandomMailDomain()
        {
            return "gmail.com";
        }
    }
}
