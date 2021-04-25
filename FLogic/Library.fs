namespace FLogic

open System

module FL_Base =
    type FL<'a> = 
    | T of 'a
    | F
    | O of string
    | V of string
    | G of G<'a>
    
    and G<'a> =
    | AND of FL<'a> * FL<'a>

module Say =
    let hello (name:string) =
        let msg = sprintf "Hello %s" name
        printfn "%A" msg
        msg

    let constructBase str =
        FL_Base.T str

    let constructGT str =
        FL_Base.T (FL_Base.T str)

    let rec calcm (gts: FL_Base.FL<'a>) (m: Map<string, 'a>): FL_Base.FL<'a> =
        match gts with
        | FL_Base.T x -> FL_Base.T x
        | FL_Base.F -> FL_Base.F
        | FL_Base.V v -> 
            let res = Map.tryFind v m
            if res.IsSome then FL_Base.T res.Value else FL_Base.F
        | FL_Base.G g -> 
            match g with
            FL_Base.AND(x,y) -> if calcm x m = calcm y m then calcm x m else FL_Base.F

    let rec calc (gts: FL_Base.FL<'a>) : FL_Base.FL<'a> =
        calcm gts Map.empty