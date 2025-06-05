using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIConsoleLog
{
    public string message;
    public string stackTrace;
    public LogType type;
}

public class GUIConsole : MonoBehaviourSingleton<GUIConsole>
{
    List<GUIConsoleLog> logs = new List<GUIConsoleLog>();

    bool isOpen = false;

    int padding = 16;
    int x = 0;
    int y = 0;
    int width = 600;
    int height = 300;

    Vector2 scrollPosition = Vector2.zero;

    public override void Awake()
    {
        base.Awake();

        x = Screen.width - width - padding;
        y = Screen.height - height - padding;

        Application.logMessageReceived += OnLogMessageReceived;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= OnLogMessageReceived;
    }

    public void OnGUI()
    {
        if (!isOpen)
            return;

        GUI.backgroundColor = new Color(0, 0, 0, 1.0f);
        GUILayout.BeginArea(new Rect(x, y, width, height), GUI.skin.box);
        GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        scrollPosition = GUILayout.BeginScrollView(
            scrollPosition,
            alwaysShowHorizontal: false,
            alwaysShowVertical: true,
            GUI.skin.horizontalScrollbar,
            GUI.skin.verticalScrollbar,
            GUI.skin.scrollView,
            new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) }
        );

        foreach (var log in logs)
        {
            GUI.color =
                log.type == LogType.Error
                || log.type == LogType.Exception
                || log.type == LogType.Assert
                    ? Color.red
                : log.type == LogType.Warning ? Color.yellow
                : Color.white;

            GUILayout.Label(log.message, new GUILayoutOption[] { GUILayout.ExpandWidth(true) });

            if (!string.IsNullOrEmpty(log.stackTrace))
                GUILayout.Label(
                    log.stackTrace,
                    new GUILayoutOption[] { GUILayout.ExpandWidth(true) }
                );
        }

        GUILayout.EndScrollView();

        GUILayout.EndArea();
    }

    public void Open()
    {
        isOpen = true;
    }

    public void Close()
    {
        isOpen = false;
    }

    void OnLogMessageReceived(string message, string stackTrace, LogType type)
    {
        logs.Add(
            new GUIConsoleLog
            {
                message = message,
                stackTrace = stackTrace,
                type = type,
            }
        );

        if (logs.Count > 100)
            logs.RemoveAt(0);

        scrollPosition.y = float.MaxValue;
    }
}
