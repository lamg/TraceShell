module TraceShell.REPL

open System

type Action =
  | Quit
  | PrintOutput of string
  | PrintHelp

let userInputStream () =
  Seq.initInfinite (fun _ ->
    Console.Write ">>> "
    Console.ReadLine())

let evalCommand (eval: string -> string) input =
  match input with
  | ":q" -> Quit
  | ":h" -> PrintHelp
  | _ -> PrintOutput(eval input)

let dummyEval input = $"got {input}"

let repl (evaluator: string -> string) =
  userInputStream ()
  |> Seq.takeWhile (fun x -> x <> null)
  |> Seq.map (evalCommand evaluator)
  |> Seq.takeWhile (function
    | Quit -> false
    | PrintOutput output ->
      Console.WriteLine output
      true
    | PrintHelp ->
      Console.WriteLine "HELP"
      true)
  |> Seq.toList
  |> ignore
