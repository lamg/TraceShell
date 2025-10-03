module TraceShell.FsEval

open System.IO
open System.Text
open FSharp.Compiler.Interactive.Shell

let makeSession () =
  let sbOut = new StringBuilder()
  let sbErr = new StringBuilder()
  let inStream = new StringReader ""
  let outStream = new StringWriter(sbOut)
  let errStream = new StringWriter(sbErr)

  let argv = [| "fsi.exe"; "--noninteractive" |]
  let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()
  FsiEvaluationSession.Create(fsiConfig, argv, inStream, outStream, errStream)

let eval (session: FsiEvaluationSession) (input: string) =
  match session.EvalInteractionNonThrowing input with
  | Choice.Choice1Of2(Some v), _ ->
    // session.GetCompletions()
    $"{v.ReflectionValue}"
  | Choice.Choice1Of2 None, _ -> "()"
  | Choice.Choice2Of2 exn, errs ->
    let msgs = errs |> Array.toList |> List.map (fun e -> e.Message)
    $"{exn.Message} {msgs}"
