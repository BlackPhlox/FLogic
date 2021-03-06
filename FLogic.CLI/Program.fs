// Learn more about F# at http://fsharp.org

open System

let rec menu() : Option<string> =
    printfn "Menu"
    let rs = Console.ReadLine()
    let action = 
        match rs with
        | x when x = "Simplify" -> 
            printf "Simp"
            None
        | x when x = "exit" -> 
            Some("Exit")
        | _ -> 
            printf "Invalid command"
            Some("Error")
    if action.IsSome 
    then Some("Exit")
    else menu()

let rec any str1 str2 = function
    | x::xs -> 
        if str1 = x || str2 = x
        then true
        else any str1 str2 xs
    | _ -> false

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
        printfn "Welcome to FLogic.Interactive!"
        let res = menu()
        if res.IsSome then exit(0)
    elif (e_arg "-h" "--help") then
        printfn "Long help message"
    else 
        printfn "Welcome to FLogic.CLI!"
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
        else 
            printfn "Unknown command, exiting"
            
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