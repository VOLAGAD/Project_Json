using System.Text.Json;

namespace Project
{
    static class PathToFiles
    {
        public const string PathToList = "List_Persons.json";
        public const string PathToLastPerson = "Last_Person.json";
    }

    static class ManagerPerson
    {
        public static Person GetPersonFromFile(string pathFile)
        {
            if (!File.Exists(pathFile))
                File.WriteAllText(pathFile, "{}");

            string json = File.ReadAllText(pathFile);
            Person person = JsonSerializer.Deserialize<Person>(json);

            return person is not null ? person : new Person();
        }

        public static List<Person> GetPersonListFromFile(string pathFile)
        {
            if (!File.Exists(pathFile))
                File.WriteAllText(pathFile, "{}");

            string json = File.ReadAllText(pathFile);
            List<Person> persons = JsonSerializer.Deserialize<List<Person>>(json);

            return persons is not null ? persons : new List<Person>();
        }

        public static void SavePerson(string pathFile, Person person)
        {
            string json = JsonSerializer.Serialize(person);
            File.WriteAllText(pathFile, json);
        }

        public static void SavePerson(string pathFile, List<Person> persons)
        {
            string json = JsonSerializer.Serialize(persons);
            File.WriteAllText(pathFile, json);
        }

        public static void InitFileSystem(string pathToLastPerson, string pathToListPersons)
        {
            File.WriteAllText(pathToLastPerson, "{}");
            File.WriteAllText(pathToListPersons, "{}");
        }
    }

    static class CreatorPerson
    {
        public static List<Person> Persons { get; private set; } = new List<Person>();

        public static Person CreatePerson()
        {
            Console.Write("Write name your person: ");
            string name = Console.ReadLine() ?? "Default";

            Console.Write("Write age your person: ");
            int.TryParse(Console.ReadLine() ?? "0", out int age);

            return new Person(name, age);
        }

        public static void AddPersonToList(Person person) => Persons.Add(person);

        public static Person ChoosePerson()
        {
            if (Persons.Count == 0)
                Persons = ManagerPerson.GetPersonListFromFile(PathToFiles.PathToList);

            Console.WriteLine($"You have {Persons.Count} persons");

            if (!ManagerPerson.GetPersonFromFile(PathToFiles.PathToLastPerson).Equals(new Person()))
            {
                Console.WriteLine($"Last Person Load (0)");
                Console.WriteLine("- - -");
            }

            for (int i = 0; i < Persons.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Persons[i].Name} {Persons[i].Age}");
                Console.WriteLine("- - -");
            }

            Console.Write("Choose person by index: ");
            int.TryParse(Console.ReadLine() ?? "1", out int change);

            if (change == 0)
                return ManagerPerson.GetPersonFromFile(PathToFiles.PathToLastPerson);

            for (int i = 0; i < Persons.Count; i++)
            {
                if (change - 1 == i)
                {
                    ManagerPerson.SavePerson(PathToFiles.PathToLastPerson, Persons[i]);
                    return Persons[i];
                }
            }

            Console.WriteLine("\n!Write correct index!\n");
            return new Person();
        }
    }

    class Person
    {
        public Person() : this("Default", 0) { }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; set; }
        public int Age { get; set; }

        public void PrintDataPerson()
        {
            Console.WriteLine($"Name: {Name}\n" +
                $"Age: {Age}");
        }

        public override bool Equals(object? obj)
        {
            if (obj is Person person)
                return person.Name == Name && person.Age == Age;

            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ManagerPerson.InitFileSystem(PathToFiles.PathToLastPerson, PathToFiles.PathToList); // The required line!

            Person person = CreatorPerson.ChoosePerson();

            person.PrintDataPerson();
        }
    }
}
