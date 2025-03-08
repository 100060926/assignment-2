using UnityEngine;
using UnityEngine.UI;

public class KittyCounterUI : MonoBehaviour
{
    public Text kittyCountText; 

    void Update()
    {
        kittyCountText.text = "Kitties: " + EnemyAI.GetKittyCount(); 
    }
}
