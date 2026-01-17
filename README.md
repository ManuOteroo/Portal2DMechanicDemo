# Portal2DMechanicDemo
C# implementation of momentum-preserving portals in Unity 2D. Focused on momentum conservation and physics-based teleportation.

## Key Features

### 1. Dynamic Portal Generation (`PortalProjectile.cs`)
* **Surface Normal Alignment:** Projectiles calculate the impact surface's normal to instantiate portals with the correct rotation using `Mathf.Atan2`.
* **Placement Validation:** Includes a safety check to prevent portal overlapping or "blocked impacts" based on distance.
* **Dynamic Parent-Child Relationship:** Portals automatically become children of "Platforms," allowing for moving portal mechanics.

### 2. Physics-Based Teleportation (`PortalBehaviour.cs`)
* **Velocity Preservation:** The system captures the player's entry speed and redirects it according to the output portal's orientation.
* **Dynamic Exit Velocity:** Includes a 'Minimum Exit Velocity' setting to ensure the player never gets stuck when entering a portal at low speeds.
* **Safety Margin:** Implements a calculated offset in the exit direction to prevent the player from getting stuck inside geometry.

### 3. Weapon System (`PortalGun.cs`)
* **Mouse-Look Integration:** The weapon rotates dynamically toward the cursor with sprite-flipping logic to ensure visual consistency.
* **State-Based Input:** Uses the **Unity Input System** to handle equipping and firing different portal types (A and B).
* **Animation & Audio Sync:** Integrated triggers for shooting animations and sound effects upon firing.

```csharp
private void Teleport(GameObject obj, Transform exitTransform)
{
    Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

    if (rb != null)
    {
        float magnitude = rb.velocity.magnitude;
        if (magnitude < minExitVelocity)
        {
            magnitude = minExitVelocity;
        }
        obj.transform.position = exitTransform.position + exitTransform.right * 0.7f; 
        rb.velocity = exitTransform.right * magnitude;
    }
}

