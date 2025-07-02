using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public Boss BS;
    public Boss_Stage_Player Player;
    public Life_Ui LU;
    public int Player_Life = 3;
    public int Boss_Life = 3;

    public void Lose_Player_Life()
    {
        Debug.Log("플레이어 체력 1 감소");
        LU.Delete_Life_Image(Player_Life);
        Player_Life -= 1;
        if (Player_Life <= 0)
        {
            BS.Stop_Effect_Boss();
            Player.Player_Die_();
        }
    }

    public void Lose_Boss_Life()
    {
        Debug.Log("보스 체력 1 감소");
        Boss_Life -= 1;
        if (Boss_Life == 1)
        {
            BS.Hidden_Boss();
        }

        if(Boss_Life <= 0 )
        {
            BS.Boss_Die();
        }
    }
}
