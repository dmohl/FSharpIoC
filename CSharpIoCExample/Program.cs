using System;
using FSharpIoC;

namespace CSharpIoCExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var iocContainer = new Container();
            iocContainer.Register<IPersonFactory, PersonFactory>();
            iocContainer.Register<PersonService, PersonService>();
            var personService = iocContainer.Resolve<PersonService>();
            var person = personService.GetPerson();
            Console.WriteLine("The persons name is {0} {1}", person.FirstName, person.LastName);
            Console.ReadLine();
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public interface IPersonFactory
    {
        Person CreatePerson();
    }

    public class PersonFactory : IPersonFactory
    {
        public Person CreatePerson()
        {
            return new Person { FirstName = "TestFirst", LastName = "TestLast" };
        }
    }

    public class PersonService
    {
        protected readonly IPersonFactory _personFactory;
        public PersonService(IPersonFactory personFactory)
        {
            _personFactory = personFactory;
        }

        public Person GetPerson()
        {
            return _personFactory.CreatePerson();
        }
    }
}
