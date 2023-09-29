module ECSTypes

open Components

type Entity = {
    Id: int
    Type: EntityNames
    Components: Map<Component, obj>
}

type World = {
    Entities: Entity list
}