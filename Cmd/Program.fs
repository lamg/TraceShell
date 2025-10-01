open System
open System.IO
open FSharp.Compiler.Interactive.Shell

[<EntryPoint>]
let main _ =
  // Create in/out streams for the REPL
  use inStream = new StringReader("")
  use outStream = new StringWriter()
  use errStream = new StringWriter()

  // FSI arguments (pretend we are running `dotnet fsi`)
  let argv = [| "fsi.exe"; "--noninteractive" |]

  // Create the interactive session
  let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()

  let fsiSession =
    FsiEvaluationSession.Create(fsiConfig, argv, inStream, outStream, errStream)

  printfn "🔁 F# REPL (type `:quit` to exit)"
  printfn "----------------------------------"

  let rec loop () =
    Console.Write("> ")
    let input = Console.ReadLine()

    match input with
    | null
    | ":quit" -> ()
    | code ->
      try
        let result, _ = fsiSession.EvalInteractionNonThrowing(code)
        printfn "%s" (outStream.ToString())
        outStream.GetStringBuilder().Clear() |> ignore
        errStream.GetStringBuilder().Clear() |> ignore
      with ex ->
        printfn "❌ Error: %s" ex.Message

      loop ()

  loop ()
  0
