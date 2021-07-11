using UnityEngine;

public class TestPlayerSpawner : MonoBehaviour
{
    public static PlayerController player;

    public GameObject bodyPrefab;
    public GameObject uiPrefab;

    public Canvas canvas;

    public EnemyController enemy;
    public int enemyLevel;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();

        InitEnemy();
    }

    private void InitEnemy()
    {
        if (!enemy) return;

        enemy.Set(enemyLevel);

        enemy.SetColor(0, Color.red);
    }

    private void SpawnPlayer()
    {
        player = PlayerController.Instantiate(bodyPrefab, uiPrefab, Camera.main, canvas.transform);
        player.SetColor(0, Color.green);
        player.SetColor(1, Color.green);

        player.transform.Translate(transform.position);
    }
}
