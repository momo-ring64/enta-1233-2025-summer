using UnityEngine;

public class GameTester : MonoBehaviour
{
    [SerializeField] private CharacterManager Characters;

    private void Start()
    {
        Characters.SpawnCharacter();
    }
}
