module Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
open System

open ECS
open Components
open RenderSystem
open MovementSystem
open EnemyMovementSystem
open MouseShootingSystem
open EnemySpawnSystem
open CollisionSystem
open HealthSystem
open ECSTypes

// current goal: just get player to move around
// next goal: have player shoot in movement direction

type Game1 () as x =
    inherit Game()

    do x.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(x)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let mutable world : World = { Entities = [] }

    override x.Initialize() =

        spriteBatch <- new SpriteBatch(x.GraphicsDevice)
        base.Initialize()

        x.IsMouseVisible <- true

        let texture = new Texture2D(x.GraphicsDevice, 1, 1)
        texture.SetData([| Color.Black |])
        let playerEntity =
            createEntity (Map.ofList [
                (SpriteComponent, { Texture = texture; Position = Vector2(0.0f, 0.0f); Scale = Vector2(1.0f, 1.0f); FrameSize = Point(32, 32); Color = Color.White; Offset = Vector2(0.0f, 0.0f) });
                (PlayerMovementComponent, { Speed = 200 });
                (PositionComponent, Vector2(96.0f, 6f * 32.0f));
                (MouseShootingComponent, { Offset = Vector2(0.0f, 0.0f); CoolDownTime = 0.25; CoolDownTimer = TimeSpan.Zero })
            ])

        let enemySpawnerEntity =
            createEntity (Map.ofList [
                (EnemySpawnComponent, { SpawnTimer = TimeSpan.Zero; SpawnTime = 1.0 })
            ])

        let entities =
            [ playerEntity ]
            |> List.append [ enemySpawnerEntity ]

        world <- { world with Entities = entities @ world.Entities }

    override x.LoadContent() =
        ()

    override x.Update (gameTime: GameTime) =
        base.Update(gameTime)

        world <-
            world
            |> MovementSystem.update gameTime
            |> MouseShootingSystem.update gameTime x.GraphicsDevice
            |> EnemyMovementSystem.update gameTime
            |> EnemySpawnSystem.update gameTime x.GraphicsDevice
            |> CollisionSystem.update
            |> HealthSystem.update

    override x.Draw (gameTime: GameTime) =
        x.GraphicsDevice.Clear(Color.CornflowerBlue)
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null)

        world <-
            world
            |> RenderSystem.update spriteBatch

        spriteBatch.End()
        base.Draw(gameTime)
