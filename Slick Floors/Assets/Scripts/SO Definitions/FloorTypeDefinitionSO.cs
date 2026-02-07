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
        if (floorRenderers == null || floorRenderers.Count == 0) return null;
        return floorRenderers[Random.Range(0, floorRenderers.Count)];
    }
}