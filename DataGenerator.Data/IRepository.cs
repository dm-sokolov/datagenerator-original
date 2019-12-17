namespace DataGenerator.Data
{
    public interface IRepository
    {
        void Init();
        IdentityInfo GetRandomName();
        string GetRandomSurname(Gender gender);
        string GetRandomPatronymic(Gender gender);
        string GetRandomLogin();
        string GetRandomMailDomain();
    }
}
