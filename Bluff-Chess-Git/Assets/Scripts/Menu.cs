using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Bolt;
using Photon.Bolt.Matchmaking;
using UdpKit;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : GlobalEventListener
{
    [SerializeField] TextMeshProUGUI ruleNumberTMP;
    [SerializeField] Button ruleBookButton;
    [SerializeField] Button cancelButton;
    [SerializeField] Button findGameButton;
    [SerializeField] Button createPrivateGameButton;
    [SerializeField] Button joinPrivateGameButton;
    [SerializeField] Button joinButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Image ruleBookBackground;
    [SerializeField] Image[] ruleBookImages;
    int currentRuleBookIndex = 0;
    [SerializeField] TextMeshProUGUI menuInfoText;
    [SerializeField] Image inputFieldBackground;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI nickNameTMP;
    [SerializeField] Image avatarImage;
    [SerializeField] Image profileScreen;
    [SerializeField] TMP_InputField nickNameInputField;

    [SerializeField] Image creditsScreen;


    bool privateGame = false;


    Settings settings;


    //for avatars
    [SerializeField] Sprite[] avatarSprites;
    int selectedAvatarIndex;

    [SerializeField] Button[] avatarButtons;
    [SerializeField] Sprite selectedAvatarSprite;
    [SerializeField] Sprite nonselectedAvatarSprite;
    [SerializeField] Image referenceAvatarImage;

    //------



    //tips
    [SerializeField] Image[] allTips;
    [SerializeField] float changeTipCD;
    float changeTipCDCounter = -1;
    int currentTipIndex = -1;

    IEnumerator closeTipCoroutine;
    IEnumerator openTipCoroutine;
    [SerializeField] float tipTransparencyChange;




    //--------


    void Start()
    {
        settings = FindObjectOfType<Settings>();
        UpdateProfile();
    }

    void Update()
    {
        HandleTips();
    }


    private IEnumerator CloseTip(Image image)
    {
        
        while(image.color.a > 0)
        {
            Color color = image.color;
            color.a -= tipTransparencyChange * Time.deltaTime;
            image.color = color;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        image.gameObject.SetActive(false);
    }

    private IEnumerator OpenTip(Image image)
    {
        image.gameObject.SetActive(true);

        while (image.color.a < 1)
        {
            Color color = image.color;
            color.a += tipTransparencyChange * Time.deltaTime;
            image.color = color;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        
    }
    void HandleTips()
    {
        if(changeTipCDCounter < 0)
        {
            ChangeTip();
            changeTipCDCounter = changeTipCD;
        }
        else
        {
            changeTipCDCounter -= Time.deltaTime;
        }
    }
    void ChangeTip()
    {

        int rand = UnityEngine.Random.Range(0, allTips.Length);
        
        while(rand == currentTipIndex)
        {
            rand = UnityEngine.Random.Range(0, allTips.Length);
        }

        currentTipIndex = rand;

        for(int i=0;i<allTips.Length;i++)
        {
            if(currentTipIndex == i)
            {
                openTipCoroutine = OpenTip(allTips[i]);
                StartCoroutine(openTipCoroutine);
            }
            else
            {
                closeTipCoroutine = CloseTip(allTips[i]);
                StartCoroutine(closeTipCoroutine);
            }
        }


    }
    void UpdateProfile()
    {
        nickNameTMP.text = settings.nickName;
        avatarImage.sprite = avatarSprites[settings.avatarIndex];
    }
   
    void CreateServer()
    {

        BoltLauncher.StartServer();

    }

    public void FindGameButtonOnClick()
    {
        findGameButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(true);
        createPrivateGameButton.gameObject.SetActive(false);
        joinPrivateGameButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        menuInfoText.text = "Finding game ...";

        settings.privateGame = false;
        privateGame = false;

        BoltLauncher.StartClient();

        

    }

    public void CreatePrivateGameButtonOnClick()
    {
        findGameButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(true);
        createPrivateGameButton.gameObject.SetActive(false);
        joinPrivateGameButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);

        menuInfoText.text = "Creating Game...";

        privateGame = true;
        settings.privateGame = true;
        BoltLauncher.StartServer();


    }
    public void JoinPrivateGameButtonOnClick()
    {
        findGameButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(true);
        createPrivateGameButton.gameObject.SetActive(false);
        joinPrivateGameButton.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        joinButton.gameObject.SetActive(true);
        inputFieldBackground.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(false);
    }

    string CreateRandomID()
    {
        string result = "";

        string st = "ABCDEFGHIJKLMNPRSTUVWYZ123456789";
        //string st = "123456789";
        for(int i=0;i<8;i++)
        {
            char c = st[UnityEngine.Random.Range(0,st.Length)];
            result += c;
        }

        return result;
    }

    public override void BoltStartDone()
    {
        if(BoltNetwork.IsServer)
        {
            if(privateGame)
            {
                PhotonRoomProperties token = new PhotonRoomProperties();
                token.IsOpen = true; // set if the room will be open to be joined
                token.IsVisible = false; // set if the room will be visible
                string matchname = CreateRandomID();

                BoltMatchmaking.CreateSession(
                    sessionID: matchname,
                    token,
                    sceneToLoad: "GameLevel"
                );
            }
            else
            {
                string matchname = CreateRandomID();
                BoltMatchmaking.CreateSession(
                    sessionID: matchname,
                    sceneToLoad: "GameLevel"
                );
            }
        }
        else
        {
            if(privateGame)
            {
                BoltMatchmaking.JoinSession(inputField.text);
                
            }
            else
            {
                BoltMatchmaking.JoinRandomSession();
            }
            
        }
    }


    public override void SessionConnectFailed(UdpSession session, IProtocolToken token, UdpSessionError errorReason)
    { 
        BoltLauncher.Shutdown();
        if(privateGame)
        {
            menuInfoText.text = "Game not found";
        }
        else
        {
            BoltLauncher.StartServer();
        }
        
    }

    public override void ConnectRefused(UdpEndPoint endpoint, IProtocolToken token)
    {
        BoltLauncher.Shutdown();

        BoltMatchmaking.JoinRandomSession();

    }


    public void CancelButtonOnClick()
    {
        findGameButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(false);
        createPrivateGameButton.gameObject.SetActive(true);
        joinPrivateGameButton.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);
        inputField.gameObject.SetActive(false);
        joinButton.gameObject.SetActive(false);
        inputFieldBackground.gameObject.SetActive(false);
        menuInfoText.text = "";
        inputField.text = "";

 
        BoltLauncher.Shutdown();
    }


    public void JoinButtonOnClick()
    {
        privateGame = true;
        settings.privateGame = true;

        BoltLauncher.StartClient();

        findGameButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(true);
        createPrivateGameButton.gameObject.SetActive(false);
        joinPrivateGameButton.gameObject.SetActive(false);
        inputFieldBackground.gameObject.SetActive(false);
        joinButton.gameObject.SetActive(false);

        menuInfoText.text = "Connecting...";

    }

    public void OpenRuleBookButtonOnClick()
    {
        ruleBookBackground.gameObject.SetActive(true);
        ruleBookButton.gameObject.SetActive(false);

        currentRuleBookIndex = 0;
        for(int i=0;i<ruleBookImages.Length;i++)
        {
            if (i != currentRuleBookIndex) ruleBookImages[i].gameObject.SetActive(false);
            else ruleBookImages[i].gameObject.SetActive(true);
        }

        ruleNumberTMP.text = (currentRuleBookIndex + 1).ToString() + " / " + ruleBookImages.Length.ToString();

    }

    public void CloseRuleBookButtonOnClick()
    {
        ruleBookBackground.gameObject.SetActive(false);
        ruleBookButton.gameObject.SetActive(true);
    }


    public void ProfileButtonOnClick()
    {
        profileScreen.gameObject.SetActive(true);
        nickNameInputField.text = settings.nickName + " sikerim seni";
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            if (i == settings.avatarIndex) avatarButtons[i].image.sprite = selectedAvatarSprite;
            else avatarButtons[i].image.sprite = nonselectedAvatarSprite;
        }

        referenceAvatarImage.sprite = avatarSprites[selectedAvatarIndex];

    }

    public void SaveButtonOnClick()
    {
        settings.nickName = nickNameInputField.text;
        settings.avatarIndex = selectedAvatarIndex;
    }


    public void BackButtonOnClick()
    {
        profileScreen.gameObject.SetActive(false);
        creditsScreen.gameObject.SetActive(false);
        UpdateProfile();
    }

    public void AvatarButtonOnClick(int index)
    {
        selectedAvatarIndex = index;
        referenceAvatarImage.sprite = avatarSprites[selectedAvatarIndex];
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            if (i == selectedAvatarIndex) avatarButtons[i].image.sprite = selectedAvatarSprite;
            else avatarButtons[i].image.sprite = nonselectedAvatarSprite;
        }
    }


    public void CreditsButtonOnClick()
    {
        creditsScreen.gameObject.SetActive(true);
    }


    public void IncrementRuleBookIndex()
    {
        if (currentRuleBookIndex < ruleBookImages.Length - 1) currentRuleBookIndex++;
        else return;

        for (int i = 0; i < ruleBookImages.Length; i++)
        {
            if (i != currentRuleBookIndex) ruleBookImages[i].gameObject.SetActive(false);
            else ruleBookImages[i].gameObject.SetActive(true);
        }

        ruleNumberTMP.text = (currentRuleBookIndex + 1).ToString() + " / " + ruleBookImages.Length.ToString();

    }

    public void DecreaseRuleBookIndex()
    {
        if (currentRuleBookIndex > 0) currentRuleBookIndex--;
        else return;

        for (int i = 0; i < ruleBookImages.Length; i++)
        {
            if (i != currentRuleBookIndex) ruleBookImages[i].gameObject.SetActive(false);
            else ruleBookImages[i].gameObject.SetActive(true);
        }

        ruleNumberTMP.text = (currentRuleBookIndex + 1).ToString() + " / " + ruleBookImages.Length.ToString();

    }
    
}
