module CheckPlayerDeathSystem

open Microsoft.Xna.Framework
open ECSTypes
open ECS
open Components
open System.Diagnostics

let update (handlePlayerDeath: unit -> World) (world: World) =
    let playerEntity =
        world.Entities
        |> List.tryFind (fun entity -> entity.Type = Player)
    match playerEntity with
        | Some player ->
            let h = getComponentFromEntity HealthComponent player
            match h with
                | Some hl ->
                    let healthComponent = unbox<HealthComponent> hl
                    if healthComponent.Health <= 0 then
                        handlePlayerDeath()
                    else
                        world
                | _ ->
                    world
        | None ->
            world