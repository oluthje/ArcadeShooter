module EnemyMovementSystem

open Microsoft.Xna.Framework
open System

open ECS
open ECSTypes
open Components

let private moveTo (originPos: Vector2) (targetPos: Vector2) (speed: float) (gameTime: GameTime) =
    let direction = targetPos - originPos
    let velocity =
        if direction <> Vector2.Zero then direction.Normalize()
        direction * float32 speed
    originPos + velocity * float32 gameTime.ElapsedGameTime.TotalSeconds

let update (gameTime: GameTime) (world: World)  =
    let newEntities =
        world.Entities
        |> List.map (fun entity ->
            let c = getComponentFromEntity ChasePlayerComponent entity
            let p = getComponentFromEntity PositionComponent entity
            match (c, p) with
                | Some chase, Some pos ->
                    let chasePlayerComponent = unbox<ChasePlayerComponent> chase
                    let positionComponent = unbox<Vector2> pos
                    let playerPosition =
                        match getEntityWithComponent PlayerMovementComponent world with
                            | Some player ->
                                let p = unbox<Entity> player
                                match getComponentFromEntity PositionComponent p with
                                    | Some position -> unbox<Vector2> position
                                    | None -> failwith "Player position not found"
                            | None -> failwith "Player not found"

                    let newPosition = moveTo positionComponent playerPosition chasePlayerComponent.Speed gameTime
                    addComponentToEntity PositionComponent newPosition entity
                | _, _ ->
                    entity
        )
    { world with Entities = newEntities }