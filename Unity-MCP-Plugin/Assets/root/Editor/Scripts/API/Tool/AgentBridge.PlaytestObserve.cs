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
        [Description("Retrieves the status and results of a running or completed playtest. " +
            "Returns the current state, captured screenshots, game state snapshots, and any errors. " +
            "Requires com.nyosegawa.agent-bridge package with PlaytestRunner component. " +
            "NOTE: This tool is a stub — PlaytestRunner will be implemented in Phase 4.")]
        public ResponseCallTool PlaytestObserve
        (
            [Description("ID of the playtest session to observe. If empty, returns the most recent session.")]
            string sessionId = ""
        )
        {
            var bridgeType = ResolveAgentBridgeType();
            if (bridgeType == null)
                return AgentBridgeNotInstalled();

            return ResponseCallTool.Error(
                "[Error] playtest-observe is not yet implemented. " +
                "PlaytestRunner will be added to com.nyosegawa.agent-bridge in Phase 4. " +
                "For now, use 'screenshot-debug' and 'game-state-text' to observe the current state.");
        }
    }
}
