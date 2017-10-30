#r @"packages\Fsharp.Data.dll"

open FSharp.Data

let [<Literal>] VotesUrl = "http://cspoll4.azurewebsites.net/api/votes"

type Votes = JsonProvider<VotesUrl>

let votes = Votes.GetSamples()

votes 
|> Seq.sortByDescending (fun v->  
    v.TotalScore, v.Vote.Version    
)
|> Seq.take 5
|> Seq.map(fun v -> 
    v.TotalScore, v.Count, v.Vote.Name
)
|> List.ofSeq
