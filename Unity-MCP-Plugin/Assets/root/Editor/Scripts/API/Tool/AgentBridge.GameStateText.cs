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
            "game-state-text",
            Title = "Agent Bridge / Game State Text",
            ReadOnlyHint = true,
            IdempotentHint = true
        )]
        [Description("Captures the current game state and returns it as structured data. " +
            "Includes scene hierarchy, transform positions, physics state (Rigidbody velocities), " +
            "collider bounds, and component summaries. Use 'json' format for full hierarchy " +
            "or 'text' format for a compact summary. " +
            "Requires com.nyosegawa.agent-bridge package installed in the Unity project.")]
        public ResponseCallTool GameStateText
        (
            [Description("Output format: 'json' for full hierarchical state, 'text' for compact summary.")]
            string format = "json",
            [Description("Maximum hierarchy depth to traverse (json format only).")]
            int maxDepth = 3,
            [Description("Include inactive GameObjects (json format only).")]
            bool includeInactive = false
        )
        {
            var bridgeType = ResolveAgentBridgeType();
            if (bridgeType == null)
                return AgentBridgeNotInstalled();

            return MainThread.Instance.Run(() =>
            {
                string? result;

                if (format == "text")
                {
                    result = InvokeStatic(bridgeType, "CaptureGameStateText") as string;
                }
                else
                {
                    result = InvokeStatic(bridgeType, "CaptureGameStateJson",
                        maxDepth, includeInactive) as string;
                }

                if (string.IsNullOrEmpty(result))
                    return ResponseCallTool.Error("[Error] Failed to capture game state.");

                return ResponseCallTool.Text($"[Success] Game state ({format}):\n{result}");
            });
        }
    }
}
