// Types - Records
// The forward pipe operator
// Function compositiion

open System

let names = [|
    "stefan uzunov, m, 32"
    "gergana ivanova, f, 42"
    "hristo velev, o, 29"
    |]


// first type - Record
type Person = {
    Name: string
    Gender:string
    Age: int
}

// 1. type inference needs help with objects` methods
// let splitStr delimiters str =
//     str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)

// 2. type inference needs help with method overloading    
// let splitStr delimiters (str: string) =
//     str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)

let splitStr (delimiters: char []) (str: string) =
    str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)

let trim (str: string) = str.Trim()
 

// normal way
// let getPersonInfo info = splitStr [|','|] info


// Now, lets start using partial application
// first-class function can be bint to identifier 
// Partial application helps in specializing functions 
// improves readability
let getPersonInfo = splitStr [|','|]

// initial declaration - before removing the lambda

// let getPerson person =
//     let personInfo = 
//         Array.map (fun info -> trim info) (getPersonInfo person)
//     {
//         Name = personInfo.[0]
//         Gender = personInfo.[1]
//         Age = int (personInfo.[2])
//     }
let getPerson person =
    let personInfo = Array.map trim (getPersonInfo person)
    {
        Name = personInfo.[0]
        Gender = personInfo.[1]
        Age = int (personInfo.[2])
    }


// test 
// let personsInfo = 
//     Array.map getPerson names

(*
    Read the file with people and get personsInfo
*)

// In demo no problem, but in real project at start
open System.IO

let personInfos = 
    Array.map getPerson (File.ReadAllLines "names.txt")

(*
    // Capitalize first name
    for given member of the list
    get people with 3th most met first name ordered by age and displayed:
    (Name: LastName, FirstName; Age: age)
    group by first name
    
    display by (Name: LastName, FirstName; Age: age)

*)

// function specialization
let getFirstAndLastName = splitStr [|' '|]
let capitalize (str: string) = 
    string (str.ToUpper().[0]) + str.[1..]

let byFirstName person = 
    (getFirstAndLastName person.Name).[0]

let peopleByFirstName = Array.groupBy byFirstName personInfos

let thirdMostCommonFirstNamePeople =
    let sortedPeople = 
        // Tuple destructuring in lambda`s argument
        Array.sortBy (fun (_, people) -> Array.length people) peopleByFirstName
    let (_, people) = sortedPeople.[2]
    people    

let toDisplayFormat person =
    let firstAndlastName = 
        Array.map capitalize (getFirstAndLastName (person.Name))

    sprintf "Name: %s, %s; Age: %i"
        (Array.last firstAndlastName)
        (Array.head firstAndlastName)
        (person.Age)

let res = Array.map toDisplayFormat thirdMostCommonFirstNamePeople

// Can we do better?

// our first library - fluent interfaces
// little offtopic - prefix vs inflix
5 + 6 // inflix
(+) 5 6 // prefix

// forward-pipe
let (|>) x f = f x


let getThirdMostCommonFirstNamePeople peopleByFirstName =
    let sortedPeople = 
        // In demo: refactor using function composition
        Array.sortByDescending (fun (_, people) -> 
            Array.length people
        ) peopleByFirstName

    let (_, people) = sortedPeople.[2]
    people    


(File.ReadAllLines "names.txt") 
|> Array.map getPerson 
|> Array.groupBy byFirstName
|> getThirdMostCommonFirstNamePeople
|> Array.map toDisplayFormat


// What else can be improved?

// function-composition
let (>>) f g x = g (f x) 

// refactor original function to be highly functional
let getThirdMostCommonFirstNamePeopleRef =
    Array.sortByDescending (snd >> Array.length)
    >> Array.item 3
    >> snd 

let toDisplayFormatRef person =
    let firstAndlastName = 
        (getFirstAndLastName >> Array.map capitalize) person.Name

    sprintf "Name: %s, %s; Age: %i"
        (Array.last firstAndlastName)
        (Array.head firstAndlastName)
        (person.Age)

(File.ReadAllLines "names.txt") 
|> Array.map getPerson 
|> Array.groupBy byFirstName
|> getThirdMostCommonFirstNamePeopleRef
|> Array.map toDisplayFormat
