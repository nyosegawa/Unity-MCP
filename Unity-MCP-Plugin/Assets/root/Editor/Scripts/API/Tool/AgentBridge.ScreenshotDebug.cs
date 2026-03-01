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
            "screenshot-debug",
            Title = "Agent Bridge / Screenshot Debug",
            ReadOnlyHint = true,
            IdempotentHint = true
        )]
        [Description("Captures a screenshot from the main camera with debug overlays " +
            "(collider bounds, velocity vectors) rendered into the Game View. " +
            "Requires com.nyosegawa.agent-bridge package installed in the Unity project.")]
        public ResponseCallTool ScreenshotDebug
        (
            [Description("Width of the screenshot in pixels.")]
            int width = 1920,
            [Description("Height of the screenshot in pixels.")]
            int height = 1080,
            [Description("Draw collider bounds as wireframes.")]
            bool showColliders = true,
            [Description("Draw velocity vectors for Rigidbody objects.")]
            bool showVelocities = true
        )
        {
            var bridgeType = ResolveAgentBridgeType();
            if (bridgeType == null)
                return AgentBridgeNotInstalled();

            return MainThread.Instance.Run(() =>
            {
                var pngBytes = InvokeStatic(bridgeType, "CaptureDebugScreenshot",
                    width, height, showColliders, showVelocities) as byte[];

                if (pngBytes == null || pngBytes.Length == 0)
                    return ResponseCallTool.Error("[Error] Failed to capture debug screenshot. Check that a camera exists in the scene.");

                return ResponseCallTool.Image(pngBytes, McpPlugin.Common.Consts.MimeType.ImagePng,
                    $"Debug screenshot ({width}x{height}) colliders={showColliders} velocities={showVelocities}");
            });
        }
    }
}
