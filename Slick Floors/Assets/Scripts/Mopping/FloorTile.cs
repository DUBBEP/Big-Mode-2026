using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private FloorTypeDefinitionSO dirtyData;
    [SerializeField] private FloorTypeDefinitionSO cleanData;
    [SerializeField] private FloorTypeDefinitionSO neutralData;


    [SerializeField] private GroundType currentType;
    [HideInInspector] public MovementProfileSO currentMovementProfile;


    private void Start()
    {
        if (!TryGetComponent<SpriteRenderer>(out sr))
            sr = gameObject.AddComponent<SpriteRenderer>();

        ChangeTyle(currentType);
    }

    private void OnValidate()
    {
        ChangeTyle(currentType);
    }

    public void ChangeTyle(GroundType type)
    {
        currentType = type;
        switch (type)
        {
            case GroundType.Dirty:
                SetFloorType(dirtyData);
                break;
            case GroundType.Clean:
                SetFloorType(cleanData);
                break;
            case GroundType.Neutral:
                SetFloorType(neutralData);
                break;
            default:
                Debug.LogError($"Unsupported Ground Type Passed Into Change Tyle Method on {name}");
                break;
        }
    }

    private void SetFloorType(FloorTypeDefinitionSO data)
    {
        if (data == null)
        {
            Debug.LogError($"Floor Data is missing on floor tile {name}");
            return;
        }

        currentMovementProfile = data.playerMovementProfile;
        sr.sprite = data.floorSprite;
    }

    public GroundType GetCurrentType()
    {
        return currentType;
    }
}
