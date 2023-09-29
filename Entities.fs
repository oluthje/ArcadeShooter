module Entities

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open System

open ECS
open Components

let createPlayer (graphicsDevice: GraphicsDevice) =
    let texture = new Texture2D(graphicsDevice, 1, 1)
    texture.SetData([| Color.Black |])
    let size = Point(32, 32)
    createEntity (Map.ofList [
        (SpriteComponent, createSpriteComponent texture (Vector2(0.0f, 0.0f)) (Vector2(1.0f, 1.0f)) (Point(32, 32)) Color.White (Vector2(0.0f, 0.0f)));
        (PlayerMovementComponent, createPlayerMovementComponent 200);
        (PositionComponent, createPositionComponent (Vector2(96.0f, 6f * 32.0f)));
        (MouseShootingComponent, createMouseShootingComponent (Vector2(0.0f, 0.0f)) 0.25 TimeSpan.Zero)
        (HealthComponent, createHealthComponent 3)
        (CollisionComponent, createCollisionComponent size)
        (FlashComponent, createFlashComponent 0.05 3)
    ]) EntityNames.Player

let createEnemySpawner =
    createEntity (Map.ofList [
        (EnemySpawnComponent, createEnemySpawnComponent TimeSpan.Zero 1.0)
    ]) EntityNames.EnemySpawner

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
        (FlashComponent, createFlashComponent 0.05 1)
    ]) EntityNames.Enemy

let createFastEnemyEntity (position: Vector2) (graphicsDevice: GraphicsDevice) =
    createEnemyEntity 1 Color.Pink 100 position graphicsDevice

let createSlowEnemyEntity (position: Vector2) (graphicsDevice: GraphicsDevice) =
    createEnemyEntity 3 Color.DarkRed 50 position graphicsDevice

let createBulletEntity (position: Vector2) (graphicsDevice: GraphicsDevice) (direction: Vector2) =
    let texture = new Texture2D(graphicsDevice, 1, 1)
    texture.SetData([| Color.Black |])
    let size = Point(8, 8)
    createEntity (Map.ofList [
        (SpriteComponent, createSpriteComponent texture (Vector2(0.0f, 0.0f)) (Vector2(1.0f, 1.0f)) (Point(8, 8)) Color.White (Vector2(0.0f, 0.0f)));
        (BulletComponent, createBulletComponent direction 700);
        (PositionComponent, createPositionComponent position);
        (CollisionComponent, createCollisionComponent size);
        (DamageComponent, createDamageComponent 1);
        (HealthComponent, createHealthComponent 1)
    ]) EntityNames.Bullet