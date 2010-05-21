﻿namespace FSharpIoC

open System
open System.Collections.Generic
open System.Reflection

module FSharpIoCModule = 
    let rec Resolve requestedType (typeContainer:Dictionary<Type,Type>) =
        let newType = typeContainer.[requestedType]
        let constructors = newType.GetConstructors() 
                           |> Array.sortBy (fun c -> c.GetParameters().Length) 
                           |> Array.rev
        let theConstructor = constructors.[0]
        match theConstructor.GetParameters() with
        | cstorParams when cstorParams.Length = 0 -> Activator.CreateInstance(newType)
        | cstorParams -> cstorParams 
                         |> Seq.map(fun (paramInfo:ParameterInfo) -> 
                                (Resolve paramInfo.ParameterType typeContainer)) 
                         |> Seq.toArray 
                         |> theConstructor.Invoke

type Container =
    val typeContainer : Dictionary<Type, Type>
    new () = {typeContainer = new Dictionary<Type, Type>()}
    member x.Register<'a,'b> () =        
        x.typeContainer.Add(typeof<'a>, typeof<'b>)
    member x.Resolve(requestedType) =
        FSharpIoCModule.Resolve requestedType x.typeContainer
    member x.Resolve<'a> () =
        FSharpIoCModule.Resolve typeof<'a> x.typeContainer :?> 'a
