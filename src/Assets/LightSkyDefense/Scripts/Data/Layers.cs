using System;

[Flags]
public enum Layers
{
    None = 0,

    // Built-in layers
    Default = 1 << 0,
    TransparentFX = 1 << 1,
    IgnoreRaycast = 1 << 2,
    Water = 1 << 4,
    UI = 1 << 5,

    // Custom layers
    Towers = 1 << 8,
    Enemies = 1 << 9,
    Path = 1 << 10,
    Controls = 1 << 11,
    Projectiles = 1 << 12,
    Pickups = 1 << 13,
}
