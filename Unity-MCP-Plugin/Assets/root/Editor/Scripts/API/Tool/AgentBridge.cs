/*
┌──────────────────────────────────────────────────────────────────┐
│  Repository: GitHub (https://github.com/nyosegawa/Unity-MCP)     │
│  Copyright (c) 2026 Sakasegawa                                   │
│  Licensed under the Apache License, Version 2.0.                 │
│  See the LICENSE file in the project root for more information.  │
└──────────────────────────────────────────────────────────────────┘
*/

#nullable enable
using System;
using System.Reflection;
using com.IvanMurzak.McpPlugin;
using com.IvanMurzak.McpPlugin.Common.Model;

namespace com.IvanMurzak.Unity.MCP.Editor.API
{
    [McpPluginToolType]
    public partial class Tool_AgentBridge
    {
        private const string AgentBridgeTypeName = "Nyosegawa.AgentBridge.AgentBridge, com.nyosegawa.agent-bridge";

        private static Type? ResolveAgentBridgeType()
        {
            return Type.GetType(AgentBridgeTypeName);
        }

        private static ResponseCallTool AgentBridgeNotInstalled()
        {
            return ResponseCallTool.Error(
                "[Error] com.nyosegawa.agent-bridge package is not installed in the Unity project. " +
                "Install it via Package Manager or add to Packages/manifest.json.");
        }

        private static object? InvokeStatic(Type type, string methodName, params object[] args)
        {
            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
            if (method == null)
                throw new MissingMethodException(type.FullName, methodName);
            return method.Invoke(null, args);
        }
    }
}
