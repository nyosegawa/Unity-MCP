/*
┌──────────────────────────────────────────────────────────────────┐
│  Repository: GitHub (https://github.com/nyosegawa/Unity-MCP)     │
│  Copyright (c) 2026 Sakasegawa                                   │
│  Licensed under the Apache License, Version 2.0.                 │
│  See the LICENSE file in the project root for more information.  │
└──────────────────────────────────────────────────────────────────┘
*/

#nullable enable
using System.ComponentModel;
using com.IvanMurzak.McpPlugin;
using com.IvanMurzak.McpPlugin.Common.Model;
using com.IvanMurzak.ReflectorNet.Utils;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    public partial class Tool_AgentBridge
    {
        [McpPluginTool
        (
            "playtest-observe",
            Title = "Agent Bridge / Playtest Observe",
            ReadOnlyHint = true,
            IdempotentHint = true
        )]
        [Description("Retrieves the status and results of a playtest session. " +
            "Returns session state, actions executed, game state text, and optionally a screenshot. " +
            "If sessionId is empty, returns the most recent session. " +
            "Requires com.nyosegawa.agent-bridge package.")]
        public ResponseCallTool PlaytestObserve
        (
            [Description("Session ID from playtest-run. Leave empty for the most recent session.")]
            string sessionId = "",
            [Description("Include the screenshot in the response (if captured).")]
            bool includeScreenshot = true
        )
        {
            var bridgeType = ResolveAgentBridgeType();
            if (bridgeType == null)
                return AgentBridgeNotInstalled();

            return MainThread.Instance.Run(() =>
            {
                var resultJson = InvokeStatic(bridgeType, "GetPlaytestResult", sessionId) as string;

                if (string.IsNullOrEmpty(resultJson))
                    return ResponseCallTool.Error("[Error] No playtest session found.");

                if (includeScreenshot)
                {
                    var pngBytes = InvokeStatic(bridgeType, "GetPlaytestScreenshot", sessionId) as byte[];
                    if (pngBytes != null && pngBytes.Length > 0)
                    {
                        return ResponseCallTool.Image(pngBytes, McpPlugin.Common.Consts.MimeType.ImagePng,
                            $"[Success] Playtest result:\n{resultJson}");
                    }
                }

                return ResponseCallTool.Text($"[Success] Playtest result:\n{resultJson}");
            });
        }
    }
}
