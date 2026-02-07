using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewFloorType", menuName = "FloorType")]
public class FloorTypeDefinitionSO : ScriptableObject
{
    public string floorTypeName;
    public List<FloorRenderer> floorRenderers;
    public PhysicsMaterial2D material;
    public MovementProfileSO playerMovementProfile;

    public FloorRenderer GetRandomRenderer()
    {
        if (floorRenderers == null || floorRenderers.Count == 0)
        {
            Debug.LogError($"FloorTypeDefinitionSO '{name}': floorRenderers list is null or empty!");
            return null;
        }

        // Filter out null entries to prevent random null returns
        var validRenderers = floorRenderers.FindAll(r => r != null);

        if (validRenderers.Count == 0)
        {
            Debug.LogError($"FloorTypeDefinitionSO '{name}': All renderers in the list are null! Check inspector.");
            return null;
        }

        return validRenderers[Random.Range(0, validRenderers.Count)];
    }
}