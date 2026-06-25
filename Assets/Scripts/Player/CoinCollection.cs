using TMPro;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    private int Coins;
    public int MaxCoinsToWin;
    public TextMeshProUGUI coinText;


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Coin")
        {
            Coins++;
            coinText.text = "Coins: " + Coins.ToString();
            Destroy(other.gameObject);

            if(Coins == MaxCoinsToWin) //Win condition
            {
                onPlayerWin();
            }
        }
    }

    public void ResetCoins() //Reset coins if level restarts
    {
        Coins = 0;
        coinText.text = "Coins: " + Coins.ToString();
    }

    private void onPlayerWin()
    {
        //menu win title etc...
    }
}
