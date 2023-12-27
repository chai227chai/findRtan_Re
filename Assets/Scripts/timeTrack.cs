using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeTrack : MonoBehaviour
{
    private float shakeTime;//흔들리는 시간
    private float shakeIntensity;//흔들리는 범위

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void onShake(float shakeTime = 1.0f, float shakeIntensity = 5f){
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        if(gameManager.I.isstart){
            StopCoroutine("shake");
            StartCoroutine("shake");
        }
        else{
            StopCoroutine("shake");
        }
    }

    IEnumerator shake(){
        //흔들리기 직전 위치
        Vector3 startPosition = transform.position;

        while(shakeTime > 0.0f){
            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;

            shakeTime -= Time.deltaTime;

            yield return null;
        }

        //원래 자리로 되돌아옴
        transform.position = startPosition;
    }
}
