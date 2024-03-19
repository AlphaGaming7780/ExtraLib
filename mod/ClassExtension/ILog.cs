// using System.Diagnostics;
// using System.Reflection;
// using Colossal.Logging;

// namespace Extra.Lib;

// public static class ILogExtension
// {
// 	public static void PrintInfo(this ILog log, object obj)
// 	{
// 		#if DEBUG
// 			MethodBase caller = new StackFrame(1, false).GetMethod();
// 			UnityEngine.Debug.Log($"[{caller.DeclaringType} : {caller.Name}] {obj}");
// 		#else
// 			if(true) {
// 				MethodBase caller = new StackFrame(1, false).GetMethod();
// 				UnityEngine.Debug.Log($"[{caller.DeclaringType} : {caller.Name}] {obj}");
// 				log.Info(obj);
// 			} else {
// 				log.Info(obj);
// 			}
// 		#endif
// 	}
// }