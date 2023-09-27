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

    let update (spriteBatch) (world: World) =
        world.Entities
        |> List.iter (fun entity ->
            match Map.tryFind SpriteComponent entity.Components with
                | Some(someComponent) ->
                    let positionOffset =
                        match getComponentFromEntity PositionComponent entity with
                            | Some position -> unbox<Vector2> position
                            | None -> Vector2.Zero
                    let spriteComponent = unbox<SpriteComponent> someComponent
                    draw spriteBatch spriteComponent positionOffset
                | None ->
                    ()
        )
        world