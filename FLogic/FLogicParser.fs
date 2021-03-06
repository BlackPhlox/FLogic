module FLogic

open TextInput
open ParserLibrary
open System

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
    <?> "bool"       // give it a label

// ======================================
// Parsing a LString
// ======================================

/// Parse an unescaped char
let lUnescapedChar =
    satisfy (fun ch -> ch <> '\\' && ch <> '\"') "char"

/// Parse an escaped char
let lEscapedChar =
    [
    // (stringToMatch, resultChar)
    ("\\\"",'\"')      // quote
    ("\\\\",'\\')      // reverse solidus
    ("\\/",'/')        // solidus
    ("\\b",'\b')       // backspace
    ("\\f",'\f')       // formfeed
    ("\\n",'\n')       // newline
    ("\\r",'\r')       // cr
    ("\\t",'\t')       // tab
    ]
    // convert each pair into a parser
    |> List.map (fun (toMatch,result) ->
        pstring toMatch >>% result)
    // and combine them into one
    |> choice
    <?> "escaped char" // set label

/// Parse a unicode char
let lUnicodeChar =

    // set up the "primitive" parsers
    let backslash = pchar '\\'
    let uChar = pchar 'u'
    let hexdigit = 
        anyOf (['0'..'9'] @ ['A'..'F'] @ ['a'..'f'])
    let fourHexDigits =
        hexdigit .>>. hexdigit .>>. hexdigit .>>. hexdigit

    // convert the parser output (nested tuples)
    // to a char
    let convertToChar (((h1,h2),h3),h4) =
        let str = sprintf "%c%c%c%c" h1 h2 h3 h4
        Int32.Parse(str,Globalization.NumberStyles.HexNumber) |> char

    // set up the main parser
    backslash  >>. uChar >>. fourHexDigits
    |>> convertToChar

/// Parse a quoted string
let quotedString =
    let quote = pchar '\"' <?> "quote"
    let jchar = lUnescapedChar <|> lEscapedChar <|> lUnicodeChar

    // set up the main parser
    quote >>. manyChars jchar .>> quote

/// Parse a JString
let jString =
    // wrap the string in a JString
    quotedString
    |>> JString           // convert to JString
    <?> "quoted string"   // add label