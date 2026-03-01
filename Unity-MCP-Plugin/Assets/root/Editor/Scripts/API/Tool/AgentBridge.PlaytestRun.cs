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
            "playtest-run",
            Title = "Agent Bridge / Playtest Run",
            DestructiveHint = false
        )]
        [Description("Executes an input sequence in Play Mode and captures the result. " +
            "The scene must already be in Play Mode (use 'editor-application-set-state' first). " +
            "Runs the specified input actions via InputSimulator, waits for completion, " +
            "then captures a debug screenshot and game state. " +
            "Returns the session ID — use 'playtest-observe' to get results. " +
            "Requires com.nyosegawa.agent-bridge package.")]
        public ResponseCallTool PlaytestRun
        (
            [Description("JSON array of input actions. Format: [{\"action\":\"key_down\",\"key\":\"space\",\"delayMs\":100}, ...]. " +
                "Supported actions: key_down, key_up, wait. " +
                "Keys: space, left, right, up, down, a, d, w, s, escape, return.")]
            string inputSequenceJson,
            [Description("Maximum duration in seconds before timeout.")]
            float timeoutSeconds = 10f,
            [Description("Capture a debug screenshot after the sequence completes.")]
            bool captureScreenshot = true
        )
        {
            var bridgeType = ResolveAgentBridgeType();
            if (bridgeType == null)
                return AgentBridgeNotInstalled();

            return MainThread.Instance.Run(() =>
            {
                var sessionId = InvokeStatic(bridgeType, "RunPlaytest",
                    inputSequenceJson, timeoutSeconds, captureScreenshot) as string;

                if (string.IsNullOrEmpty(sessionId))
                    return ResponseCallTool.Error("[Error] Failed to start playtest session.");

                return ResponseCallTool.Text(
                    $"[Success] Playtest session started. Session ID: {sessionId}. " +
                    "Use 'playtest-observe' to get results after the sequence completes.");
            });
        }
    }
}
