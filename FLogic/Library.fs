namespace FLogic

open System

module Say =
    let hello (name:string) =
        let msg = sprintf "Hello %s" name
        printfn "%A" msg
        msg
