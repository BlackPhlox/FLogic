module Tests

open System
open Xunit
open FLogic
open System.IO

[<Fact>]
let ``My test`` () =
    Assert.Equal("Hello Test", Say.hello "Test")

[<Fact>]
let ``My binary write test`` () =
    let v = 42
    let name = "mybinarytestdata"
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
let ``My binary string write test`` () =
    let name = "mystringtestdata"
    let content = "hello\nworld"
    File.WriteAllText(name,content)
    let msg = File.ReadAllText(name)
    printfn "%A" msg
    File.Delete(name)
    Assert.Equal(content,msg)