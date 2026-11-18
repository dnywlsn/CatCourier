using UnityEngine;

// builds the level from a LevelData asset
public class LevelBuilder : MonoBehaviour
{
    public LevelData[] levels;
    public static LevelData current;

    public GameObject roofPrefab;
    public GameObject acPrefab;
    public GameObject chimneyPrefab;
    public GameObject fishPrefab;
    public GameObject doorPrefab;
    public GameObject pigeonPrefab;
    public GameObject dogPrefab;
    public GameObject crowPrefab;

    public Transform player;
    public GameUI ui;

    void Awake()
    {
        SaveSystem.Get(); // load the save if not loaded yet
        int index = SaveSystem.selectedLevel;
        if (index < 0 || index >= levels.Length) index = 0;
        LevelData level = levels[index];
        current = level;

        // roofs
        Transform roofParent = new GameObject("Roofs").transform;
        foreach (RoofInfo roof in level.roofs)
        {
            float width = roof.sprite.bounds.size.x;
            GameObject r = Instantiate(roofPrefab, new Vector3(roof.leftX + width / 2f, roof.topY, 0), Quaternion.identity, roofParent);
            SpriteRenderer sr = r.GetComponent<SpriteRenderer>();
            sr.sprite = roof.sprite;
            sr.color = level.roofColor;
            BoxCollider2D box = r.GetComponent<BoxCollider2D>();
            box.size = roof.sprite.bounds.size;
            box.offset = roof.sprite.bounds.center;
        }

        // AC units and chimneys
        foreach (PropInfo prop in level.props)
        {
            GameObject prefab = prop.type == PropType.AC ? acPrefab : chimneyPrefab;
            Instantiate(prefab, prop.position, Quaternion.identity, roofParent);
        }

        // fish
        Transform fishParent = new GameObject("Fish").transform;
        foreach (Vector2 pos in level.fish)
        {
            Instantiate(fishPrefab, pos, Quaternion.identity, fishParent);
        }

        // enemies
        Transform enemyParent = new GameObject("Enemies").transform;
        foreach (EnemySpawn e in level.enemies)
        {
            if (e.type == EnemyType.Pigeon)
            {
                GameObject p = Instantiate(pigeonPrefab, e.position, Quaternion.identity, enemyParent);
                p.GetComponent<Pigeon>().leftX = e.leftX;
                p.GetComponent<Pigeon>().rightX = e.rightX;
            }
            else if (e.type == EnemyType.Dog)
            {
                GameObject d = Instantiate(dogPrefab, e.position, Quaternion.identity, enemyParent);
                d.GetComponent<Dog>().leftX = e.leftX;
                d.GetComponent<Dog>().rightX = e.rightX;
            }
            else
            {
                Instantiate(crowPrefab, e.position, Quaternion.identity, enemyParent);
            }
        }

        Instantiate(doorPrefab, level.doorPosition, Quaternion.identity);
        player.position = level.startPosition;
        ui.ShowLevel(index + 1, level.levelName);
    }
}
