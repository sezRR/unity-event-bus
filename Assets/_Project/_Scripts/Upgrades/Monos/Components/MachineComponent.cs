using UnityEngine;

public class MachineComponent : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public float Speed
    {
        get { return speed; }
        private set { speed = value; }
    }

    public void IncreaseSpeed(float amount)
    {
        Speed += amount;
        Debug.Log($"Machine speed increased by {amount}. New speed: {Speed}");
    }
}
