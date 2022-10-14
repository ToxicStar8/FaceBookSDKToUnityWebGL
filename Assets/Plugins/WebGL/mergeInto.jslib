var func = {
    //============================= 回调相关开始 =============================
    //回调测试
    //v=void i=int或指针类型 viiii=void,int或指针类型,int或指针类型,int或指针类型,int或指针类型
    //零个参数是 Runtime.dynCall('v', JsCallCsTest.callback);
    //一个参数是 Runtime.dynCall('vi', JsCallCsTest.callback, [param1]);
    //四个参数是 Runtime.dynCall('viiii', JsCallCsTest.callback, [param1, param2, param3, param4]);
    //$JsCallCsTest: {},
    //ProvideCallback: function (obj) {
    //var returnStr = "error";
    //var bufferSize = lengthBytesUTF8(returnStr) + 1;
    //var buffer = _malloc(bufferSize);
    //stringToUTF8(returnStr, buffer, bufferSize);

    //JsCallCsTest.callback = obj;
    //Module.dynCall_vii('vi', JsCallCsTest.callback,buffer);
    //},

    $MsgMgr: {
        //转C#字符串方法
        ToString: function (returnStr) {
            var bufferSize = lengthBytesUTF8(returnStr) + 1;
            var buffer = _malloc(bufferSize);
            stringToUTF8(returnStr, buffer, bufferSize);
            return buffer;
        },

        //Log
        Log: function (log) {
            gameInstance.SendMessage("FBUserSDK", "Log", log);
        },

        //"FBUserSDK"同时也是游戏对象的名字
        //fnName是类下面的方法名
        //param 参数为string 不需要转换
        SendMessage: function (fnName, param) {
            gameInstance.SendMessage("FBUserSDK", fnName, param);
        },
    },
    //============================= 回调相关结束 =============================

    //============================= FB SDK相关开始 =============================
    //FB 校验
    FBCalibrationToken: function (str) {
        MsgMgr.Log('FB Calibration Token');
        var returnStr = null;
        FBInstant.player.getSignedPlayerInfoAsync(UTF8ToString(str))
            .then(function (result) {
                returnStr = result.getSignature();
                MsgMgr.Log("SignInfo=" + returnStr);
                MsgMgr.SendMessage('CalibrationTokenCallback', returnStr);
            });
    },

    //获得玩家Id
    FBGetPlayerId: function () {
        MsgMgr.Log('FB Get PlayID');
        return MsgMgr.ToString(FBInstant.player.getID());
    },

    //获得玩家名
    FBGetPlayerName: function () {
        MsgMgr.Log('FB Get PlayName');
        return MsgMgr.ToString(FBInstant.player.getName());
    },

    //获得玩家头像
    FBGetPlayerPhoto: function () {
        MsgMgr.Log('FB Get PlayPhoto');
        return MsgMgr.ToString(FBInstant.player.getPhoto());
    },

    //玩家从哪里进入的游戏
    //FBInstant.getEntryPointAsync().then(function (data) {console.log("player.getEntryPointAsync=" + data);})

    //获取和当前玩家有关联的玩家列表信息（即好友列表）  Json字符串  取不到数据，可能是没有好友的原因
    FBGetFriendInfo: function () {
        MsgMgr.Log('FB Get Friend Info');
        FBInstant.player.getConnectedPlayersAsync().then(function (result) {
            var friendsInfo = result.map(function (result) {
                MsgMgr.Log('result='+result);
                return {
                    id: player.getID(),
                    name: player.getName(),
                }
            });
            MsgMgr.Log('playInfo get complete');
            MsgMgr.Log('friendsInfo JSON.stringify=' + JSON.stringify(friendsInfo));
            MsgMgr.SendMessage('GetFriendInfoCallbackCallback', JSON.stringify(friendsInfo));
        });
    },

    //分享给好友
    FBPlayWithFriends: function () {
        MsgMgr.Log('FB Play With Friends Start');
        FBInstant.player.getName();
        FBInstant.context.chooseAsync().then(function () {
            FBInstant.updateAsync({
                action: "CUSTOM",
                cta: "Join Now",
                template: "play_with_friends",
                text: { default: player + "invited you to play Fantasy Clash" },
                image: base64,
            })
        }).then(function () {
            //回调
            MsgMgr.Log('FB Play With Friends Close');
            MsgMgr.SendMessage('PlayWithFriendsCallback');
        }).catch(function (error) {
            MsgMgr.Log('FB Play With Friends Error=' + error.message);
            MsgMgr.SendMessage('PlayWithFriendsCallback');
        });
    },
    //============================= FB SDK相关结束 =============================

    //============================= 广告相关开始 =============================
    $AdMgr: {
        AdList: [],
    },

    //加载广告
    FBLoadAds: function () {
        MsgMgr.Log('Ads Load Start');
        var preloadedRewardedVideo = null;
        FBInstant.getRewardedVideoAsync('VID_HD_9_16_39S_APP_INSTALL#YOUR_PLACEMENT_ID').then(function (rewarded) {
            preloadedRewardedVideo = rewarded;
            preloadedRewardedVideo.loadAsync().then(function () {
                AdMgr.AdList.push(preloadedRewardedVideo);
                MsgMgr.Log('Ads Load Complete');
                MsgMgr.Log('AdMgr.AdList.Length=' + AdMgr.AdList.length);
                MsgMgr.SendMessage('LoadAdsCallback', '1');
            }).catch(function (error) {
                MsgMgr.Log('Ads Load Error=' + error.message);
                MsgMgr.SendMessage('LoadAdsCallback', '0');
            });
        });
    },

    //展示广告
    FBShowAds: function () {
        MsgMgr.Log('Ads Show Start');
        MsgMgr.Log('AdMgr.AdList.Length=' + AdMgr.AdList.length);
        if (AdMgr.length > 0) {
            var preloadedRewardedVideo = AdMgr.AdList.pop();
            preloadedRewardedVideo.showAsync().then(function () {
                MsgMgr.Log('Ads Show End');
                MsgMgr.SendMessage('ShowAdsCallback', '1');
            }).catch(function (error) {
                MsgMgr.Log('Ads Show Error=' + error.message);
                MsgMgr.SendMessage('ShowAdsCallback', '0');
            });
        }
    },
    //============================= 广告相关结束 =============================
};

//带$的相当于JS内部建立一个静态类，可以被JS内部自己调用，需要调用autoAddDeps添加
autoAddDeps(func, '$MsgMgr');
autoAddDeps(func, '$AdMgr');
mergeInto(LibraryManager.library, func);