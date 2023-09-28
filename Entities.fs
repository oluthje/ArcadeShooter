module Entities

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open System

open ECS
open Components

let createPlayer (graphicsDevice: GraphicsDevice) =
    let texture = new Texture2D(graphicsDevice, 1, 1)
    texture.SetData([| Color.Black |])
    let playerMovementComp : PlayerMovementComponent = { Speed = 200 }
    createEntity (Map.ofList [
        (SpriteComponent, createSpriteComponent texture (Vector2(0.0f, 0.0f)) (Vector2(1.0f, 1.0f)) (Point(32, 32)) Color.White (Vector2(0.0f, 0.0f)));
        (PlayerMovementComponent, createPlayerMovementComponent 200);
        (PositionComponent, createPositionComponent (Vector2(96.0f, 6f * 32.0f)));
        (MouseShootingComponent, createMouseShootingComponent (Vector2(0.0f, 0.0f)) 0.25 TimeSpan.Zero)
        (HealthComponent, createHealthComponent 9)
    ])

let createEnemySpawner =
    createEntity (Map.ofList [
        (EnemySpawnComponent, createEnemySpawnComponent TimeSpan.Zero 1.0)
    ])

let createEnemyEntity (health: int) (color: Color) (speed: float) (position: Vector2) (graphicsDevice: GraphicsDevice) =
    let texture = new Texture2D(graphicsDevice, 1, 1)
    texture.SetData([| color |])
    let size = Point(32, 32)
    createEntity (Map.ofList [
        (SpriteComponent, createSpriteComponent texture (Vector2(0.0f, 0.0f)) (Vector2(1.0f, 1.0f)) (Point(32, 32)) Color.OrangeRed (Vector2(0.0f, 0.0f)));
        (ChasePlayerComponent, createChasePlayerComponent speed);
        (PositionComponent, createPositionComponent position);
        (HealthComponent, createHealthComponent health);
        (DamageComponent, createDamageComponent 1);
        (CollisionComponent, createCollisionComponent size);
        (FlashComponent, createFlashComponent TimeSpan.Zero 0.05 1 0 false false)
    ])

let createFastEnemyEntity (position: Vector2) (graphicsDevice: GraphicsDevice) =
    createEnemyEntity 1 Color.Pink 100 position graphicsDevice

let createSlowEnemyEntity (position: Vector2) (graphicsDevice: GraphicsDevice) =
    createEnemyEntity 3 Color.DarkRed 50 position graphicsDevice