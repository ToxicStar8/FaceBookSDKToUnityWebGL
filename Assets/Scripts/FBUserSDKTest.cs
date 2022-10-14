// **************************************************************
// Script Name: 
// Author: WangYS
// Time : 2022/10/09 17:30:45
// Des: 描述
// **************************************************************

using UnityEngine;
using UnityEngine.UI;

public class FBUserSDKTest : MonoBehaviour
{
    public FBUserSDK FBUser;
    public InputField Input_Text;

    public void FnLoadAds()
    {
        FBUser.FnLoadAds((status) => Debug.Log("FnLoadAds End,status=" + status));
    }

    public void FnShowAds()
    {
        FBUser.FnShowAds((status) => Debug.Log("FnShowAds End,status=" + status));
    }

    public void FnCalibrationToken()
    {
        FBUser.FnCalibrationToken(Input_Text.text, (token) => Debug.Log("FnCalibrationToken End,token=" + token));
    }

    public void FnPlayWithFriends()
    {
        FBUser.FnPlayWithFriends(() => Debug.Log("FnPlayWithFriends End"));
    }

    public void FnGetFriendInfo()
    {
        FBUser.FnGetFriendInfo((token) => Debug.Log("FnGetFriendInfo End,friendInfo=" + token));
    }

    public void FnGetPlayerInfo()
    {
        var playerId = FBUser.FnGetPlayerId();
        var playerName = FBUser.FnGetPlayerName();
        var playerHead = FBUser.FnGetPlayerPhoto();
        Debug.Log($"playerId={playerId},playerName={playerName},playerHead={playerHead}");
    }
}
