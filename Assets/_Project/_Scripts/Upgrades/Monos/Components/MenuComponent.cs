using UnityEngine;

public class MenuComponent : MonoBehaviour
{
    [SerializeField]
    private float variety;

    public float Variety
    {
        get { return variety; }
        private set { variety = value; }
    }

    public void IncreaseMenuVariety(float amount)
    {
        Variety += amount;
        Debug.Log($"Menu variety increased by {amount}. New variety: {Variety}");
    }
}
