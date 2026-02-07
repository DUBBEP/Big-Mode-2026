using UnityEngine;

public class FloorTile : MonoBehaviour
{
    [SerializeField] private FloorRenderer floorRenderer;
    [SerializeField] private SpriteRenderer floorSprite;
    [SerializeField] private SpriteRenderer foregroundSprite;
    [SerializeField] private GenericEventSO tileUpdatedEvent;
    [SerializeField] private SpriteRenderer backgroundSprite;

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
        TileHandler.AddTile(this);

        ChangeTile(currentType);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
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
        floorRenderer = data.floorRenderer;
        if (floorRenderer != null)
        {
            if (floorRenderer.Floor != null && floorSprite != null)
            {
                floorSprite.sprite = floorRenderer.Floor.sprite;
                floorSprite.color = floorRenderer.Floor.color;
                floorSprite.flipX = floorRenderer.Floor.flipX;
                floorSprite.flipY = floorRenderer.Floor.flipY;
                floorSprite.sortingLayerID = floorRenderer.Floor.sortingLayerID;
                floorSprite.sortingOrder = floorRenderer.Floor.sortingOrder;
                floorSprite.sharedMaterial = floorRenderer.Floor.sharedMaterial;
            }

            if (floorRenderer.Foreground != null && foregroundSprite != null)
            {
                foregroundSprite.sprite = floorRenderer.Foreground.sprite;
                foregroundSprite.color = floorRenderer.Foreground.color;
                foregroundSprite.flipX = floorRenderer.Foreground.flipX;
                foregroundSprite.flipY = floorRenderer.Foreground.flipY;
                foregroundSprite.sortingLayerID = floorRenderer.Foreground.sortingLayerID;
                foregroundSprite.sortingOrder = floorRenderer.Foreground.sortingOrder;
                foregroundSprite.sharedMaterial = floorRenderer.Foreground.sharedMaterial;
            }

            if (floorRenderer.Background != null && backgroundSprite != null)
            {
                backgroundSprite.sprite = floorRenderer.Background.sprite;
                backgroundSprite.color = floorRenderer.Background.color;
                backgroundSprite.flipX = floorRenderer.Background.flipX;
                backgroundSprite.flipY = floorRenderer.Background.flipY;
                backgroundSprite.sortingLayerID = floorRenderer.Background.sortingLayerID;
                backgroundSprite.sortingOrder = floorRenderer.Background.sortingOrder;
                backgroundSprite.sharedMaterial = floorRenderer.Background.sharedMaterial;
            }
        }
    }
}
