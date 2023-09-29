module Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content
open System

open RenderSystem
open ECSTypes
open Entities

type Game1 () as x =
    inherit Game()

    do x.Content.RootDirectory <- "Content"
    let graphics = new GraphicsDeviceManager(x)
    let mutable spriteBatch = Unchecked.defaultof<SpriteBatch>
    let mutable world : World = { Entities = [] }

    let startGame () =
        let entities =
            [ createPlayer x.GraphicsDevice ]
            @ [ createEnemySpawner ]

        { world with Entities = entities }

    override x.Initialize() =
        spriteBatch <- new SpriteBatch(x.GraphicsDevice)
        base.Initialize()
        x.IsMouseVisible <- true

        world <- startGame()

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
            |> CollisionSystem.startUpdate // CALL BEFORE COLLISION CHECKS
            |> EnemySystem.update
            |> CollidedHandlerSystem.update
            |> CollisionSystem.endUpdate // CALL AFTER COLLISION CHECKS
            |> CheckPlayerDeathSystem.update startGame

    override x.Draw (gameTime: GameTime) =
        x.GraphicsDevice.Clear(Color.CornflowerBlue)
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null)

        world <-
            world
            |> RenderSystem.update spriteBatch gameTime

        spriteBatch.End()
        base.Draw(gameTime)
