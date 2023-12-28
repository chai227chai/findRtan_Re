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
    public GameObject music;
    public GameObject cntDown;
    public GameObject startCount;
    public GameObject alert;
    public GameObject endPanel;
    public GameObject timeTrack;
    public GameObject bar;
    public GameObject damage;

    public AudioClip match;
    public AudioClip drum;
    public AudioSource audioSource;

    public Text timeText;
    public Text updateText;
    public Text countText;
    public Text countDown;
    public Text startTXT;
    public Text result;
    public Text endTime;
    public Text matchCount;

    public bool areOpen;
    public bool isstart;
    public bool isEmer;
    public bool nocards;

    float time = 60.0f;
    float soundTime = 0f;
    float scale = 1f;

    public int count = 0;
    public int state = 0;

    int countdown = 0;
    int timer = 3;
    int startTime = 0;

    Dictionary<GameObject, int> deck = new Dictionary<GameObject, int>();

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
        nocards = false;

        endPanel.SetActive(false);

        newCards();
    }

    // Update is called once per frame
    void Update()
    {
        if(isstart == false){
            if(!nocards){
                StartCoroutine("dealCards");
            }
            
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
                StartCoroutine("barCount");
                soundTime -= Time.deltaTime;
                if(soundTime <= 0){
                    countdown++;
                    soundTime = 1f; 
                }
                timer = 5 - countdown;
                cntDown.SetActive(true);
                countDown.text = timer.ToString("N0");
                if(timer <= 0){
                    isMatched();
                }
            }

            if(cardsLeft == 0){
                result.text = "성공!";
                result.color = new Color32(50, 255, 255, 255);
                areOpen = true;
                GameEnd();
            }
            else if(time <= 0.0f){
                result.text = "실패!";
                result.color = new Color32(255, 50, 0, 255);
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
                Instantiate(damage);
                timeTrack.GetComponent<timeTrack>().onShake();

                firstCard.GetComponent<card>().closeCard();
                secondCard.GetComponent<card>().closeCard();
            }
        }
        else{
            areOpen = true;
            state = 2;
            time -= 3f;
            Instantiate(damage);
            timeTrack.GetComponent<timeTrack>().onShake();

            firstCard.GetComponent<card>().closeCard();
        }

        soundTime = 1f;
        firstCard = null;
        secondCard = null;
        cntDown.SetActive(false);
        countdown = 0;
        barReset();
    }

    void GameEnd(){
        nocards = true;
        isstart = false;
        isEmer = false;
        Time.timeScale = 0.0f;

        endPanel.SetActive(true);
        endTime.text = time.ToString("N2");
        matchCount.text = count.ToString();

        music.GetComponent<audioManager>().stopMusic();

        timeTrack.GetComponent<timeTrack>().onShake();
    }

    public void retryGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    void newCards(){
        int[] rtans = {0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7};
        rtans = rtans.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for(int i = 0; i < 16; i++){

            GameObject newCard = Instantiate(card);
            //newCards를 cards로 옮겨라
            newCard.transform.parent = GameObject.Find("Cards").transform;

            // float x = (i / 4) * 1.4f - 2.1f;
            // float y = (i % 4) * 1.4f - 3.0f;
            newCard.transform.position = new Vector3(0, 4f, 0);

            string rtanName = "rtan" + rtans[i].ToString();
            newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(rtanName);
            deck.Add(newCard, i);
        }
    }

    IEnumerator dealCards(){
        foreach(KeyValuePair<GameObject, int> item in deck){
            float x = (item.Value / 4) * 1.4f - 2.1f;
            float y = (item.Value % 4) * 1.4f - 3.0f;
            Vector3 target = new Vector3(x, y, 0);
            item.Key.transform.position = Vector3.Lerp(item.Key.transform.position, target, 0.03f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator barCount(){
        scale -= Time.deltaTime / 5 ;
        bar.transform.localScale = new Vector3(scale, 1f, 1f);
        bar.transform.GetComponent<Image>().color = Color.Lerp(Color.red, Color.white, scale);
        yield return null;
    }

    void barReset(){
        bar.transform.localScale = new Vector3(1f, 1f, 1f);
        scale = 1f;
    }
}
