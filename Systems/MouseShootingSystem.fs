module MouseShootingSystem

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open System

open ECS
open ECSTypes
open Components
open Microsoft.Xna.Framework.Graphics
open System.Diagnostics

let spaceBarPressed (state: KeyboardState) =
    state.IsKeyDown Keys.Space

let private createBulletEntity (position: Vector2) (graphicsDevice: GraphicsDevice) (direction: Vector2) =
    let texture = new Texture2D(graphicsDevice, 1, 1)
    texture.SetData([| Color.Black |])
    let size = Point(8, 8)
    createEntity (Map.ofList [
        (SpriteComponent, { Texture = texture; Position = Vector2(0.0f, 0.0f); Scale = Vector2(1.0f, 1.0f); FrameSize = Point(8, 8); Color = Color.White; Offset = Vector2(0.0f, 0.0f) });
        (BulletComponent, { Direction = direction; Speed = 700 });
        (PositionComponent, position);
        (CollisionComponent, { Size = size; Collided = false });
        (DamageComponent, { Damage = 1 })
        (HealthComponent, { Health = 1 })
    ])

let update (gameTime: GameTime) (graphicsDevice: GraphicsDevice) (world: World)  =
    let mutable addedEntities : Entity list = []
    let newEntities =
        world.Entities
        |> List.map (fun entity ->
            match (getComponentFromEntity MouseShootingComponent entity, getComponentFromEntity PositionComponent entity) with
                | Some shoot, Some position ->
                    let mouseShootingComponent = unbox<MouseShootingComponent> shoot
                    let positionComponent = unbox<Vector2> position
                    let mouseDirection = Mouse.GetState().Position.ToVector2() - positionComponent
                    if mouseDirection <> Vector2.Zero then mouseDirection.Normalize()
                    let bulletPosition = positionComponent + mouseShootingComponent.Offset
                    let newCoolDownTimer =
                        if spaceBarPressed (Keyboard.GetState()) && mouseShootingComponent.CoolDownTimer >= TimeSpan.FromSeconds mouseShootingComponent.CoolDownTime then
                            let bulletEntity = createBulletEntity bulletPosition graphicsDevice mouseDirection
                            addedEntities <- bulletEntity :: addedEntities
                            TimeSpan.Zero
                        else
                            mouseShootingComponent.CoolDownTimer + gameTime.ElapsedGameTime
                    let newMouseShootingComponent = { mouseShootingComponent with CoolDownTimer = newCoolDownTimer }
                    addComponentToEntity MouseShootingComponent newMouseShootingComponent entity
                | _, _ ->
                    entity
        )
    { world with Entities = newEntities @ addedEntities }