using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lunity
{
    public static class LUI
    {
        
        //Todo - add more of these
        //https://unitylist.com/p/5c3/Unity-editor-icons
        public enum Icon
        {
            Search = 0,
            Trash = 1,
            Plus = 2,
            Minus = 3,
            Next = 4,
            Prev = 5,
            StepRight = 6,
            StepLeft = 7,
            Gear = 8,
            Eye = 9,
            Info = 10,
            Warning = 11,
            Error = 12,
            Help = 13,
            VerticalDots = 14,
            SkipBack = 15,
            SkipNext = 16,
            Play = 17,
            Pause = 18,
            Lock = 19,
            Web = 20,
            Check = 21,
            Speaker = 22,
            Loop = 23,
        }

        private static string[] Icons =
        {
            "Search Icon",
            "TreeEditor.Trash",
            "Toolbar Plus",
            "Toolbar Minus",
            "tab_next",
            "tab_prev",
            "StepButton",
            "StepLeftButton",
            "SettingsIcon",
            "scenevis_visible_hover",
            "console.infoicon.sml",
            "console.warnicon.sml",
            "console.erroricon.sml",
            "_Help",
            "_Menu",
            "Animation.FirstKey",
            "Animation.LastKey",
            "PlayButton",
            "PauseButton",
            "AssemblyLock",
            "BuildSettings.Web.Small",
            "FilterSelectedOnly",
            "Profiler.Audio",
            "preAudioLoopOff",
        };

        public static GUIContent GetIcon(Icon icon) => EditorGUIUtility.IconContent((EditorGUIUtility.isProSkin ? "d_" : "") + Icons[(int) icon]);
        
        public static Rect LeftAlign(Rect rect, float width, float offset = 0f)
        {
            return new Rect(rect.x + offset, rect.y, width, rect.height);
        }

        public static Rect RightAlign(Rect rect, float width, float offset = 0f)
        {
            return new Rect(rect.x + rect.width - width + offset, rect.y, width, rect.height);
        }
        
        public static void ReorderableList<T>(ref List<T> list, string label, Action<Rect, int> onDrawItem = null, UnityEngine.Object undoContext = null, int buttonSize = 25)
        {
            var threeButton = buttonSize * 3;

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox)) {
                var headerRect = EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("");
                if (!string.IsNullOrEmpty(label)) {
                    EditorGUI.LabelField(LeftAlign(headerRect, headerRect.width - threeButton), label, EditorStyles.boldLabel);
                }
                var newCount = EditorGUI.DelayedIntField(RightAlign(headerRect, threeButton), list.Count);
                if (newCount < 0) newCount = 0;
                
                if (newCount != list.Count) {
                    if (undoContext != null) Undo.RecordObject(undoContext, "Changed list count");
                    if (newCount > list.Count) {
                        while (list.Count < newCount) list.Add(list.Count == 0 ? default : list[list.Count - 1]);
                    } else {
                        while (list.Count > newCount) list.RemoveAt(list.Count - 1);
                    }
                }
                EditorGUILayout.EndHorizontal();
                
                for (var i = 0; i < list.Count; i++) {
                    var rect = EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("");
                    onDrawItem?.Invoke(LeftAlign(rect, rect.width - threeButton), i);
                    if (i > 0) {
                        if (GUI.Button(RightAlign(rect, buttonSize, -buttonSize * 2f), "▲")) {
                            if (undoContext != null) Undo.RecordObject(undoContext, "Reordered list");
                            var item = list[i - 1];
                            list[i - 1] = list[i];
                            list[i] = item;
                        }
                    }

                    if (GUI.Button(RightAlign(rect, buttonSize, -buttonSize), "X")) {
                        if (undoContext != null) Undo.RecordObject(undoContext, "Removed list item");
                        list.RemoveAt(i);
                    }

                    if (i < list.Count - 1) {
                        if (GUI.Button(RightAlign(rect, buttonSize), "▼")) {
                            if (undoContext != null) Undo.RecordObject(undoContext, "Reordered list");
                            var item = list[i];
                            list[i] = list[i + 1];
                            list[i + 1] = item;
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }

                var footerRect = EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("");
                if (GUI.Button(RightAlign(footerRect, threeButton), "+")) {
                    if (undoContext != null) Undo.RecordObject(undoContext, "Added list item");
                    list.Add(default);
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}