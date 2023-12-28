using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damage : MonoBehaviour
{
    float scale = 1f;

    Text damageTXT;


    // Start is called before the first frame update
    void Start()
    {
        damageTXT = GameObject.Find("damageTXT").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        damageTXT.transform.position += new Vector3(0f, 0.1f, 0f);
        StartCoroutine("fadeOut");
    }

    IEnumerator fadeOut(){
        scale -= Time.deltaTime;
        damageTXT.color = new Color(1, 0, 0, scale);
        yield return null;

        if(scale <= 0){
            Destroy(gameObject);
        }
    }
}
