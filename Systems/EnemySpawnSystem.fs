module EnemySpawnSystem

open Microsoft.Xna.Framework
open System

open ECS
open ECSTypes
open Components
open Entities
open Microsoft.Xna.Framework.Graphics
open System.Diagnostics

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
                            let random = Random()
                            let enemyType = random.Next(3)
                            let enemyEntity =
                                match enemyType with
                                    | 0 -> createSlowEnemyEntity (getRandomEnemySpawnPosition graphicsDevice) graphicsDevice
                                    | _ -> createFastEnemyEntity (getRandomEnemySpawnPosition graphicsDevice) graphicsDevice

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