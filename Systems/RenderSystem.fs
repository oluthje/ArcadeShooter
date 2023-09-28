module RenderSystem

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open System

open ECS
open ECSTypes
open Components

module RenderSystem =
    let draw (spriteBatch:SpriteBatch) (sprite:SpriteComponent) (offset:Vector2) =
        let spriteFrameRect = Rectangle(Point(0, 0), sprite.FrameSize)
        spriteBatch.Draw(sprite.Texture, sprite.Position + sprite.Offset + offset, Nullable.op_Implicit spriteFrameRect, sprite.Color, 0.f, Vector2.Zero, sprite.Scale, SpriteEffects.None, 0.f)

    let drawSprite (spriteBatch: SpriteBatch) (sprite: obj) (entity: Entity) =
        let positionOffset =
            match getComponentFromEntity PositionComponent entity with
                | Some position -> unbox<Vector2> position
                | None -> Vector2.Zero
        let spriteComponent = unbox<SpriteComponent> sprite
        draw spriteBatch spriteComponent positionOffset

    let updateFlashComponent (flashComponent: FlashComponent) (gameTime: GameTime) =
        if flashComponent.Running then
            if flashComponent.FlashTimer >= TimeSpan.FromSeconds flashComponent.FlashTime then
                let newFlashTimesLeft = flashComponent.FlashTimesLeft - 1
                let newFlashOn = not flashComponent.FlashOn
                let newTimer = TimeSpan.Zero
                if newFlashTimesLeft <= 0 then
                    { flashComponent with FlashTimer = newTimer; FlashTimesLeft = newFlashTimesLeft; FlashOn = newFlashOn; Running = false }
                else
                    { flashComponent with FlashTimer = newTimer; FlashTimesLeft = newFlashTimesLeft; FlashOn = newFlashOn }
            else
                { flashComponent with FlashTimer = flashComponent.FlashTimer + gameTime.ElapsedGameTime }
        else
            flashComponent

    let update (spriteBatch) (gameTime: GameTime) (world: World) =
        let newEntities =
            world.Entities
            |> List.map (fun entity ->
                let s = getComponentFromEntity SpriteComponent entity
                let f = getComponentFromEntity FlashComponent entity
                match (s, f) with
                    | Some sprite, Some flash ->
                        let flashComponent = unbox<FlashComponent> flash
                        let newFlashComponent = updateFlashComponent flashComponent gameTime
                        if not flashComponent.FlashOn then
                            drawSprite spriteBatch sprite entity
                        addComponentToEntity FlashComponent newFlashComponent entity
                    | Some someComponent, _ ->
                        drawSprite spriteBatch someComponent entity
                        entity
                    | None, _ ->
                        entity
            )
        { world with Entities = newEntities }