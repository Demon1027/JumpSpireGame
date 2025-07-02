using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Double_Jump_Coin : MonoBehaviour
{
    public Audio_Manager_Script AMS;
    public Boss_Stage_Player Player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            AMS.Coin_Sound_Effect();
            gameObject.SetActive(false);
            Invoke("Set_Coin", 5);
            Player.Set_Double_Jump_Count();
            Player.WaitTime();
        }
    }

    void Set_Coin()
    {
        gameObject.SetActive(true);
    }
}
