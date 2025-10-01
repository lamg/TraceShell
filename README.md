# TraceShell

Ergonomic tracing of function calls and values in F# programs, i.e. print debugging on steroids.

Goals:

- Declare tracing sessions, including traced functions and variables
- Run tracing sessions with specific values and store the results
- Tools for getting insights from traces

## Example

Target code:

```fsharp
let simpleFun () =
  let xs = ["fun"; "with"; "F#"]
  xs |> List.map (fun x -> $"{x}!")
``

Tracing session 0:

```fsharp
let trace0 = trace ["simpleFun";"x"] simpleFun ()
show trace0
```

Output:

```
["fun"; "with"; "F#"]
```

This shows how any variable in `simpleFun` can be traced and inspected.
