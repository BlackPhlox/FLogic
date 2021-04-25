// Learn more about F# at http://fsharp.org

open System

let help() =
    printfn "Long help message"

//Warning: Stack overflow if to many menu stack-frames is added to the stack
//Needs to be tail recursive using either 
//tail recursion or continuation
let rec menu (str:Option<string>) : Option<string> =
    if str.IsNone 
    then None
    else
        printfn "%A" str.Value
        let rs = Console.ReadLine()
        Console.Clear()
        let action = 
            match rs with
            | x when x = "Simplify" -> 
                menu (Some "Simplify")
            | x when x = "help" ->
                help()
                menu (Some "Menu")
            | x when x = "exit" -> 
                None
            | _ -> 
                printfn "Invalid command: %A" rs
                None
        if action.IsSome 
        then menu str
        else None

let rec any str1 str2 = function
    | x::xs -> 
        if str1 = x || str2 = x
        then true
        else any str1 str2 xs
    | _ -> false

let handle_CLI e_arg lst_args =
    if e_arg "-tt" "--truthtable" then printfn "With truthtable"
    if e_arg "-t" "--tree" then printfn "With tree"

    let gateEval = e_arg "-ge" "--gateeval"
    let typeSimp = e_arg "-ts" "--typesimp"

    if gateEval && typeSimp then
        printfn "Invalid request, you can't both do gate evaluation and type simplification"
    elif gateEval then 
        printfn "Gate evaluation"
        if not (e_arg "-in" "--input") 
        then printfn "No input provided"
        else printfn "Reading input"

    elif typeSimp then
        printfn "Type simplification"
        if not (e_arg "-g" "--gates") 
        then printfn "No gates provided, using most highest complex gates"
        else printfn "Only using gates: [AND,NAND]"
    else 
        printfn "Unknown command(s) : %A, exiting" lst_args
    

[<EntryPoint>]
let main argv =
    let lst_args = (Array.toList argv)
    #if DEBUG    
    printfn "Arguments : %A" argv.Length
    if argv.Length > 0 then printfn "All args : %A" lst_args
    #endif

    let e_arg str1 str2 = any str1 str2 lst_args 

    //No args provided or contains overwrite keyword interactive
    if argv.Length = 0 || (any "-i" "--interactive" lst_args) then
        //Interactive handler
        printfn "Welcome to FLogic.Interactive!\nWrite 'help' to get more information."
        let res = menu (Some "Menu")
        if res.IsNone then exit(0)
    elif (e_arg "-h" "--help") then
        //Help printout
        help()
    else 
        //CLI Handler
        printfn "Welcome to FLogic.CLI!"
        handle_CLI e_arg lst_args
            
    printfn ""
    printfn "Press any key to exit"
    let _ = Console.ReadLine()    
    0 // return an integer exit code

    // Parse input
    // FLogic Interactive

    // Functionailty
        //Type Simplify
        //Gate Eval
        //Tree
        //Truth Table