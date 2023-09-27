module Components

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open System

type Component =
    | PlayerMovementComponent
    | SpriteComponent
    | PositionComponent
    | MouseShootingComponent
    | BulletComponent
    | ChasePlayerComponent
    | HealthComponent
    | DamageComponent
    | EnemySpawnComponent
    | CollisionComponent
    | FlashComponent
    // | RemoveEntityComponent

type PlayerMovementComponent = {
    Speed: float
}

type ChasePlayerComponent = {
    Speed: float
    Chase: bool
}

type SpriteComponent = {
    Texture: Texture2D
    Position: Vector2
    Scale:Vector2
    FrameSize:Point
    Color:Color
    Offset:Vector2
}

type MouseShootingComponent = {
    Offset: Vector2
    CoolDownTime: float
    CoolDownTimer: TimeSpan
}

type BulletComponent = {
    Direction: Vector2
    Speed: float
}

type HealthComponent = {
    Health: int
}

type DamageComponent = {
    Damage: int
}

type EnemySpawnComponent = {
    SpawnTimer: TimeSpan
    SpawnTime: float
}

type CollisionComponent = {
    Collided: bool
    Size: Point
}

type FlashComponent = {
    FlashTimer: TimeSpan
    FlashTime: float
    FlashTimes: int
    FlashTimesLeft: int
}