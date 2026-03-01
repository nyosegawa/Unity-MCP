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
            "Enters Play Mode if not already playing, runs the specified input actions, " +
            "waits for completion, then captures a screenshot and game state. " +
            "Requires com.nyosegawa.agent-bridge package with PlaytestRunner component. " +
            "NOTE: This tool is a stub — PlaytestRunner will be implemented in Phase 4.")]
        public ResponseCallTool PlaytestRun
        (
            [Description("JSON-encoded input sequence. Format: [{\"action\":\"key_down\",\"key\":\"space\",\"delay_ms\":100}, ...]. " +
                "Supported actions: key_down, key_up, move_horizontal, move_vertical, wait.")]
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

            return ResponseCallTool.Error(
                "[Error] playtest-run is not yet implemented. " +
                "PlaytestRunner will be added to com.nyosegawa.agent-bridge in Phase 4. " +
                "For now, use 'editor-application-set-state' to enter Play Mode, " +
                "then 'screenshot-debug' and 'game-state-text' to observe results.");
        }
    }
}
