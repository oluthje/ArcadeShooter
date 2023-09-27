module MovementSystem

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open System

open ECS
open ECSTypes
open Components

let private getMovementVector (state: KeyboardState) =
    if state.IsKeyDown Keys.W && state.IsKeyDown Keys.A then Vector2(-1.f, -1.f)
    elif state.IsKeyDown Keys.W && state.IsKeyDown Keys.D then Vector2(1.f, -1.f)
    elif state.IsKeyDown Keys.S && state.IsKeyDown Keys.A then Vector2(-1.f, 1.f)
    elif state.IsKeyDown Keys.S && state.IsKeyDown Keys.D then Vector2(1.f, 1.f)
    elif state.IsKeyDown Keys.W then Vector2(0.f, -1.f)
    elif state.IsKeyDown Keys.S then Vector2(0.f, 1.f)
    elif state.IsKeyDown Keys.A then Vector2(-1.f, 0.f)
    elif state.IsKeyDown Keys.D then Vector2(1.f, 0.f)
    else Vector2.Zero

let update (gameTime: GameTime) (world: World)  =
    let newEntities =
        world.Entities
        |> List.map (fun entity ->
            match (getComponentFromEntity PlayerMovementComponent entity, getComponentFromEntity PositionComponent entity, getComponentFromEntity BulletComponent entity) with
                | Some movement, Some position, _ ->
                    let movementComponent = unbox<PlayerMovementComponent> movement
                    let positionComponent = unbox<Vector2> position
                    let movementVector = getMovementVector (Keyboard.GetState())
                    let velocity =
                        if movementVector <> Vector2.Zero then movementVector.Normalize()
                        movementVector * float32 movementComponent.Speed
                    let newPosition = positionComponent + velocity * float32 gameTime.ElapsedGameTime.TotalSeconds
                    addComponentToEntity PositionComponent newPosition entity
                | _, Some position, Some bullet ->
                    let positionComponent = unbox<Vector2> position
                    let bulletComponent = unbox<BulletComponent> bullet
                    let newPosition = positionComponent + bulletComponent.Direction * float32 bulletComponent.Speed * float32 gameTime.ElapsedGameTime.TotalSeconds
                    addComponentToEntity PositionComponent newPosition entity
                | _, _, _ ->
                    entity
        )
    { world with Entities = newEntities }