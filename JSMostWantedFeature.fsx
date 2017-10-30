// Type providers
#r @"packages\Fsharp.Data.dll"

open FSharp.Data
open System.Diagnostics

// value assigned during compilation
let [<Literal>] proposalUrl = "https://api.github.com/search/repositories?q=tc39/proposal"

type Proposals = JsonProvider<proposalUrl>

let resp = Proposals.GetSample()

resp.Items
|> Seq.map(fun (item: Proposals.Item) ->
    item.HtmlUrl, item.StargazersCount
)
|> Seq.sortByDescending snd
|> Seq.take 5
|> Seq.map fst
|> Seq.iter (fun i -> 
    Process.Start("chrome.exe", i) |> ignore
) 
