using Colossal.Logging;
using System.Diagnostics;
using System.Reflection;

namespace Extra.Lib.Debugger;

public class Logger(ILog log, bool debugMod = false)
{
	public ILog logger = log;
    public bool debugMod = debugMod;

    public void Info(object LogMessage) {
		#if DEBUG
			MethodBase caller = new StackFrame(1, false).GetMethod();
			UnityEngine.Debug.Log($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
		#else
		if(debugMod)
		{
            MethodBase caller = new StackFrame(1, false).GetMethod();
            UnityEngine.Debug.Log($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
        }
		logger.Info(LogMessage);
		#endif
	}

	public void Warn(object LogMessage) {
		#if DEBUG
			MethodBase caller = new StackFrame(1, false).GetMethod();
			UnityEngine.Debug.LogWarning($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
		#else
		if(debugMod)
		{
            MethodBase caller = new StackFrame(1, false).GetMethod();
            UnityEngine.Debug.LogWarning($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
        }
		logger.Warn(LogMessage);
		#endif
	}

	public void Error(object LogMessage) {
		#if DEBUG
			MethodBase caller = new StackFrame(1, false).GetMethod();
			UnityEngine.Debug.LogError($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
		#else
		if(debugMod)
		{
            MethodBase caller = new StackFrame(1, false).GetMethod();
            UnityEngine.Debug.LogError($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
        }
		logger.Error(LogMessage);
		#endif
	}

	public void Critical(object LogMessage)
	{
#if DEBUG
			MethodBase caller = new StackFrame(1, false).GetMethod();
			UnityEngine.Debug.LogError($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
#else
        if (debugMod)
        {
            MethodBase caller = new StackFrame(1, false).GetMethod();
            UnityEngine.Debug.LogError($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
        }
        logger.Critical(LogMessage);
#endif
    }
    public void Fatal(object LogMessage)
    {
#if DEBUG
			MethodBase caller = new StackFrame(1, false).GetMethod();
			UnityEngine.Debug.LogError($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
#else
        if (debugMod)
        {
            MethodBase caller = new StackFrame(1, false).GetMethod();
            UnityEngine.Debug.LogError($"[{caller.DeclaringType} : {caller.Name}] {LogMessage}");
        }
        logger.Fatal(LogMessage);
#endif
    }
}