using System.Collections.Generic;

public class NextEnemySpawn
{
    public float countdown;
    public EnemyType enemyType;
    public List<EnemyModifier> enemyModifiers;

    public NextEnemySpawn(float countdown, EnemyType enemyType, List<EnemyModifier> enemyModifiers)
    {
        this.countdown = countdown;
        this.enemyType = enemyType;
        this.enemyModifiers = new List<EnemyModifier>(enemyModifiers);
    }
}
