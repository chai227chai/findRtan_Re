using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public GameObject card;
    public GameObject firstCard;
    public GameObject secondCard;
    public GameObject endText;
    public GameObject music;
    public GameObject cntDown;
    public GameObject startCount;
    public GameObject alert;

    public AudioClip match;
    public AudioClip drum;
    public AudioSource audioSource;

    public Text timeText;
    public Text updateText;
    public Text countText;
    public Text countDown;
    public Text startTXT;

    public bool areOpen;
    public bool isstart;
    public bool isEmer;

    float time = 60.0f;
    
    float soundTime = 0f;

    public int count = 0;
    public int state = 0;

    int countdown = 0;
    int timer = 3;
    int startTime = 0;

    public static gameManager I;

    void Awake()
    {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        areOpen = false;
        isstart = false;
        isEmer = false;

        int[] rtans = {0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7};
        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for(int i = 0; i < 16; i++){

            GameObject newCard = Instantiate(card);
            //newCards를 cards로 옮겨라
            newCard.transform.parent = GameObject.Find("Cards").transform;

            float x = (i / 4) * 1.4f - 2.1f;
            float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(x, y, 0);

            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isstart == false){
            soundTime -= Time.deltaTime;
            if(soundTime <= 0){
                startTime++;
                soundTime = 1f;
                if(timer > 0){
                    audioSource.PlayOneShot(drum);
                }
            }

            timer = 4 - startTime;
            startTXT.text = timer.ToString("N0");
            startCount.SetActive(true);

            if(timer == 0){
                startTXT.text = "시작!";
            }
            if(timer < 0){
                isstart = true;
            }
        }

        if(isstart == true){
            startCount.SetActive(false);
            time -= Time.deltaTime;
            timeText.text = time.ToString("N2");
            if(time <= 15f){
                isEmer = true;
                timeText.color = new Color32(255, 50, 0, 255);
            }
            int cardsLeft = GameObject.Find("Cards").transform.childCount;

            if(firstCard != null){
                soundTime -= Time.deltaTime;
                if(soundTime <= 0){
                    countdown++;
                    soundTime = 1f; 
                }
                timer = 5 - countdown;
                cntDown.SetActive(true);
                countDown.text = timer.ToString("N0");
                if(timer <= 0){
                    // count += 1;
                    // state = 2;
                    // areOpen = true;

                    // firstCard.GetComponent<card>().closeCard();

                    // firstCard = null;
                    // cntDown.SetActive(false);
                    // countdown = 0;
                    isMatched();
                }
            }

            if(time <= 0.0f || cardsLeft == 0){
                time = 0f;
                areOpen = true;
                GameEnd();
            }

            countText.text = count.ToString();

            if(state == 1){
                updateText.text = "성공!";
            }
            else if(state == 2){
                updateText.text = "실패!";
            }
            else{
                updateText.text = "카드를 뒤집으세요!";
            }
        }
    }

    public void isMatched(){
        count += 1;
        if(firstCard && secondCard){
            string firstCardImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
            string secondCardImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        

            if(firstCardImage == secondCardImage){
                audioSource.PlayOneShot(match);
                state = 1;

                firstCard.GetComponent<card>().destroyCard();
                secondCard.GetComponent<card>().destroyCard();
            }
            else{
                state = 2;
                time -= 3f;

                firstCard.GetComponent<card>().closeCard();
                secondCard.GetComponent<card>().closeCard();
            }
        }
        else{
            areOpen = true;
            state = 2;
            time -= 3f;

            firstCard.GetComponent<card>().closeCard();
        }

        firstCard = null;
        secondCard = null;
        cntDown.SetActive(false);
        countdown = 0;
    }

    void GameEnd(){
        isstart = false;
        isEmer = false;
        Time.timeScale = 0.0f;
        endText.SetActive(true);
        music.GetComponent<audioManager>().stopMusic();
    }

    public void retryGame()
    {
        SceneManager.LoadScene("MainScene");
    }

}
