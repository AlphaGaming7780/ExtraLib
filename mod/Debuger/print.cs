using System.Diagnostics;
using System.Reflection;

namespace ExtraLib.Debugger;

internal static class Print
{
	public static void Info(object LogMessage) {
		#if DEBUG
			MethodBase caller = new StackFrame(1, false).GetMethod();
			UnityEngine.Debug.Log($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
		#else
			Mod.Logger.Info(LogMessage);
		#endif
	}

	public static void Warn(object LogMessage) {
		#if DEBUG
			MethodBase caller = new StackFrame(1, false).GetMethod();
			UnityEngine.Debug.LogWarning($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
		#else
			Mod.Logger.Warn(LogMessage);
		#endif
	}

	public static void Error(object LogMessage) {
		#if DEBUG
			MethodBase caller = new StackFrame(1, false).GetMethod();
			UnityEngine.Debug.LogError($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
		#else
			Mod.Logger.Error(LogMessage);
		#endif
	}	
}