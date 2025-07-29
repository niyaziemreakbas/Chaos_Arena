using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] private TargetMover mover;

    [SerializeField] private Transform testTarget;

    void Start()
    {
        mover.SetTarget(testTarget); // Hedefe doðru yürümeye baþlar
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            mover.ClearTarget(); // Hedefi siler, hareket durur
        }
    }
}
