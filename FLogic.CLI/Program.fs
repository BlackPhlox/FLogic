// Learn more about F# at http://fsharp.org

open System

[<EntryPoint>]
let main argv =
    #if DEBUG    
    printfn "Arguments : %A" argv.Length
    if argv.Length > 0 then printfn "All args : %A" (Array.toList argv)
    #endif
    //No args provided or contains overwrite keyword interactive
    let input = argv.ToString()
    if argv.Length = 0 || input.Contains "-i" || input.Contains "--interactive" then 
        printfn "Welcome to FLogic.Interactive!"
        else 
        printfn "Welcome to FLogic.CLI!"

    let rs = Console.ReadLine()
    
    printf "%A" rs
    0 // return an integer exit code

    // Parse input
    // FLogic Interactive

    // Functionailty
        //Type Simplify
        //Gate Eval
        //Tree
        //Truth Table
