module ECSTypes

open Components

type Entity = {
    Id: int
    Components: Map<Component, obj>
}

type World = {
    Entities: Entity list
}