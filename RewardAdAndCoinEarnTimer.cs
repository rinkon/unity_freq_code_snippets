// 1. ad AdsManager Gameobject and add a script to it. Only one line in the Start Method : MobileAds.Initialize(initStatus => {});
// 2. add Reward add from google mobile ads menu.
// 3. add game serialized gameobjects timeRemainingPanel and adLoadingView
// 4. requestRewardEvent = RewardAd.LoadAd
// 5. showRewardEvent = RewardAd.ShowIfLoaded
// 6. setupcallbacks

#region Ad related
[SerializeField]
private UnityEvent requestRewardEvent, showRewardEvent;
[SerializeField]
private GameObject timeRemainingPanel, adLoadingView; //get the prefabs
#endregion

#region Ad Related

public void ShowAdGetCoin(){
    string dateString = PlayerPrefs.GetString("lastAdShowTime", "noTimeAssigned");

    if(!dateString.Equals("noTimeAssigned")){
        DateTime convertedDate = Convert.ToDateTime(dateString);
        double minutes = (DateTime.Now - convertedDate).TotalMinutes;
        double seconds = (DateTime.Now - convertedDate).TotalSeconds % 60;

        if(minutes > 3){
            if(Application.internetReachability == NetworkReachability.NotReachable){
                genericAlert.GetComponent<GenericAlertScript>().primaryText = "Internet Error!";
                genericAlert.GetComponent<GenericAlertScript>().secondaryText = "Please check internet connection.";
                genericAlert.SetActive(true);
            }
            else{
                // show ad loading screen
                adLoadingView.SetActive(true);
                TMP_Text message =(TMP_Text) adLoadingView.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                message.text = "Ad is loading, Please wait...";
                requestRewardEvent.Invoke();
            }
        }
        else{
            // show timer pop up
            timeRemainingPanel.SetActive(true);
            timeRemainingPanel.GetComponent<AdTimerScript>().minutes = (3 - minutes);
            timeRemainingPanel.GetComponent<AdTimerScript>().seconds = (60-seconds);

            print(minutes);
            print(seconds);
        }
    }
    else{
        adLoadingView.SetActive(true);
        requestRewardEvent.Invoke();
    }
}

public void AdLoadedCallBack(){
    adLoadingView.SetActive(false);   
    showRewardEvent.Invoke();
}

public void AdFailedToLoadCallback(){
    TMP_Text message =(TMP_Text) adLoadingView.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    message.text = "Sorry, failed to load ad.";
    StartCoroutine(HideGameObject(adLoadingView));
}

public void RewardAdSuccessful(){
    adLoadingView.SetActive(false);
    PlayerPrefs.SetString("lastAdShowTime", DateTime.Now.ToString());
    int coinCount = PlayerPrefs.GetInt("coins", 0);
    coinCount += 1500;
    PlayerPrefs.SetInt("coins", coinCount);
    PlayerPrefs.Save();
    mansionCanvasController.coinText.text = coinCount.ToString();
    
    AudioClip coinRattle = (AudioClip)Resources.Load("positiveSound");
    GenericAlertScript genericAlertScript = genericAlert.GetComponent<GenericAlertScript>();
    genericAlertScript.primaryText = "Congratulations!";
    genericAlertScript.secondaryText = "You've earned 1500 coins!!";
    genericAlertScript.audioClip = coinRattle;
    genericAlertScript.shouldPlay = true;
    genericAlert.SetActive(true);
}

public void RewardAdNOTSuccessful(){
    Debug.Log("Failed Failed failed failed");
}

IEnumerator HideGameObject(GameObject go){
    yield return new WaitForSeconds(3f);
    go.SetActive(false);
}

#endregion



// here goes the adtimerscript

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdTimerScript : MonoBehaviour
{
    [System.NonSerialized]
    public double minutes = 0, seconds = 100;
    [SerializeField]
    public TMP_Text timer_text;

    [SerializeField]
    private GameObject parentOnAction;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        seconds -= Time.deltaTime;
        if(seconds < 0){
            
            seconds = 60;
            minutes -= 1;

            if(minutes < 0){
                seconds = 0;
                minutes = 0;
            }
        }
        
        timer_text.text = string.Format("{0}:{1}", ((int)minutes).ToString("D2"), ((int)seconds).ToString("D2"));
        
    }

    private void OnEnable() {
        Tweener.Tween(parentOnAction, 1.5f, 1.5f);
    }

    public void HideAlertPanel(){
        this.gameObject.SetActive(false);
    }
}
