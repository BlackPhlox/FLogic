module TypeTests

open System
open Xunit
open System.IO
open FLogic

[<Fact>]
let ``Type test 01`` () =
    let test1 = IN("A", B true)
    let test2 = IN("B", U)
    let test3 = OUT("O1", IN("C", U))
    let test4 = OUT("O1", AND(IN("D", U), IN("D", U)))
    Assert.IsNotType<Lexp>(test1)
    Assert.IsNotType<Lexp>(test2)
    Assert.IsNotType<Lexp>(test3)
    Assert.IsNotType<Lexp>(test4)