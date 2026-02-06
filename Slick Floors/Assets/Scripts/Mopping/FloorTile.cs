using UnityEngine;

public class FloorTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private FloorTypeDefinitionSO dirtyData;
    [SerializeField] private FloorTypeDefinitionSO cleanData;
    [SerializeField] private FloorTypeDefinitionSO neutralData;

    [SerializeField] private GroundType currentType;
    [SerializeField] private float dirtyTileDamageValue;
    [HideInInspector] public MovementProfileSO currentMovementProfile;

    private DirtyTileDamage damageComponent;
    private GroundType previousType;

    private void Start()
    {
        if (!TryGetComponent<SpriteRenderer>(out sr))
            sr = gameObject.AddComponent<SpriteRenderer>();

        TileHandler.AddTile(this);

        ChangeTyle(currentType);
    }

    private void OnValidate()
    {
        ChangeTyle(currentType);
    }

    public void ChangeTyle(GroundType type)
    {
        previousType = currentType;
        TileHandler.UpdateTileTypeCount(currentType, type);

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
        if (previousType != currentType && currentMovementProfile != null && currentMovementProfile.moppedSoundFXs != null)
        {
            SoundFXManager.Instance.PlayMopSounds(currentMovementProfile.moppedSoundFXs[Random.Range(0, currentMovementProfile.moppedSoundFXs.Count)], transform);
        }

        UpdateDamageComponent();
    }

    private void UpdateDamageComponent()
    {
        if (currentType == GroundType.Dirty && damageComponent == null)
        {
            damageComponent = gameObject.AddComponent<DirtyTileDamage>();
            damageComponent.damageValue = dirtyTileDamageValue;
        }
        else
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (damageComponent != null)
                    DestroyImmediate(damageComponent);
            };
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
}
