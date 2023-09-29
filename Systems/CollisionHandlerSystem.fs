module CollidedHandlerSystem

open Microsoft.Xna.Framework
open System

open ECS
open ECSTypes
open Components
open Microsoft.Xna.Framework.Graphics
open System.Diagnostics

let startFlashComponent (flashComponent) =
    { flashComponent with FlashTimesLeft = flashComponent.FlashTimes; FlashOn = true; Running = true }
let private removeDeadEntities (entities: Entity list) =
    entities
    |> List.filter (fun entity ->
        let h = getComponentFromEntity HealthComponent entity
        match (h) with
            | Some hl ->
                let healthComponent = unbox<HealthComponent> hl
                not (healthComponent.Health <= 0 && entity.Type <> Player)
            | _ ->
                true
    )

let handleEnemyCollidedWithBullet (enemy: Entity) =
    let h = getComponentFromEntity HealthComponent enemy
    let f = getComponentFromEntity FlashComponent enemy
    match h, f with
        | Some hl, Some fl ->
            let healthComponent = unbox<HealthComponent> hl
            let flashComponent = unbox<FlashComponent> fl
            let damage = 1
            let newHealthComponent = { healthComponent with Health = healthComponent.Health - damage }
            let newFlashComponent = startFlashComponent flashComponent

            enemy
            |> addComponentToEntity HealthComponent newHealthComponent
            |> addComponentToEntity FlashComponent newFlashComponent
        | _ ->
            enemy

let handleEnemyCollidedWithPlayer (enemy: Entity) =
    let h = getComponentFromEntity HealthComponent enemy
    let f = getComponentFromEntity FlashComponent enemy
    match h, f with
        | Some hl, Some fl ->
            let healthComponent = unbox<HealthComponent> hl
            let flashComponent = unbox<FlashComponent> fl
            let newHealthComponent = { healthComponent with Health = 0 }
            let newFlashComponent = startFlashComponent flashComponent

            enemy
            |> addComponentToEntity HealthComponent newHealthComponent
            |> addComponentToEntity FlashComponent newFlashComponent
        | _ ->
            enemy

let handlePlayerCollidedWithEnemy (player: Entity) =
    let h = getComponentFromEntity HealthComponent player
    let f = getComponentFromEntity FlashComponent player
    match (h, f) with
        | Some hl, Some fl ->
            let healthComponent = unbox<HealthComponent> hl
            let flashComponent = unbox<FlashComponent> fl
            let damage = 1
            let newHealthComponent =
                if (healthComponent.Health - damage) <= 0 then
                    { healthComponent with Health = 1 }
                else
                    { healthComponent with Health = healthComponent.Health - damage }
            let newFlashComponent = startFlashComponent flashComponent

            player
            |> addComponentToEntity HealthComponent newHealthComponent
            |> addComponentToEntity FlashComponent newFlashComponent
        | _ ->
            player

let handleBulletCollidedWithEnemy (bullet: Entity) =
    let h = getComponentFromEntity HealthComponent bullet
    match h with
        | Some hl ->
            let healthComponent = unbox<HealthComponent> hl
            let newHealthComponent = { healthComponent with Health = 0 }

            bullet
            |> addComponentToEntity HealthComponent newHealthComponent
        | _ ->
            bullet

let update (world: World) =
    let newEntities =
        world.Entities
        |> List.map (fun entity ->
            let c = getComponentFromEntity CollisionComponent entity
            match c with
                | Some col ->
                    let collisionComponent = unbox<CollisionComponent> col
                    if collisionComponent.Collided then
                        match entity.Type, collisionComponent.Collider with
                            | Enemy, Bullet ->
                                handleEnemyCollidedWithBullet entity
                            | Enemy, Player ->
                                handleEnemyCollidedWithPlayer entity
                            | Player, Enemy ->
                                handlePlayerCollidedWithEnemy entity
                            | Bullet, Enemy ->
                                handleBulletCollidedWithEnemy entity
                            | _ ->
                                entity
                    else entity
                | _ ->
                    entity
        )

    { world with Entities = removeDeadEntities newEntities }