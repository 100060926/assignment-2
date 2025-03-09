using UnityEngine;
using UnityEngine.UI;

public class KittyCounterUI : MonoBehaviour
{
    public Text kittyCountText; // Reference to the UI text element

    void Start()
    {
        EnemyAI.AssignKittyCounterUI(kittyCountText); // 🛠 Link UI with EnemyAI
    }
}
