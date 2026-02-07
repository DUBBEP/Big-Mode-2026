using UnityEngine;

public class FloorTile : MonoBehaviour
{
    [SerializeField] private GenericEventSO tileUpdatedEvent;
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private FloorTypeDefinitionSO dirtyData;
    [SerializeField] private FloorTypeDefinitionSO cleanData;
    [SerializeField] private FloorTypeDefinitionSO neutralData;

    [SerializeField] private GroundType currentType;
    [SerializeField] private float dirtyTileDamageValue;
    [HideInInspector] public MovementProfileSO currentMovementProfile;

    private DirtyTileDamage damageComponent;
    private GroundType previousType;
    [HideInInspector] public GroundType CurrentType { get { return currentType; } private set { } }

    private void Start()
    {
        if (!TryGetComponent<SpriteRenderer>(out sr))
            sr = gameObject.AddComponent<SpriteRenderer>();

        TileHandler.AddTile(this);

        ChangeTile(currentType);
    }

    private void OnValidate()
    {
        UpdateTileData(currentType);

    }

    public void ChangeTile(GroundType type)
    {
        previousType = currentType;
        TileHandler.UpdateTileTypeCount(currentType, type);
        UpdateTileData(type);
        UpdateDamageComponent();
        tileUpdatedEvent.Raise(new GameEventPayload());

        if (previousType != currentType && currentMovementProfile != null && currentMovementProfile.moppedSoundFXs != null)
        {
            SoundFXManager.Instance.PlayMopSounds(currentMovementProfile.moppedSoundFXs[Random.Range(0, currentMovementProfile.moppedSoundFXs.Count)], transform);
        }
    }

    private void UpdateTileData(GroundType type)
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
