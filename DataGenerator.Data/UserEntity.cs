using System;

namespace DataGenerator.Data
{
    public class UserEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
