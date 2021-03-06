module BasicTests

open System
open Xunit
open System.IO
open FLogic

[<Fact>]
let ``Hello world test`` () =
    Assert.Equal("Hello Test", hello "Test")