module EnemySystem

open Microsoft.Xna.Framework
open System

open ECS
open ECSTypes
open Components

let update (world: World)  =
    // let newEntities =
    //     world.Entities
    //     |> List.map (fun entity ->
    //         let f = getComponentFromEntity FlashComponent entity
    //         let c = getComponentFromEntity CollisionComponent entity
    //         let t = entity.Type
    //         match (f, c, t) with
    //             | Some flash, Some col, Enemy ->
    //                 let flashComponent = unbox<FlashComponent> flash
    //                 let collisionComponent = unbox<CollisionComponent> col
    //                 if collisionComponent.Collided then
    //                     let newFlashComponent = { flashComponent with FlashTimesLeft = flashComponent.FlashTimes; FlashOn = true; Running = true }
    //                     addComponentToEntity FlashComponent newFlashComponent entity
    //                 else
    //                     entity
    //             | _ ->
    //                 entity
    //     )
    // { world with Entities = newEntities }
    world