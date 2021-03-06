module FLogic

open TextInput
open ParserLibrary

let hello (name:string) =
    let msg = sprintf "Hello %s" name
    printfn "%A" msg
    msg

type Lexp =
    //Value:
    //(U)ndefined or (B)ool
    | U 
    | B of bool
    //IO
    | IN of string * Lexp
    | OUT of string * Lexp
    //Base Gates
    | AND of Lexp * Lexp
    | JString of string
    | JNumber of float
    | JBool   of bool
    | JNull
    | JObject of Map<string, Lexp>
    | JArray  of Lexp list

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

// ======================================
// Parsing a JBool
// ======================================

let lBool =
    let ltrue =
        pstring "true"
        >>% B true   // map to JBool
    let lfalse =
        pstring "false"
        >>% B false  // map to JBool

    // choose between true and false
    ltrue <|> lfalse
    <?> "bool"           // give it a label