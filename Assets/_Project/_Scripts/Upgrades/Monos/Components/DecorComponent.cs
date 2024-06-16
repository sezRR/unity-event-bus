using UnityEngine;

public class DecorComponent : MonoBehaviour
{
    [SerializeField]
    private float attractiveness;

    public float Attractiveness
    {
        get { return attractiveness; }
        private set { attractiveness = value; }
    }

    public void IncreaseAttractiveness(float amount)
    {
        Attractiveness += amount;
        Debug.Log($"Decor attractiveness increased by {amount}. New attractiveness: {Attractiveness}");
    }
}
