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
    public GroundType CurrentType { get { return currentType; } }

    private void Start()
    {
        TileHandler.AddTile(this);
        UpdateTileData(currentType);
        UpdateDamageComponent();
    }

    public void ChangeTile(GroundType type)
    {
        previousType = currentType;
        
        if (previousType != type)
        {
            TileHandler.UpdateTileTypeCount(currentType, type);
        }
        
        UpdateTileData(type);
        UpdateDamageComponent();
        
        tileUpdatedEvent.Raise(new GameEventPayload());

        if (currentMovementProfile != null && currentMovementProfile.moppedSoundFXs != null && currentMovementProfile.moppedSoundFXs.Count > 0)
        {
            SoundFXManager.Instance.PlayMopSounds(currentMovementProfile.moppedSoundFXs[Random.Range(0, currentMovementProfile.moppedSoundFXs.Count)], transform);
        }

        if (ParticlePooler.Instance != null)
        {
            ParticlePooler.Instance.SpawnParticle(transform.position, Quaternion.identity);
        }
    }

    private void UpdateTileData(GroundType type)
    {
        currentType = type;
        switch (type)
        {
            case GroundType.Dirty:   SetFloorType(dirtyData); break;
            case GroundType.Clean:   SetFloorType(cleanData); break;
            case GroundType.Neutral: SetFloorType(neutralData); break;
        }
    }

    private void UpdateDamageComponent()
    {
        if (currentType == GroundType.Dirty)
        {
            if (damageComponent == null)
            {
                damageComponent = gameObject.AddComponent<DirtyTileDamage>();
                damageComponent.damageValue = dirtyTileDamageValue;
            }
        }
        else
        {
            if (damageComponent != null)
            {
                Destroy(damageComponent);
                damageComponent = null; 
            }
        }
    }

    private void SetFloorType(FloorTypeDefinitionSO data)
    {
        if (data == null) return;

        currentMovementProfile = data.playerMovementProfile;
        floorRenderer = data.GetRandomRenderer();

        if (floorRenderer != null)
        {
            ApplySpriteData(floorSprite, floorRenderer.Floor);
            ApplySpriteData(foregroundSprite, floorRenderer.Foreground);
            ApplySpriteData(backgroundSprite, floorRenderer.Background);
        }
    }

    private void ApplySpriteData(SpriteRenderer target, SpriteRenderer source)
    {
        if (target == null || source == null) return;
        target.sprite = source.sprite;
        target.transform.localPosition = source.transform.localPosition;
        target.transform.localRotation = source.transform.localRotation;
        target.transform.localScale = source.transform.localScale;
        target.color = source.color;
        target.flipX = source.flipX;
        target.flipY = source.flipY;
        target.sortingLayerID = source.sortingLayerID;
        target.sortingOrder = source.sortingOrder;
        target.sharedMaterial = source.sharedMaterial;
    }
}