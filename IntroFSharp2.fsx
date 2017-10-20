// Access to .NET ecosystem
// Next Everything is expression 

open System
open System.IO

let maleFirstNames = 
    [|
        "georgi"
        "hristo"
        "dimitar"
        "borislav"
        "zdravko"
        "miladin"
        "samuil"
        "kiro"    
    |]

let maleLastNames = [|
    "georgiev"
    "ivanov"
    "jelqzkov"
    "stoqnov"
    "zdravkov"
    "vladimirov"
    "simeonov"
    "kirov"    
|]
let femaleFirstNames = [|
    "gergana"
    "hristina"
    "monika"
    "nadejda"
    "miroslava"
    "petya"
    "elena"
    "izabela"    
|]

let femaleLastNames = [|
    "georgieva"
    "ivanova"
    "trifonova"
    "nikolova"
    "evtimova"
    "danova"
    "popova"
    "ninova"    
|]

let genders = [|"m";"f"|]

let generatePerson (rand: Random) =     
    let getRandomValue (names: 'a []) =
        let i = rand.Next (names.Length)
        names.[i]

    let personGender = getRandomValue genders
    let (firstNameArr, lastNameArr) = 
        if personGender = "m" then
            maleFirstNames, maleLastNames
        else 
            femaleFirstNames, femaleLastNames 

    sprintf "%s %s, %s, %i"
        (getRandomValue firstNameArr)
        (getRandomValue lastNameArr)
        personGender
        (getRandomValue [|0..99|])    

let generatePersons count = 
    let rand = Random()
    Array.init count (fun _ -> generatePerson rand)

// #time
File.WriteAllLines ("names.txt", generatePersons 2000)
