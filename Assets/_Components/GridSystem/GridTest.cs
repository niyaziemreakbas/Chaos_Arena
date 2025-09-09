using System.Collections;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    public UnitType[] unitTypes;      // Swordsman, Archer, Mage vs
    private SymmetricGridPositioner positioner;

    void Start()
    {
        positioner = new SymmetricGridPositioner(
            basePos: new Vector2(0, 0),
            xSpacing: 0.5f,
            ySpacing: 1f,
            maxPerRow: 6
        );

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            var type = unitTypes[Random.Range(0, unitTypes.Length)];
            int addCount = Random.Range(1, 4);

            var newPositions = positioner.AddUnits(type, addCount);

            foreach (var pos in newPositions)
            {
                var obj = Instantiate(type.prefab, pos, Quaternion.identity);
                obj.GetComponent<SpriteRenderer>().color = type.debugColor;
            }

            yield return new WaitForSeconds(2f);
        }
    }
}
