using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class PlayFabUserMgtTMP : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameoremail, password, displayname, ForgetPass, SignUpUsername, SignUpEmail, SignUpPassword, SignUpConfirmPassword;
    [SerializeField] TextMeshProUGUI msg, resetsent;
    [SerializeField] GameObject ForgetPasswordField, Login, SignUp, Loading;
    private void Start()
    {
        ShowOrHideLoading(false);
    }
    public void OnButtonReg()
    {
        ShowOrHideLoading(true);
        if (SignUpConfirmPassword.text == SignUpPassword.text)
            OnButtonReguser();
        else
        {
            ShowOrHideLoading(false);
            msg.text = "Passwords do not match";
        }
    }
    public void OnButtonReguser()
    {
        var registerRequest = new RegisterPlayFabUserRequest
        {
            Email = SignUpEmail.text,
            Password = SignUpPassword.text,
            Username = SignUpUsername.text
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegSuccess, OnError);
    }

    public void OnButtonLogin()
    {
        ShowOrHideLoading(true);
        if (IsValidEmail(usernameoremail.text))
        {
            OnButtonLoginEmail();
        }
        else
            OnButtonLoginUserName();
    }

    public void ShowOrHideLoading(bool setto)
    {
        Loading.SetActive(setto);
    }

    private bool IsValidEmail(string email)
    {
        // Regex pattern to validate email addresses
        // This is a simple pattern and may not cover all valid email addresses
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        return Regex.IsMatch(email, pattern);
    }

    public void OnButtonLoginEmail()
    {
        var loginRequest = new LoginWithEmailAddressRequest
        {
            Email = usernameoremail.text,
            Password = password.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(loginRequest, OnLoginSuccess, OnError);
    }
    public void OnButtonLoginUserName()
    {
        var loginRequest = new LoginWithPlayFabRequest
        {
            Username = usernameoremail.text,
            Password = password.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithPlayFab(loginRequest, OnLoginSuccess, OnError);
    }
    void OnLoginSuccess(LoginResult r)
    {
        //Debug.Log("Login Success");
        //msg.text = "Success" + r.PlayFabId + r.InfoResultPayload.PlayerProfile.DisplayName;
        PlayFabHandler.PlayFabID = r.PlayFabId;

        //ClientGetTitleData(); //MOTD
        GetUserData();
    }
    void OnRegSuccess(RegisterPlayFabUserResult r)
    {
        ShowOrHideLoading(false);
        UpdateMsg("Register Success");

        var req = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayname.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(req, OnDisplayNameUpdate, OnError);
        WantToSignUp();
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult r)
    {
        //UpdateMsg("Display name updated! " + r.DisplayName);
    }

    void OnError(PlayFabError e)
    {
        ShowOrHideLoading(false);
        ErrorMessage(e.GenerateErrorReport());
    }

    void ErrorMessage(string MSG)
    {
        string ConvertErrorMessage = "";
        if (MSG.Contains(':'))
        {
            int positionofcolon = 0;
            for (int i = 0; i < MSG.Length; ++i)
            {
                if (MSG[i] == ':')
                {
                    positionofcolon = i;
                }
            }
            ConvertErrorMessage = MSG.Substring(positionofcolon + 1, MSG.Length - positionofcolon - 1);
        }
        else
            ConvertErrorMessage = MSG;

        UpdateMsg(ConvertErrorMessage);
    }

    void UpdateMsg(string MSG)
    {
        Debug.Log(MSG);
        msg.text = MSG;
    }

    //public void OnButtonGetLeaderboard()
    //{
    //    var Ibreq = new GetLeaderboardRequest
    //    {
    //        StatisticName = "HighScore",
    //        StartPosition = 0,
    //        MaxResultsCount = 10
    //    };
    //    PlayFabClientAPI.GetLeaderboard(Ibreq, OnLeaderboardGet, OnError);
    //}

    //void OnLeaderboardGet(GetLeaderboardResult r)
    //{
    //    string LeaderboardStr = "Leaderboard\n";
    //    foreach(var item in r.Leaderboard)
    //    {
    //        string onerow = item.Position + "/" + item.DisplayName + "/" + item.StatValue + "\n";
    //        LeaderboardStr += onerow;
    //    }
    //    UpdateMsg(LeaderboardStr);
    //}

    //public void OnButtonSendLeaderboard()
    //{
    //    var req = new UpdatePlayerStatisticsRequest
    //    {
    //        Statistics = new List<StatisticUpdate>
    //        {
    //            new StatisticUpdate
    //            {
    //                StatisticName = "HighScore",
    //                Value=int.Parse(currentscore.text)
    //            }
    //        }
    //    };
    //    UpdateMsg("Submitting Score: " + currentscore.text);
    //    PlayFabClientAPI.UpdatePlayerStatistics(req, OnLeaderboardUpdate, OnError);
    //}

    //void OnLeaderboardUpdate(UpdatePlayerStatisticsResult r)
    //{
    //    UpdateMsg("Successful leaderboard sent" + r.ToString());
    //}

    //public void ClientGetTitleData()
    //{
    //    PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
    //        result => {
    //            if (result.Data == null || !result.Data.ContainsKey("MOTD")) Debug.Log("MOTD");
    //            else
    //            {
    //                StaticGlobal.Message = "MOTD: " + result.Data["MOTD"];
    //                //
    //            }
    //        },
    //        error => {
    //            Debug.Log("Got error getting titleData:");
    //            Debug.Log(error.GenerateErrorReport());
    //        }
    //    );
    //}

    public void SeePassword()
    {
        //Debug.Log("CHANGING");
        if (password.GetComponent<TMP_InputField>().contentType != TMP_InputField.ContentType.Standard)
            password.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Standard;
        else
            password.GetComponent<TMP_InputField>().contentType = TMP_InputField.ContentType.Password;

        password.ForceLabelUpdate();
    }

    public void WantToResetPassword()
    {
        resetsent.text = "";
        msg.text = "";
        if (ForgetPasswordField.activeSelf)
        {
            msg.text = "";
            ForgetPasswordField.SetActive(false);
            Login.SetActive(true);
        }
        else
        {
            msg.text = "Reset Password";
            ForgetPasswordField.SetActive(true);
            Login.SetActive(false);
        }

    }
    public void WantToSignUp()
    {
        msg.text = "";
        if (SignUp.activeSelf)
        {
            SignUp.SetActive(false);
            Login.SetActive(true);
        }
        else
        {
            SignUp.SetActive(true);
            Login.SetActive(false);
        }

    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = ForgetPass.text,
            TitleId = "B1904"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnErrorReset);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        resetsent.text = "Sent reset to email";
    }

    void OnErrorReset(PlayFabError e)
    {
        string MSG = e.GenerateErrorReport();
        string ConvertErrorMessage = "";
        if (MSG.Contains(':'))
        {
            int positionofcolon = 0;
            for (int i = 0; i < MSG.Length; ++i)
            {
                if (MSG[i] == ':')
                {
                    positionofcolon = i;
                }
            }
            ConvertErrorMessage = MSG.Substring(positionofcolon + 1, MSG.Length - positionofcolon - 1);
        }
        else
            ConvertErrorMessage = MSG;

        resetsent.text = ConvertErrorMessage;
    }

    //public void OnButtonSetUserData()
    //{
    //    PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
    //    {
    //        Data = new Dictionary<string, string>()
    //        {
    //            {"XP", XP.text.ToString() },
    //            {"Level", Level.text.ToString() },
    //            {"Score", Score.text.ToString() }
    //        }
    //    },
    //    result => UpdateMsg("Successfully Updated user data"),
    //    error =>
    //    {
    //        UpdateMsg("Error in getting user data");
    //        UpdateMsg(error.GenerateErrorReport());
    //    });
    //}

    public void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            , result =>
            {
                if (result.Data == null || !result.Data.ContainsKey("BGM"))
                {
                    Debug.Log("No BGM");
                    PlayFabHandler.BGMSliderValue = 0.5f;
                }
                else
                {
                    PlayFabHandler.BGMSliderValue = float.Parse(result.Data["BGM"].Value);
                }
                if (result.Data == null || !result.Data.ContainsKey("Weapon"))
                {
                    Debug.Log("No WeaponSFX");
                    PlayFabHandler.WeaponSliderValue = 0.5f;
                }
                else
                {
                    PlayFabHandler.WeaponSliderValue = float.Parse(result.Data["Weapon"].Value);
                }
                if (result.Data == null || !result.Data.ContainsKey("Interaction"))
                {
                    Debug.Log("No InteractionSFX");
                    PlayFabHandler.InteractionSliderValue = 0.5f;
                }
                else
                {
                    PlayFabHandler.InteractionSliderValue = float.Parse(result.Data["Interaction"].Value);
                }
                if (result.Data == null || !result.Data.ContainsKey("ShowFPS"))
                {
                    Debug.Log("No FPS");
                    PlayFabHandler.ShowFPS = true;
                }
                else
                {
                    if ((result.Data["ShowFPS"].Value) == "True")
                    {
                        PlayFabHandler.ShowFPS = true;
                    }
                    else
                    {
                        PlayFabHandler.ShowFPS = false;
                    }
                }
                GetPlayerHighScore();
            }
            , (error) =>
            {
                UpdateMsg("Error in getting user data");
                UpdateMsg(error.GenerateErrorReport());
            });
    }
    public void GetPlayerHighScore()
    {
        var HighScore = new GetPlayerStatisticsRequest
        {
            StatisticNames = new List<string> { "HighScore" }
        };
        PlayFabClientAPI.GetPlayerStatistics(HighScore,
            result =>
            {
                StatisticValue HighScoreString = result.Statistics.Find(score => score.StatisticName == "HighScore"); //get the highscore
                if (HighScoreString != null)
                {
                    //Debug.Log(HighScoreString.Value);

                    PlayFabHandler.HighScore = HighScoreString.Value; //assign the highscore
                    //Debug.Log(HighScore);
                }
                PlayFabHandler.GetVirtualCurrencies();
            }
            , OnError);
    }

}
