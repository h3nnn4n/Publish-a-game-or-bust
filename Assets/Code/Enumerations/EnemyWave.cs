using System;

[Serializable]
public class EnemyWave
{
    int index;

    public float interval;
    public int size;
    public EnemyType enemyType;

    public bool Finished()
    {
        return index + 1 >= size;
    }

    public void BumpIndex()
    {
        index++;
    }
}
