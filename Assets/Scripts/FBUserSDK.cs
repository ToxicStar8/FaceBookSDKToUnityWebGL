// **************************************************************
// Script Name: 
// Author: WangYS
// Time : 2022/10/09 10:48:21
// Des: 描述
// **************************************************************

using AOT;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class FBUserSDK : MonoBehaviour
{
    #region 广告
    /// <summary>
    /// 预加载广告
    /// </summary>
    [DllImport("__Internal")]
    private static extern void FBLoadAds();
    /// <summary>
    /// 预加载广告的回调
    /// </summary>
    private Action<string> _loadAdsCallback;
    /// <summary>
    /// 预加载广告 0=失败 1=成功
    /// </summary>
    /// <param name="callback">回调</param>
    public void FnLoadAds(Action<string> callback = null)
    {
        _loadAdsCallback = callback;
        FBLoadAds();
    }
    /// <summary>
    /// 预加载广告JS执行回调
    /// </summary>
    [MonoPInvokeCallback(typeof(void))]
    private void LoadAdsCallback(string status)
    {
        _loadAdsCallback?.Invoke(status);
        _loadAdsCallback = null;
    }



    /// <summary>
    /// 显示广告 0=失败 1=成功
    /// </summary>
    [DllImport("__Internal")]
    private static extern void FBShowAds();
    /// <summary>
    /// 显示广告的回调
    /// </summary>
    private Action<string> _showAdsCallback;
    /// <summary>
    /// 显示广告
    /// </summary>
    /// <param name="callback">回调</param>
    public void FnShowAds(Action<string> callback = null)
    {
        _showAdsCallback = callback;
        FBShowAds();
    }
    /// <summary>
    /// 预加载广告JS执行回调
    /// </summary>
    [MonoPInvokeCallback(typeof(void))]
    private void ShowAdsCallback(string status) { 
        _showAdsCallback?.Invoke(status);
        _showAdsCallback = null;
        //无论结果 看完广告再加载一个
        FnLoadAds();
    }
    #endregion

    #region 验证登录
    /// <summary>
    /// FB的验证登录
    /// </summary>
    [DllImport("__Internal")]
    private static extern string FBCalibrationToken(string token);
    /// <summary>
    /// 验证登录Token的回调
    /// </summary>
    private Action<string> _calibrationTokenCallback;
    /// <summary>
    /// 验证登录Token
    /// </summary>
    /// <param name="token">服务端传来的md5</param>
    /// <param name="callback">回调</param>
    public void FnCalibrationToken(string token, Action<string> callback = null)
    {
        _calibrationTokenCallback = callback;
        FBCalibrationToken(token);
    }
    /// <summary>
    /// 验证登录JS执行回调
    /// </summary>
    [MonoPInvokeCallback(typeof(string))]
    private void CalibrationTokenCallback(string token) { 
        _calibrationTokenCallback?.Invoke(token);
        _calibrationTokenCallback = null;
    }
    #endregion

    #region 好友
    /// <summary>
    /// 分享给好友 0=失败 1=成功
    /// </summary>
    [DllImport("__Internal")]
    private static extern void FBPlayWithFriends();
    /// <summary>
    /// 分享给好友的回调
    /// </summary>
    private Action _playWithFriendsCallback;
    /// <summary>
    /// 分享给好友
    /// </summary>
    /// <param name="callback">回调</param>
    public void FnPlayWithFriends(Action callback = null)
    {
        _playWithFriendsCallback = callback;
        FBPlayWithFriends();
    }
    /// <summary>
    /// 分享给好友JS执行回调
    /// </summary>
    [MonoPInvokeCallback(typeof(void))]
    private void PlayWithFriendsCallback()
    {
        _playWithFriendsCallback?.Invoke();
        _playWithFriendsCallback = null;
    }



    /// <summary>
    /// 获得玩家好友列表
    /// </summary>
    [DllImport("__Internal")]
    private static extern void FBGetFriendInfo();
    /// <summary>
    /// 获得玩家好友列表的回调
    /// </summary>
    private Action<string> _getFriendInfoCallback;
    /// <summary>
    /// 获得玩家好友列表
    /// </summary>
    /// <param name="callback">回调</param>
    public void FnGetFriendInfo(Action<string> callback = null)
    {
        _getFriendInfoCallback = callback;
        FBGetFriendInfo();
    }
    /// <summary>
    /// 获得玩家好友列表JS执行回调
    /// </summary>
    [MonoPInvokeCallback(typeof(string))]
    private void GetFriendInfoCallbackCallback(string friendsInfo) { 
        _getFriendInfoCallback?.Invoke(friendsInfo);
        _getFriendInfoCallback = null;
    }
    #endregion

    #region 玩家信息
    /// <summary>
    /// 获得玩家Id
    /// </summary>
    [DllImport("__Internal")]
    private static extern string FBGetPlayerId();
    public string FnGetPlayerId() => FBGetPlayerId();

    /// <summary>
    /// 获得玩家名
    /// </summary>
    [DllImport("__Internal")]
    private static extern string FBGetPlayerName();
    public string FnGetPlayerName() => FBGetPlayerName();

    /// <summary>
    /// 获得玩家头像
    /// </summary>
    [DllImport("__Internal")]
    private static extern string FBGetPlayerPhoto();
    public string FnGetPlayerPhoto() => FBGetPlayerPhoto();
    #endregion

    /// <summary>
    /// Js调用Log
    /// </summary>
    /// <param name="log"></param>
    [MonoPInvokeCallback(typeof(string))]
    private void Log(string log)
    {
        Debug.Log(log);
    }
}
