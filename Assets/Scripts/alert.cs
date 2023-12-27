using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alert : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.I.isEmer){
            anim.SetBool("isEmer", true);
        }
        else{
            anim.SetBool("isEmer", false);
        }
    }

    void emerAlert(){
        
    }
}
