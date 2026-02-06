using System.Collections.Generic;

public static class TileHandler
{
    public static List<FloorTile> TilesInScene = new List<FloorTile>();

    public static int TotalTileCount;

    public static int DirtyCount;
    public static int CleanCount;
    public static int NeutralCount;

    public static void UpdateTileTypeCount(GroundType oldType, GroundType newType)
    {
        ChangeTileTypeCount(oldType, IncrementType.subtract);
        ChangeTileTypeCount(newType, IncrementType.add);

        if ((DirtyCount + CleanCount + NeutralCount) != TotalTileCount)
            RecalculateTiles();
    }

    public static void RecalculateTiles()
    {
        DirtyCount = 0;
        CleanCount = 0;
        NeutralCount = 0;

        foreach (FloorTile tile in TilesInScene)
            ChangeTileTypeCount(tile.CurrentType, IncrementType.add);
    }


    public static void AddTile(FloorTile tile)
    {
        TilesInScene.Add(tile);
        TotalTileCount++;
        ChangeTileTypeCount(tile.CurrentType, IncrementType.add);
    }

    public static int GetTileTypeCount(GroundType type)
    {
        switch (type)
        {
            case (GroundType.Dirty):
                return DirtyCount;
            case (GroundType.Clean):
                return CleanCount;
            case (GroundType.Neutral):
                return NeutralCount;
        }
        
        return -1;
    }

    private static void ChangeTileTypeCount(GroundType type, IncrementType incrementType)
    {
        switch (type)
        {
            case (GroundType.Dirty):
                DirtyCount += incrementType == IncrementType.add ? 1 : -1;
                break;
            case (GroundType.Clean):
                CleanCount += incrementType == IncrementType.add ? 1 : -1;
                break;
            case (GroundType.Neutral):
                NeutralCount += incrementType == IncrementType.add ? 1 : -1;
                break;
        }
    }

    enum IncrementType
    {
        add,
        subtract,
    }
}
