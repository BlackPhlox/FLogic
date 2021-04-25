module Tests

open System
open Xunit
open FLogicS

[<Fact>]
let ``My test`` () =
    Assert.Equal("Hello Test", Say.hello "Test")

[<Fact>]
let ``Constuct base test`` () =
    Assert.Equal(FL_Base.T "Test", FL_Base.T "Test")

[<Fact>]
let ``Simple calculation test fail`` () =
    Assert.Equal(FL_Base.F, 
        Say.calc (
            FL_Base.G(
                FL_Base.AND(
                    FL_Base.T "Test", 
                    FL_Base.F
                )
            )
        )
    )

[<Fact>]
let ``Simple calculation test success`` () =
    Assert.Equal(FL_Base.T "Test", 
        Say.calc (
            FL_Base.G(
                FL_Base.AND(
                    FL_Base.T "Test", 
                    FL_Base.T "Test"
                )
            )
        )
    )

[<Fact>]
let ``Simple map calculation test success`` () =
    Assert.Equal(FL_Base.T "Test", 
        (
            Say.calcm (
                FL_Base.G(
                    FL_Base.AND(
                        FL_Base.T "Test", 
                        FL_Base.V "H"
                    )
                )
            )
        (Map.ofList [("H","Test")])
        )
    )

[<Fact>]
let ``Simple type map calculation test success`` () =
    Assert.Equal(FL_Base.T true, 
        (
            Say.calcm (
                FL_Base.G(
                    FL_Base.AND(
                        FL_Base.T true, 
                        FL_Base.V "H"
                    )
                )
            )
        (Map.ofList [("H",true)])
        )
    )

[<Fact>]
let ``Simple map calculation test fails`` () =
    Assert.Equal(FL_Base.F, 
        (
            Say.calcm (
                FL_Base.G(
                    FL_Base.AND(
                        FL_Base.T "Test", 
                        FL_Base.V "HEllo"
                    )
                )
            )
        (Map.ofList [("H","Test")])
        )
    )