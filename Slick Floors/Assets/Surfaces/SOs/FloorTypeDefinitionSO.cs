using UnityEngine;

[CreateAssetMenu(fileName = "NewFloorType", menuName = "FloorType")]
public class FloorTypeDefinitionSO : ScriptableObject
{
    public string floorTypeName;
    public Sprite floorSprite;
    public PhysicsMaterial2D material;
    public MovementProfileSO playerMovementProfile;
}
