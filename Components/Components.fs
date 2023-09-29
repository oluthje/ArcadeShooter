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

type EntityNames =
    | Player
    | EnemySpawner
    | Enemy
    | Bullet
    | NoEntity

type PlayerMovementComponent = {
    Speed: float
}

type ChasePlayerComponent = {
    Speed: float
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
    Collider: EntityNames
    Size: Point
}

type FlashComponent = {
    FlashTimer: TimeSpan
    FlashTime: float
    FlashTimes: int
    FlashTimesLeft: int
    FlashOn: bool
    Running: bool
}

let createSpriteComponent (texture: Texture2D) (position: Vector2) (scale: Vector2) (frameSize: Point) (color: Color) (offset: Vector2) : SpriteComponent =
    { Texture = texture; Position = position; Scale = scale; FrameSize = frameSize; Color = color; Offset = offset }

let createPlayerMovementComponent (speed: float) : PlayerMovementComponent =
    { Speed = speed }

let createChasePlayerComponent (speed: float) : ChasePlayerComponent =
    { Speed = speed }

let createPositionComponent (position: Vector2) : Vector2 =
    position

let createMouseShootingComponent (offset: Vector2) (coolDownTime: float) (coolDownTimer: TimeSpan) : MouseShootingComponent =
    { Offset = offset; CoolDownTime = coolDownTime; CoolDownTimer = coolDownTimer }

let createHealthComponent (health: int) : HealthComponent =
    { Health = health }

let createEnemySpawnComponent (spawnTimer: TimeSpan) (spawnTime: float) : EnemySpawnComponent =
    { SpawnTimer = spawnTimer; SpawnTime = spawnTime }

let createDamageComponent (damage: int) : DamageComponent =
    { Damage = damage }

let createCollisionComponent (size: Point) : CollisionComponent =
    { Size = size; Collided = false; Collider = NoEntity }

let createFlashComponent (flashTime: float) (flashTimes: int) : FlashComponent =
    { FlashTimer = TimeSpan.Zero; FlashTime = flashTime; FlashTimes = flashTimes; FlashTimesLeft = 0; FlashOn = false; Running = false }

let createBulletComponent (direction: Vector2) (speed: float) : BulletComponent =
    { Direction = direction; Speed = speed }