using UnityEngine;

public class Enums : MonoBehaviour
{ }


/// <summary>
/// Upgrade rarities
/// </summary>
public enum UpgradeRarity
{
    Common,
    Rare,
    Legendary
}

/// <summary>
/// Upgrade types
/// </summary>
public enum UpgradeType
{
    Add,
    Amplify,
    Activate
}

/// <summary>
/// Upgrade slots in upgrade scene
/// </summary>
public enum UpgradeSlot
{
    Slot1,
    Slot2,
    Slot3,
    Slot4
};

/// <summary>
/// Damage types
/// </summary>
public enum DamageType
{
    Bullet,
    Explosion,
    Melee
}

/// <summary>
/// Player types
/// </summary>
public enum PlayerType
{
    Universal,
    Rocket,
    Disabled
}

public enum SceneType
{
    Menu,
    Tutorial,
    BasicLevel,
    BossLevel,
    UpgradeArea
}

public enum GameState
{
    Menu,
    Tutorial,
    Running,
    LevelFinished,
    Upgrading,
    BossFinished
}

public enum EnemyType
{
    Dummy,
    Stationary,
    Moving,
    Boss
}

public enum Direction
{
    left,
    right,
    forward,
    backward
}