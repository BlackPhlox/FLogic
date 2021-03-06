module Tests

open System
open Xunit
open FLogic

[<Fact>]
let ``My test`` () =
    Assert.Equal("Hello Test", Say.hello "Test")
