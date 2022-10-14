/*******************************************************************
 *版权(C) 2019 by 框架
 *脚本名:    Log.cs
 *作者:      songqz
 *版本:      1.0
 *引擎版本:  2018.4.3f1
 *创建时间:  2020-12-02
 *描述:    游戏日志
 *历史记录: 
******************************************************************/

using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Log : MonoBehaviour
{
    public static Log Instance;
    private Vector2 _scorllPos;
    private bool _isShowLog = false;
    private float _logBtnWidth = 80;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Log脚本重复挂载");
        }
        Instance = this;
        Application.logMessageReceived -= LogCallback;
        Application.logMessageReceived += LogCallback;
    }

    #region 日志输出相关
    private Dictionary<LogType, StringBuilder> _logDic = new Dictionary<LogType, StringBuilder>()
    {
        [LogType.Log] = new StringBuilder(""),
        [LogType.Error] = new StringBuilder(""),
        [LogType.Warning] = new StringBuilder(""),
    };
    private LogType _logType = LogType.Log;
    private void LogCallback(string condition, string stackTrace, LogType type)
    {
        LogType saveLog = type;
        if((type != LogType.Log) && (type != LogType.Warning))
        {
            saveLog = LogType.Error;
        }

        if(!_logDic.TryGetValue(saveLog, out StringBuilder sb))
        {
            sb = new StringBuilder("");
            _logDic.Add(saveLog, sb);
        }

        sb.Insert(0, "\n\n");
        if (type != LogType.Log)
        {
            sb.Insert(0, stackTrace);
            sb.Insert(0, "\n");
        }
        sb.Insert(0, condition);
        sb.Insert(0, ":");
        sb.Insert(0,type.ToString());

        if ((type == LogType.Error)|| (type == LogType.Exception))
        {
            _logType = LogType.Error;
            _isShowLog = true;
        }
    }

    private void OnGUI()
    {
        GUILayout.Space(200);
        GUI.color = new Color(1, 1, 1);
        GUI.skin.button.fontSize = 15;
        if (GUI.Button(new Rect(Screen.width/2 - 25, Screen.height - 30, 50, 30), "Log")) 
        {
            _isShowLog = !_isShowLog;
        }
        // 日志输出
        if (!_isShowLog)
        {
            return;
        }

        if (GUILayout.Button("Log", GUILayout.Height(35), GUILayout.Width(_logBtnWidth)))
        {
            _scorllPos = Vector2.zero;
            _logType = LogType.Log;
        }

        if (GUILayout.Button("Error", GUILayout.Height(35), GUILayout.Width(_logBtnWidth)))
        {
            _scorllPos = Vector2.zero;
            _logType = LogType.Error;
        }

        if (GUILayout.Button("Warnning", GUILayout.Height(35), GUILayout.Width(_logBtnWidth)))
        {
            _scorllPos = Vector2.zero;
            _logType = LogType.Warning;
        }

        if (GUILayout.Button("Clear", GUILayout.Height(35), GUILayout.Width(_logBtnWidth)))
        {
            foreach (var item in _logDic.Values)
            {
                item.Clear();
            }
        }

        GUILayout.BeginHorizontal();
        GUILayout.Space(_logBtnWidth);
        _scorllPos = GUILayout.BeginScrollView(_scorllPos, GUILayout.Height(1000), GUILayout.Width(650));
        GUILayout.TextArea(_logDic[_logType].ToString(), GUILayout.Width(730));
        GUILayout.EndScrollView();
        GUILayout.EndHorizontal();
    }
    #endregion

    private void OnDestroy()
    {
        Application.logMessageReceived -= LogCallback;
    }
}
