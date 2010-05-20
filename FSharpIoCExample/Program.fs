module FSharpIoCExample

open System
open FSharpIoC

type Person =
    { FirstName : string
      LastName : string}

type IPersonFactory =
    abstract CreatePerson : unit -> Person

type PersonFactory() =
    interface IPersonFactory with
        member x.CreatePerson () = 
            {FirstName = "TestFirst"; LastName = "TestLast"}

type PersonService =
    val personFactory : IPersonFactory
    new (personFactory) = {personFactory = personFactory}
    member x.GetPerson () = 
        x.personFactory.CreatePerson()

let iocContainer = new Container()
do iocContainer.Register<IPersonFactory, PersonFactory>()
do iocContainer.Register<PersonService, PersonService>()
let personService = iocContainer.Resolve<PersonService>()
let person = personService.GetPerson()
do Console.WriteLine("The persons name is {0} {1}", person.FirstName, person.LastName)
do Console.ReadLine()

