open TraceShell

[<EntryPoint>]
let main _ =
  let session = FsEval.makeSession ()
  REPL.repl (FsEval.eval session)
  0
