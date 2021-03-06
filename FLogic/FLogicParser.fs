open System
open ParserLibrary


type JValue =
    | JString of string
    | JNumber of float
    | JBool   of bool
    | JNull
    | JObject of Map<string, JValue>
    | JArray  of JValue list

  let jNull =
      pstring "null"
      |>> (fun _ -> JNull)  // map to JNull
      <?> "null"            // give it a labellet jNull =
      pstring "null"
      |>> (fun _ -> JNull)  // map to JNull
      <?> "null"            // give it a label