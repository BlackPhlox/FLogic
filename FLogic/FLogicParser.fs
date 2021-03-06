module FLogic

open TextInput
open ParserLibrary

let hello (name:string) =
    let msg = sprintf "Hello %s" name
    printfn "%A" msg
    msg

type Lexp =
    //Undefined
    | U 
    | B of bool
    | IN of string * Lexp
    | OUT of string * Lexp
    | AND of Lexp * Lexp
    | JString of string
    | JNumber of float
    | JBool   of bool
    | JNull
    | JObject of Map<string, Lexp>
    | JArray  of Lexp list

let test1 = IN("A", B true)
let test2 = IN("B", U)
let test3 = OUT("O1", IN("C", U))
let test4 = OUT("O1", AND(IN("D", U), IN("D", U)))

//IC (A new gate) or Program (A new curcuit)

// ======================================
// Forward reference
// ======================================

/// Create a forward reference
let createParserForwardedToRef<'a>() =

    let dummyParser : Parser<'a> =
        let innerFn _ = failwith "unfixed forwarded parser"
        {parseFn=innerFn; label="unknown"}

    // ref to placeholder Parser
    let parserRef = ref dummyParser

    // wrapper Parser
    let innerFn input =
        // forward input to the placeholder
        // (Note: "!" is the deferencing operator)
        runOnInput !parserRef input
    let wrapperParser = {parseFn=innerFn; label="unknown"}

    wrapperParser, parserRef

let jValue,jValueRef = createParserForwardedToRef<Lexp>()

// ======================================
// Utility function
// ======================================

// applies the parser p, ignores the result, and returns x.
let (>>%) p x =
    p |>> (fun _ -> x)

// ======================================
// Parsing a Undefined (U)
// ======================================

let lUndefined =
    pstring "U"
    >>% U    // map to U
    <?> "U"  // give it a label
