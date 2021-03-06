module IOTests

open Xunit
open System.IO

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
let ``Binary string write json no data loss test`` () =
    let name = "jsonwritetest.js"
    let content = """{
    "name" : "Scott",
    "isMale" : true,
    "bday" : {"year":2001, "month":12, "day":25 },
    "favouriteColors" : ["blue", "green"],
    "emptyArray" : [],
    "emptyObject" : {}
    }"""
    let res1 = ParserLibrary.run FLogic.lValue content |> ParserLibrary.sprintResult
    File.WriteAllText(name,content)
    let msg = File.ReadAllText(name)
    let res2 = ParserLibrary.run FLogic.lValue msg |> ParserLibrary.sprintResult
    printfn "%A" msg
    File.Delete(name)
    Assert.Equal(res1,res2)
    Assert.Equal(content,msg)