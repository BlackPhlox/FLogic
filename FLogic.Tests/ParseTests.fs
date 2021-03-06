module Parse
open Xunit
open Xunit.Sdk
open Xunit.Abstractions
open FLogic

type Tests(output:ITestOutputHelper) =
    let dprint a (b:string) = output.WriteLine(a,b)

    [<Fact>]
    let ``Prints string to output test console`` () =
        dprint "Hello {0}" "World!"

    [<Fact>]
    let ``String Undefined Parse Test Succeeds`` () =
        let parsedResult = ParserLibrary.run lUndefined "U" |> ParserLibrary.sprintResult
        Assert.Equal("U", parsedResult)

    [<Fact>]
    let ``String invalid Parse Test fails`` () =
        let value = "X"
        let parsedResult = ParserLibrary.run lUndefined value |> ParserLibrary.sprintResult
        Assert.Equal($"Line:0 Col:0 Error parsing U\n{value}\n^Unexpected '{value}'", parsedResult)

    [<Fact>]
    let ``String Bool True & False Parse Test Succeeds`` () =
        let parsedTrueResult = ParserLibrary.run lBool "true" |> ParserLibrary.sprintResult
        let parsedFalseResult = ParserLibrary.run lBool "false" |> ParserLibrary.sprintResult
        Assert.Equal("B true", parsedTrueResult)
        Assert.Equal("B false", parsedFalseResult)
