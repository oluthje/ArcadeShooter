module ECS
open System
open Components
open ECSTypes
open System.Diagnostics

let getComponentFromEntity (componentType:Component) (entity:Entity) =
    match Map.tryFind componentType entity.Components with
        | Some(someComponent) ->
            // Debug.WriteLine(sprintf "Component %A found in entity %A" componentType entity)
            Some someComponent
        | None ->
            None

let getEntityWithComponent (componentType:Component) (world:World) =
    let hasComponent (componentType:Component) (entity:Entity) =
        Map.containsKey componentType entity.Components

    let entityWithComponent =
        world.Entities
        |> List.tryFind (hasComponent componentType)
    entityWithComponent

// add a component to an entity
let addComponentToEntity (componentType:Component) (someComponent:obj) (entity:Entity) =
    let newComponents =
        entity.Components
        |> Map.add componentType someComponent
    { entity with Components = newComponents }

let GenerateId() =
    let mutable id = 0
    fun () ->
        id <- id + 1
        id

let generateId = GenerateId()

let createEntity components =
    let entityId = generateId()
    { Id = entityId; Components = components }