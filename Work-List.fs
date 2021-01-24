open System.Collections
open FSharp.Collections

(* Input : Lattice L
           Transfer functions t1,...,tk
           Control-Flow-Graph nodes v1,...,vn
   Output: Abstract states x1,...,xn
*)
let WorkList (L : ILattice<'T>) (T : Map<int,('T->'T)>) (cfg : ICFG)  =
    
    // define: n = #{v1,...,vn}, 
    // x=(<v1,x1>,...,<vn,xn>) = (⊥,...,⊥) abstract states, 
    // W=(v1,...,vn) cfg nodes in worker list
    let n = cfg.Length()
    let x = new Hashtable() 
    let mutable W = []
    let mutable v = [cfg.EntryBlock()]
    let mutable head = v.Head
    for i = 0 to n do
        x.Add(head, L.Bot)
        W <- W@[head]
        v <- v.Tail@(head.Succ())
        head <- v.Head

    // Scans work-list and applies transfer function based on type of statement
    while W.Length <> 0 do
        let vi = W.Head
        W <- W.Tail
        let y = x.[vi] |> T.[vi.Statm().Type]

        // for each vi's successor node, compute join and, if It's not already present, add It
        for vj in vi.Dep() do
            let z = L.Join x.[vj] y
            
            if x.[vj] <> z then 
                x.[vj]<-z
                W <- W@[vj]
    x
;;