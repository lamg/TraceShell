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

let evalCommand (eval: string -> string) (input: string) =
  match input.Trim() with
  | ":q" -> Quit
  | ":h" -> PrintHelp
  | _ -> PrintOutput(eval input)

let dummyEval input = $"got {input}"

let repl (evaluator: string -> string) =
  userInputStream ()
  |> Seq.takeWhile (function
    | x when x = null -> false
    | x ->
      match evalCommand evaluator x with
      | Quit -> false
      | PrintOutput output ->
        Console.WriteLine output
        true
      | PrintHelp ->
        Console.WriteLine "HELP"
        true)
  |> Seq.iter ignore
