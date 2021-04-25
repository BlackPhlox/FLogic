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

    let rec calcmmo (gts: FL_Base.FL<'a>) (m: Map<string, 'a>) o: (FL_Base.FL<'a> * Map<string, 'a> * Map<string, FL_Base.FL<'a>>) =
        match gts with
        | FL_Base.T x -> (FL_Base.T x, m, o)
        | FL_Base.F -> (FL_Base.F, m, o)
        | FL_Base.V v -> 
            let res = Map.tryFind v m
            if res.IsSome then (FL_Base.T res.Value, m, o) else (FL_Base.F, m, o)
        | FL_Base.O (s, x) -> 
            let res = calcmmo x m o
            let (exp, ma, out) = res 
            match res with
            | FL_Base.T r, map, o -> calcmmo exp (Map.add s r map) (Map.add s exp o)
            | _ -> res
        | FL_Base.G g -> 
            match g with
            FL_Base.AND(x,y) -> 
                match (calcmmo x m o, calcmmo y m o) with
                | ((FL_Base.T a, ma, oa), (FL_Base.T b, mb, ob)) -> if a = b then calcmmo x (join ma mb) (join oa ob) else (FL_Base.F, m, (join oa ob))
                | _ -> (FL_Base.F, m, o)
    
    let calcmm (gts: FL_Base.FL<'a>) (m: Map<string, 'a>) : (FL_Base.FL<'a> * Map<string, 'a> * Map<string, FL_Base.FL<'a>>) =
        calcmmo gts m Map.empty

    let calcm (gts: FL_Base.FL<'a>) m =
        let (e, map, out) = (calcmm gts m)
        e

    let rec calc (gts: FL_Base.FL<'a>) : FL_Base.FL<'a> =
        calcm gts Map.empty