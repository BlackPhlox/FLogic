namespace FLogicS

open System

module FL_Base =
    type FL<'a> = 
    //Primary
    | T of 'a
    | F
    //Variable
    | V of string
    //Output
    | O of (string * FL<'a>)
    //Gate
    | G of G<'a>
    
    //Types of gates
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

    let join (p:Map<'a,'b>) (q:Map<'a,'b>) = 
        Map(Seq.concat [ (Map.toSeq p) ; (Map.toSeq q) ])

    let rec calcmm (gts: FL_Base.FL<'a>) (m: Map<string, 'a>): (FL_Base.FL<'a> * Map<string, 'a>) =
        match gts with
        | FL_Base.T x -> (FL_Base.T x, m)
        | FL_Base.F -> (FL_Base.F, m)
        | FL_Base.V v -> 
            let res = Map.tryFind v m
            if res.IsSome then (FL_Base.T res.Value, m) else (FL_Base.F, m)
        | FL_Base.O (s, x) -> 
            let res = calcmm x m
            match res with
            | FL_Base.T r, map -> calcmm (fst res) (Map.add s r map)
            | _ -> res
        | FL_Base.G g -> 
            match g with
            FL_Base.AND(x,y) -> 
                match (calcmm x m, calcmm y m) with
                | ((FL_Base.T a, ma), (FL_Base.T b, mb)) -> if a = b then calcmm x (join ma mb) else (FL_Base.F, m)
                | _ -> (FL_Base.F, m)

    let calcm (gts: FL_Base.FL<'a>) (m: Map<string, 'a>) =
        fst (calcmm gts m)

    let rec calc (gts: FL_Base.FL<'a>) : FL_Base.FL<'a> =
        calcm gts Map.empty