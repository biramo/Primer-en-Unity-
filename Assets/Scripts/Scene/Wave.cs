[System.Serializable]
public class Wave
{
    public int meleeCount;

    public int distanceCount;

    public float spawnInterval=0.5f;

    public int totalEnemies()
    {
        return meleeCount+distanceCount;
    }
}