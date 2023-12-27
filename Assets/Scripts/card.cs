using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class card : MonoBehaviour
{
    public Animator anim;

    public AudioClip flip;
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void openCard(){
        if(!gameManager.I.areOpen && gameManager.I.isstart){
            audioSource.PlayOneShot(flip);
            anim.SetBool("isOpen", true);

            transform.Find("front").gameObject.SetActive(true);
            transform.Find("back").gameObject.SetActive(false);

            if(gameManager.I.firstCard == null){
                gameManager.I.firstCard = gameObject;
            }
            else{
                gameManager.I.secondCard = gameObject;
                gameManager.I.isMatched();
                gameManager.I.areOpen = true;
            }
        }
    }

    public void destroyCard()
    {
        Invoke("destroyCardInvoke", 0.6f);
        anim.SetTrigger("isSuccess");
    }

    void destroyCardInvoke()
    {
        Destroy(gameObject);
        gameManager.I.areOpen = false;
        gameManager.I.state = 0;
    }

    public void closeCard()
    {
        Invoke("closeCardInvoke", 1.0f);
    }

    void closeCardInvoke()
    {   
        anim.SetBool("isOpen", false);
        anim.SetTrigger("toClose");
        audioSource.PlayOneShot(flip);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
        gameManager.I.areOpen = false;
        gameManager.I.state = 0;
    }
}
