module EnemySpawnSystem

open Microsoft.Xna.Framework
open System

open ECS
open ECSTypes
open Components
open Microsoft.Xna.Framework.Graphics
open System.Diagnostics

let private createEnemyEntity (position: Vector2) (graphicsDevice: GraphicsDevice) =
    let texture = new Texture2D(graphicsDevice, 1, 1)
    texture.SetData([| Color.Black |])
    let size = Point(32, 32)
    createEntity (Map.ofList [
        (SpriteComponent, { Texture = texture; Position = Vector2(0.0f, 0.0f); Scale = Vector2(1.0f, 1.0f); FrameSize = Point(32, 32); Color = Color.OrangeRed; Offset = Vector2(0.0f, 0.0f) });
        (ChasePlayerComponent, { Speed = 50; Chase = true });
        (PositionComponent, position);
        (HealthComponent, { Health = 3 });
        (DamageComponent, { Damage = 1 });
        (CollisionComponent, { Size = size; Collided = false });
        (FlashComponent, { FlashTimer = TimeSpan.Zero; FlashTime = 0.1; FlashTimes = 2; FlashTimesLeft = 0 })
    ])

let private getRandomEnemySpawnPosition (graphicsDevice:GraphicsDevice) =
    let screenWidth = float graphicsDevice.Viewport.Width
    let screenHeight = float graphicsDevice.Viewport.Height
    let random = Random()
    let side = random.Next(4)
    let position =
        match side with
            | 0 -> Vector2(float32 (random.NextDouble() * screenWidth), 0.0f)
            | 1 -> Vector2(float32 screenWidth, float32 (random.NextDouble() * screenHeight))
            | 2 -> Vector2(float32(random.NextDouble() * screenWidth), float32 screenHeight)
            | _ -> Vector2(0.0f, float32(random.NextDouble() * screenHeight))
    position

let update (gameTime: GameTime) (graphicsDevice: GraphicsDevice) (world: World)  =
    let mutable addedEntities: Entity list = []
    let newEntities =
        world.Entities
        |> List.map (fun entity ->
            let s = getComponentFromEntity EnemySpawnComponent entity
            match (s) with
                | Some spawner->
                    let enemySpawnerEntity = unbox<EnemySpawnComponent> spawner
                    let newSpawnTimer =
                        if enemySpawnerEntity.SpawnTimer >= TimeSpan.FromSeconds enemySpawnerEntity.SpawnTime then
                            let enemyEntity = createEnemyEntity(getRandomEnemySpawnPosition graphicsDevice) graphicsDevice
                            addedEntities <- enemyEntity :: addedEntities
                            TimeSpan.Zero
                        else
                            enemySpawnerEntity.SpawnTimer + gameTime.ElapsedGameTime
                    let newSpawner = { enemySpawnerEntity with SpawnTimer = newSpawnTimer }
                    addComponentToEntity EnemySpawnComponent newSpawner entity
                | _ ->
                    entity
        )
    { world with Entities = newEntities @ addedEntities }