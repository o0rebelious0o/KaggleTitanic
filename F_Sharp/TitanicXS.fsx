﻿//
// Titanic: Machine Learning from Disaster 
//

#load "TallyHo.fs"
#load "DecisionTree.fs"
#load "Passengers.fs"
open TallyHo
open DecisionTree
open Passengers

// Load training data

let passengers = allPassengers.[0..599]

// 1. Discover statistics - simple features

let female (passenger:Passenger) = (passenger.Sex = Female)
let survived (passenger:Passenger) = passenger.Survived

// Female passengers

let females = passengers |> where female
let femaleSurvivors = females |> tally survived
let femaleSurvivorsPc = females |> percentage survived

// a) Children under 10
// Your code here <----

// b) Passesngers over 50
// Your code hre <--

// c) Upper class passengers
// Your code here <---


// 2. Discover statistics - groups  

/// Survival rate of a criterias group
let survivalRate criteria = 
    passengers |> Array.groupBy criteria 
    |> Array.map (fun (key,matching) -> 
        key, matching |> Array.percentage survived
    )

let embarked = survivalRate (fun p -> p.Embarked)

// a) By passenger class
// Your code here <---

// b) By age group (under 10, adult, over 50)
// Your code here <---


// 3. Scoring

let testPassengers = allPassengers.[600..]

let score f = testPassengers |> Array.percentage (fun p -> f p = survived p)

let notSurvived (p:Passenger) = false

let notSurvivedRate = score notSurvived

// a) Score by embarked point
// Your code here <---

// b) Construct function to score over 80%
// Your code here <---


// 4. Decision trees

let labels = 
    [|"sex"; "class"|]

let features (p:Passenger) : obj[] = 
    [|p.Sex; p.Class|]

let dataSet : obj[][] =
    [|for p in passengers ->
        [|yield! features p; 
          yield box (p.Survived)|] |]

let tree = createTree(dataSet, labels)

// Classify

let test (p:Passenger) = 
    match classify(tree, labels, features p) with
    | Some(x) -> x
    | None -> mode dataSet
    :?> bool

let treeRate = score test

// a) Optimize features
