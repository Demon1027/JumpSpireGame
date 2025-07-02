using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life_Ui : MonoBehaviour
{
    public Image[] Life_Images;

    public void Delete_Life_Image(int Life_Count)
    {
        if (Life_Count <= 3 && Life_Count > 0)
        {
            Life_Images[Life_Count - 1].gameObject.SetActive(false);
        }
        else
            return;
    }
}
