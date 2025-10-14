using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingsScript : MonoBehaviour
{
    public int numEndings = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i < 3; i++)
        {
            if (progressTrackerScript.Instance.endingsGotten[i])
            {
                numEndings++;
            }
        }
        GetComponent<TMP_Text>().text = ("Endings: "+numEndings.ToString()+"/3");
    }
}
