using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win_Coin_Script : MonoBehaviour
{
    public Audio_Manager_Script AMS;
    public Boss_Stage_Player BSP;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 18)
        {
            AMS.Coin_Sound_Effect();
            BSP.Win_Player_Scene();
            gameObject.SetActive(false);
        }
    }
    public void Win_Coin_Set_On()
    {
        gameObject.SetActive(true);
    }

}
