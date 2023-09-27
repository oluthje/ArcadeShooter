module HealthSystem

open Microsoft.Xna.Framework
open System

open ECS
open ECSTypes
open Components
open Microsoft.Xna.Framework.Graphics
open System.Diagnostics

let private removeDeadEntities (entities: Entity list) =
    entities
    |> List.filter (fun entity ->
        let h = getComponentFromEntity HealthComponent entity
        match (h) with
            | Some hl ->
                let healthComponent = unbox<HealthComponent> hl
                not (healthComponent.Health <= 0)
            | _ ->
                true
    )

let update (world: World) =
    let newEntities =
        world.Entities
        |> List.map (fun entity ->
            let h = getComponentFromEntity HealthComponent entity
            // let d = getComponentFromEntity DamageComponent entity
            let c = getComponentFromEntity CollisionComponent entity
            match (h, c) with
                | Some hl, Some col ->
                    let healthComponent = unbox<HealthComponent> hl
                    let collisionComponent = unbox<CollisionComponent> col
                    let damage = 1

                    let newHealthComponent =
                        if collisionComponent.Collided then
                            { healthComponent with Health = healthComponent.Health - damage }
                        else
                            healthComponent

                    let newCollisionComponent = { collisionComponent with Collided = false }

                    entity
                    |> addComponentToEntity CollisionComponent newCollisionComponent
                    |> addComponentToEntity HealthComponent newHealthComponent
                | _ ->
                    entity
        )

    { world with Entities = removeDeadEntities newEntities }