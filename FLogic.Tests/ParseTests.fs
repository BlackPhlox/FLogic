module ParseTests
open Xunit
open FLogic

[<Fact>]
let ``Binary string write read parse test`` () =
    let parsedResult = ParserLibrary.run lUndefined "U" |> ParserLibrary.sprintResult
    Assert.Equal(parsedResult, "U")