using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataGenerator.Data
{
    public class Repository : IRepository
    {
        private readonly object _key = new object();

        private readonly List<IdentityInfo> _names = new List<IdentityInfo>();
        private readonly List<IdentityInfo> _surnames = new List<IdentityInfo>();
        private readonly List<IdentityInfo> _patronymics = new List<IdentityInfo>();
        private readonly List<string> _logins = new List<string>();
        private readonly List<string> _mailDomains = new List<string>();

        private string[] _maleSurnames;
        private string[] _femaleSurnames;
        private string[] _malePatronymic;
        private string[] _femalePatronymic;

        private  int[] _randomUniqNumbers;
        private int _currentLoginIndex = 0;
        private readonly Random _random = new Random();

        private const string MALE_NAMES_PATH = @"Files\MaleNames.txt";
        private const string FEMALE_NAMES_PATH = @"Files\FemaleNames.txt";
        private const string SURNAMES_PATH = @"Files\surnames.txt";
        private const string PATRONYMIC_PATH = @"Files\patronymic.txt";
        private const string ENGLISH_WORD_PATH = @"Files\EnglishWord.txt";

        public void Init()
        {
            FillNames();
            FillSurnames();
            FillPatronymic();
            FillLogins();
            FillMailDomains();

            _randomUniqNumbers = Enumerable.Range(0, _logins.Count).OrderBy(x => _random.NextDouble()).ToArray();

            _maleSurnames =
                _surnames.Where(s => s.Gender == Gender.Male || s.Gender == Gender.Unisex)
                    .Select(s => s.Identity.Replace("'",""))
                    .ToArray();

            _femaleSurnames =
                _surnames.Where(s => s.Gender == Gender.Female || s.Gender == Gender.Unisex)
                    .Select(s => s.Identity.Replace("'", ""))
                    .ToArray();

            _malePatronymic =
                _patronymics.Where(p => p.Gender == Gender.Male || p.Gender == Gender.Unisex)
                    .Select(p => p.Identity.Substring(0, 1).ToUpper() + p.Identity.Substring(1).Replace("'", ""))
                    .ToArray();

            _femalePatronymic =
                _patronymics.Where(p => p.Gender == Gender.Female || p.Gender == Gender.Unisex)
                    .Select(p => p.Identity.Substring(0, 1).ToUpper() + p.Identity.Substring(1).Replace("'", ""))
                    .ToArray();
        }


        public IdentityInfo GetRandomName()
        {
            return _names[_random.Next(_names.Count)];
        }

        public string GetRandomSurname(Gender gender)
        {
            string[] surnames = gender == Gender.Female ? _femaleSurnames : _maleSurnames;
            return surnames[_random.Next(surnames.Length)];
        }

        public string GetRandomPatronymic(Gender gender)
        {
            string[] patronymic = gender == Gender.Female ? _femalePatronymic : _malePatronymic;
            return patronymic[_random.Next(patronymic.Length)];
        }

        public string GetRandomLogin()
        {
            if (_currentLoginIndex >= _logins.Count)
                throw new Exception("Из источника данных невозможно выбрать уникальный логин.");

            string login;

            lock (_key)
            {
                login = _logins[_randomUniqNumbers[_currentLoginIndex]];
                _currentLoginIndex++;
            }


            return login;
        }

        public string GetRandomMailDomain()
        {
            return _mailDomains[_random.Next(_mailDomains.Count)];
        }

        internal string GetLogin(string line)
        {
            Regex regex = new Regex(@"\d+\s{2}(?<word>[a-z]+).+?n-");
            if (regex.IsMatch(line))
            {
                return regex.Match(line).Groups["word"].ToString();
            }

            return null;
        }

        private void FillNames()
        {
            Action<string, Gender> addNames = (a, b) =>
            {
                using (TextReader reader = new StreamReader(a))
                {
                    while (true)
                    {
                        string name = reader.ReadLine();
                        if (name == null) break;

                        _names.Add(new IdentityInfo() { Gender = b, Identity = name });
                    }
                }
            };

            addNames(MALE_NAMES_PATH, Gender.Male);
            addNames(FEMALE_NAMES_PATH, Gender.Female);
        }

        private void FillSurnames()
        {
            using (TextReader reader=new StreamReader(SURNAMES_PATH))
            {
                while (true)
                {
                    string surname = reader.ReadLine();
                    if(surname==null)break;

                    char endSymbol = surname.ToLower()[surname.Length - 1];

                    Gender gender;
                    switch (endSymbol)
                    {
                        case 'а':
                        case 'я':
                            gender = Gender.Female;
                            break;
                        case 'й':
                        case 'в':
                        case 'н':
                            gender = Gender.Male;
                            break;
                        default:
                            gender = Gender.Unisex;
                            break;
                    }

                    _surnames.Add(new IdentityInfo {Identity = surname, Gender = gender});
                }
            }
        }

        private void FillPatronymic()
        {
            using (TextReader reader = new StreamReader(PATRONYMIC_PATH))
            {
                while (true)
                {
                    string patronymic = reader.ReadLine();
                    if (patronymic == null) break;

                    char endSymbol = patronymic.ToLower()[patronymic.Length - 1];

                    Gender gender;
                    switch (endSymbol)
                    {
                        case 'а':
                            gender = Gender.Female;
                            break;
                        case 'ч':
                            gender = Gender.Male;
                            break;
                        default:
                            gender = Gender.Unisex;
                            break;
                    }

                    _patronymics.Add(new IdentityInfo {Identity = patronymic, Gender = gender});
                }
            }
        }

        private void FillLogins()
        {
            using (TextReader reader = new StreamReader(ENGLISH_WORD_PATH))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null) break;

                    string login = GetLogin(line);
                    if (login != null)
                        _logins.Add(login);
                }
            }
        }

        private void FillMailDomains()
        {
            _mailDomains.Add("gmail.com");
            _mailDomains.Add("mail.ru");
            _mailDomains.Add("hotmail.com");
            _mailDomains.Add("yandex.ru");
            _mailDomains.Add("list.ru");
        }
    }
}
