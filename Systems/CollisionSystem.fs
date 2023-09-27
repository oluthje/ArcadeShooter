module CollisionSystem

open Microsoft.Xna.Framework
open System

open ECS
open ECSTypes
open Components
open System.Diagnostics

let private isColliding (entity1: Entity) (entity2: Entity) =
    let p1 = getComponentFromEntity PositionComponent entity1
    let p2 = getComponentFromEntity PositionComponent entity2
    let c1 = getComponentFromEntity CollisionComponent entity1
    let c2 = getComponentFromEntity CollisionComponent entity2
    match (p1, p2, c1, c2) with
        | (Some pos1, Some pos2, Some col1, Some col2) ->
            let position1 = unbox<Vector2> pos1
            let position2 = unbox<Vector2> pos2
            let collisionComponent1 = unbox<CollisionComponent> col1
            let collisionComponent2 = unbox<CollisionComponent> col2
            let collisionRectangle1 = new Rectangle(int position1.X, int position1.Y, collisionComponent1.Size.X, collisionComponent1.Size.Y)
            let collisionRectangle2 = new Rectangle(int position2.X, int position2.Y, collisionComponent2.Size.X, collisionComponent2.Size.Y)
            collisionRectangle1.Intersects(collisionRectangle2)
        | _ ->
            false

let private removeCollidedEntities (entities: Entity list) =
    entities
    |> List.filter (fun entity ->
        let c = getComponentFromEntity CollisionComponent entity
        match (c) with
            | Some col ->
                let collisionComponent = unbox<CollisionComponent> col
                not collisionComponent.Collided
            | _ ->
                true
    )

let update (world: World) =
    let collisionEntities =
        world.Entities
        |> List.filter (fun entity ->
            let p = getComponentFromEntity PositionComponent entity
            let c = getComponentFromEntity CollisionComponent entity
            match (p, c) with
                | (Some pos, Some col) ->
                    true
                | _ ->
                    false
        )

    let newEntities =
        world.Entities
        |> List.map (fun entity ->
            let c = getComponentFromEntity CollisionComponent entity
            match (c) with
                | Some col ->
                    let collisionComponent = unbox<CollisionComponent> col
                    let isColliding =
                        let rec check entities =
                            match entities with
                                | otherEntity :: rest ->
                                    if entity.Id <> otherEntity.Id && isColliding entity otherEntity then
                                        true
                                    else check rest
                                | [] ->
                                    false
                        check collisionEntities

                    let newCollisionComponent = { collisionComponent with Collided = isColliding }
                    addComponentToEntity CollisionComponent newCollisionComponent entity
                | _ ->
                    entity
        )

    { world with Entities = newEntities }