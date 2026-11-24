# Forest Builder

Forest Builder is a 3D mobile incremental game currently being developed with Unity and C#.

The player gathers resources, sells them, improves their abilities, and gradually progresses through the world. The goal is to build this core loop into a larger game with new regions, resources, automation, and production systems.

This is an active project that I am building both as a game I intend to release and as a way to improve my Unity, C#, and game development skills by working on a complete project rather than isolated prototypes.

## Gameplay

The current gameplay loop is:

**Harvest → Collect → Sell → Upgrade → Progress**

At the current stage, the player can:

- Harvest trees and rocks
- Collect Wood and Stone
- Carry multiple resource types with limited inventory capacity
- Sell collected resources for coins
- Unlock Stone as a new resource
- Upgrade Carry Capacity
- Upgrade Harvest Power
- Upgrade Movement Speed

Upgrades have their own costs, level limits, and progression indicators.

## World Generation

The starting area uses a procedural region system instead of relying entirely on manually placed resources.

A region defines the playable area and contains both reserved gameplay spaces and generated resource clusters. The generator attempts to find valid positions for each cluster while keeping them inside the region and preventing overlap with reserved areas or other clusters.

The current system supports:

- Hexagonal region boundaries
- Forest and rock clusters
- Configurable cluster counts
- Reserved gameplay areas
- Cluster overlap prevention
- Procedural resource placement

The same system is intended to support additional regions and resource types later in development.

## Architecture

As the project grows, I am trying to keep gameplay systems independent enough to be extended without rewriting existing features for every new resource or upgrade.

Some of the current systems include:

- `ResourceDefinition` — ScriptableObject containing resource configuration
- `ResourceNode` — shared behaviour for harvestable resource nodes
- `ResourceDropper` — handles resource drops when a node is depleted
- `PlayerInventory` — manages carried resources and inventory capacity
- `ResourceUnlockSystem` — controls progression-based resource access
- `ResourceCluster` — represents a resource area inside a region
- `RegionGenerator` — places resource clusters within a region
- `RepeatableUpgrade` — base implementation for multi-level player upgrades

The architecture is still evolving as new gameplay requirements are introduced. Part of the purpose of the project is learning where abstraction is useful in an actual game project and where keeping a system simple is the better choice.

## Tech Stack

- Unity 6
- C#
- Universal Render Pipeline (URP)
- TextMesh Pro
- Git
- GitHub
- Visual Studio

### Target Platforms

- Android
- iOS

Development and testing currently focus on Android. iOS support is planned using the same Unity project.

## Development Status

Forest Builder is currently under active development and is not yet a finished game.

The main systems implemented so far are:

- Player movement and mobile controls
- Resource harvesting and collection
- Inventory and carrying
- Resource selling and coin economy
- Resource unlocking
- Procedural resource regions
- Player upgrade progression

### Planned

The current development direction includes:

- Region expansion and progression
- Additional resources
- New regions and environments
- Workers and automation
- Production systems
- Save/load
- Visual changes for selected upgrade levels
- Improved UI and gameplay feedback
- Sound and visual effects
- Mobile performance profiling and optimization
- Android release preparation
- iOS support

These plans may change as the game develops and the core gameplay is tested.

## Screenshots

Gameplay screenshots and video will be added as the visual side of the project develops further.
