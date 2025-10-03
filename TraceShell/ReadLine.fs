module TraceShell.ReadLine

open System

type Context =
  { prompt: string
    linePos: int // 0 <= linePos < line.Length
    history: string list
    line: string }

  member this.moveCursorRight() =
    match this.linePos with
    | _ when this.linePos = this.line.Length -> this
    | _ ->
      Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop)

      { this with linePos = this.linePos + 1 }

  member this.moveCursorLeft() =
    match this.linePos with
    | 0 -> this
    | _ ->
      Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)

      { this with linePos = this.linePos - 1 }

  member this.removeCharLeft() =
    match this.linePos with
    | 0 -> this
    | _ ->
      let withoutChar = this.line.Remove(this.linePos - 1, 1)
      Console.Write "\b \b"
      let newPos = this.linePos - 1

      { this with
          line = withoutChar
          linePos = newPos }

  member this.removeCharRight() =
    match this.line with
    | _ when this.linePos >= this.line.Length -> this
    | _ ->
      let head = this.line.Substring(0, this.linePos)

      let tail =
        this.line.Substring(this.linePos + 1, this.line.Length - this.linePos - 1)

      Console.Write $"{tail} "
      Console.CursorLeft <- Console.CursorLeft - tail.Length - 1

      { this with line = head + tail }

  member this.enterLine() =
    Console.CursorLeft <- this.prompt.Length
    Console.Write(String.replicate this.line.Length " ")
    Console.CursorLeft <- this.prompt.Length

    { this with
        line = ""
        linePos = 0
        history = this.line :: this.history }

let handleKey (ctx: Context) (key: ConsoleKeyInfo) =
  match key.Key with
  | ConsoleKey.Backspace -> ctx.removeCharLeft ()
  | ConsoleKey.Delete -> ctx.removeCharRight ()
  | ConsoleKey.LeftArrow -> ctx.moveCursorLeft ()
  | ConsoleKey.RightArrow -> ctx.moveCursorRight ()
  | ConsoleKey.Enter -> ctx.enterLine ()
  | _ ->
    Console.Write key.KeyChar

    { ctx with
        line = ctx.line + string key.KeyChar
        linePos = ctx.linePos + 1 }

let rec readLoop (ctx: Context) =
  let key = Console.ReadKey true

  match key.Key with
  | ConsoleKey.D when key.Modifiers = ConsoleModifiers.Control -> ()
  | _ ->
    let nctx = handleKey ctx key
    readLoop nctx

let start () =
  let handler _sender (args: ConsoleCancelEventArgs) = args.Cancel <- false
  Console.CancelKeyPress.AddHandler handler

  let ctx =
    { history = []
      line = ""
      linePos = 0
      prompt = ">>> " }

  Console.Write ctx.prompt
  readLoop ctx
