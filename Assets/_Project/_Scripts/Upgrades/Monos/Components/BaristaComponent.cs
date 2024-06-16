using UnityEngine;

public class BaristaComponent : MonoBehaviour
{
    [SerializeField]
    private float efficiency;

    public float Efficiency
    {
        get { return efficiency; }
        private set { efficiency = value; }
    }

    public void IncreaseEfficiency(float amount)
    {
        Efficiency += amount;
        Debug.Log($"Barista efficiency increased by {amount}. New efficiency: {Efficiency}");
    }
}
