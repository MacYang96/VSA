(* Represents a CFG node, with an expression and a Successor-nodes list, recursively *)

type Node(id : int, exp : Exp, succ : Node list ) = 

    let mutable Succ = succ

    // method called by a Node instance
    member this.ID() = id

    interface INode with

        // method called by a Node:>INode instance
        member this.ID() = id

        member this.Statm() = exp

        member this.Succ() = Succ |> List.map (fun x -> x :> INode)

        // forward-analysis -> dep = succ
        member this.Dep() = (this :> INode).Succ()

        // method called by a Node:>INode instance
        member this.CompareTo(o : obj) : int = 
            let node = o:?>Node
            if this.ID()>node.ID() 
                then 1
                else if this.ID()<node.ID() 
                    then -1
                else 0
               
    end

    interface System.IComparable with

        // method called by a Node instance
        member this.CompareTo(o : obj) : int = 
                let node = o:?>Node
                if this.ID()>node.ID() 
                    then 1
                    else if this.ID()<node.ID() 
                        then -1
                    else 0

    end

    override this.Equals( o : obj ) =
        let node = o:?>Node 
        if node.ID()=this.ID() then true else false

    override this.GetHashCode() = this.ID()

    override this.ToString() = (string (this.ID())+", "+string ((this:>INode).Statm()))

    member this.ChangeSucc(nodes : Node list) = Succ <- nodes

;;
