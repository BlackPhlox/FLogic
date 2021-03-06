module Tests

open System
open Xunit
open System.IO
open FLogic

[<Fact>]
let ``Hello world test`` () =
    Assert.Equal("Hello Test", hello "Test")

[<Fact>]
let ``Binary write test`` () =
    let v = 42
    let name = "binarytestdata"
    let bw = new BinaryWriter(new FileStream(name, FileMode.Create))
    bw.Write(v);
    bw.Close();
    let br = new BinaryReader(new FileStream(name, FileMode.Open));
    let i = br.ReadInt32();
    printfn "%A" i
    br.Close();
    File.Delete(name)
    Assert.Equal(v,i)

[<Fact>]
let ``Binary string write test`` () =
    let name = "stringtestdata"
    let content = "hello\nworld"
    File.WriteAllText(name,content)
    let msg = File.ReadAllText(name)
    printfn "%A" msg
    File.Delete(name)
    Assert.Equal(content,msg)

[<Fact>]
let ``Binary string write read parse test`` () =
    let a = ParserLibrary.run lUndefined "U" |> ParserLibrary.sprintResult
    Assert.Equal(a, "U")