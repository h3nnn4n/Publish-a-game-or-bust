using UnityEngine;

public class Node : MonoBehaviour
{
    public bool canBuild;
    public string type = "UNKNOW";
    public Vector2Int position;
    public bool hasTower;
    public bool enemyCanWalk;
    float towerBuiltOn = -1;

    public void SetEmpty()
    {
        canBuild = false;
        enemyCanWalk = false;
    }

    public void SetBuildable()
    {
        canBuild = true;
        enemyCanWalk = true;
        hasTower = false;
        towerBuiltOn = -1;
    }

    public void SetHasTower()
    {
        hasTower = true;
        enemyCanWalk = false;
        canBuild = false;
        towerBuiltOn = Time.time;
    }

    public bool HasTower()
    {
        return hasTower;
    }

    public bool IsTowerNew()
    {
        Debug.LogFormat("{0} {1} {2}", Time.time, towerBuiltOn, Time.time - towerBuiltOn);
        return Time.time - towerBuiltOn <= 0.01 || towerBuiltOn <= 0;
    }
}
