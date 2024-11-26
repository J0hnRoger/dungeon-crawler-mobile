graph TD
    A[System] --> B[Model]
    A --> C[View]
    A --> D[Controller]
    E[Injector] -.-> A
    F[GameStore] -.-> A
    G[EventBus] <-.-> A

sequenceDiagram
    participant Bootstrap
    participant Core Services
    participant GameStore
    participant Systems
    
    Bootstrap->>Core Services: 1. Initialize
    Note over Core Services: - SceneManager<br>- EventBus<br>- Injector<br>- SaveLoadSystem
    
    Core Services->>Bootstrap: Ready
    
    Bootstrap->>SaveLoadSystem: 2. Load Game Data
    SaveLoadSystem->>GameStore: 3. Initialize Store
    
    GameStore->>Systems: 4. Inject Dependencies
    Note over Systems: Systems can be:<br>- Eager loaded<br>- Lazy loaded