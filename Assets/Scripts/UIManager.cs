using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private GameObject loginPanel;

    [SerializeField]
    private GameObject registrationPanel;

    [SerializeField]
    private GameObject gamePanel;

    [Space]
    [SerializeField]
    private GameObject emailVerificationPanel;

    [Space]
    [SerializeField]
    private GameObject scoreboardPanel;

    [SerializeField]
    private Text emailVerificationText;

    [Space]
    [Header("Profile Picture Update Data")]
    public GameObject profileUpdatePanel;
    public Image profileImage;
    public InputField urlInputField;

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void ClearUI()
    {
        loginPanel.SetActive(false);
        registrationPanel.SetActive(false);
        emailVerificationPanel.SetActive(false);
        gamePanel.SetActive(false);
        profileUpdatePanel.SetActive(false);
        scoreboardPanel.SetActive(false);
    }

    public void OpenLoginPanel()
    {
        ClearUI();
        loginPanel.SetActive(true);
    }

    public void OpenRegistrationPanel()
    {
        ClearUI();
        registrationPanel.SetActive(true);
    }

    public void OpenGamePanel()
    {
        ClearUI();
        gamePanel.SetActive(true);
    }

    public void OpenProfileUpdatePanel()
    {
        ClearUI();
        //profileUpdatePanel.SetActive(!profileUpdatePanel.activeSelf);
        profileUpdatePanel.SetActive(true);
    }

    public void OpenScoreboardPanel()
    {
        ClearUI();
        //profileUpdatePanel.SetActive(!profileUpdatePanel.activeSelf);
        scoreboardPanel.SetActive(true);
    }

    public void ShowVerificationResponse(bool isEmailSent, string emailId, string errorMessage)
    {
        ClearUI();
        emailVerificationPanel.SetActive(true);

        if (isEmailSent)
        {
            emailVerificationText.text = $"Please verify your email address \n Verification email has been sent to {emailId}";
        }
        else
        {
            emailVerificationText.text = $"Email couldn't be sent : {errorMessage}";
        }
    }

    public void LoadProfileImage(string url)
    {
        StartCoroutine((LoadProfileImageIE(url)));
    } 

    public IEnumerator LoadProfileImageIE(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();


#pragma warning disable CS0618 // Type or member is obsolete
        if (www.isNetworkError || www.isHttpError)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());
            profileImage.sprite = sprite;
            profileUpdatePanel.SetActive(false);
        }
    }

    public string GetProfileUpdateURL()
    {
        return urlInputField.text;
    }
}
