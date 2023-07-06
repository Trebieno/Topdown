using System;
using UnityEngine;
using Conditional = System.Diagnostics.ConditionalAttribute;

namespace Mirror.SimpleWeb
{
    public static class Log
    {
        // used for Conditional
        const string SIMPLEWEB_LOG_ENABLED = nameof(SIMPLEWEB_LOG_ENABLED);
        const string DEBUG = nameof(DEBUG);

        public enum Levels
        {
            none = 0,
            error = 1,
            warn = 2,
            info = 3,
            verbose = 4,
        }

        public static ILogger logger = Debug.unityLogger;
        public static Levels level = Levels.none;

        public static string BufferToString(byte[] buffer, int offset = 0, int? length = null)
        {
            return BitConverter.ToString(buffer, offset, length ?? buffer.Length);
        }

        [Conditional(SIMPLEWEB_LOG_ENABLED)]
        public static void DumpBuffer(string label, byte[] buffer, int offset, int length)
        {
            if (level < Levels.verbose)
                return;

            logger.Log(LogType.Log, $"[SimpleWebTransport] VERBOSE: <color=cyan>{label}: {BufferToString(buffer, offset, length)}</color>");
        }

        [Conditional(SIMPLEWEB_LOG_ENABLED)]
        public static void DumpBuffer(string label, ArrayBuffer arrayBuffer)
        {
            if (level < Levels.verbose)
                return;

            logger.Log(LogType.Log, $"[SimpleWebTransport] VERBOSE: <color=cyan>{label}: {BufferToString(arrayBuffer.array, 0, arrayBuffer.count)}</color>");
        }

        [Conditional(SIMPLEWEB_LOG_ENABLED)]
        public static void Verbose(string msg, bool showColor = true)
        {
            if (level < Levels.verbose)
                return;

            if (showColor)
                logger.Log(LogType.Log, $"[SimpleWebTransport] VERBOSE: <color=cyan>{msg}</color>");
            else
                logger.Log(LogType.Log, $"[SimpleWebTransport] VERBOSE: {msg}");
        }

        [Conditional(SIMPLEWEB_LOG_ENABLED)]
        public static void Info(string msg, bool showColor = true)
        {
            if (level < Levels.info)
                return;

            if (showColor)
                logger.Log(LogType.Log, $"[SimpleWebTransport] INFO: <color=cyan>{msg}</color>");
            else
                logger.Log(LogType.Log, $"[SimpleWebTransport] INFO: {msg}");
        }

        /// <summary>
        /// An expected Exception was caught, useful for debugging but not important
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="showColor"></param>
        [Conditional(SIMPLEWEB_LOG_ENABLED)]
        public static void InfoException(Exception e)
        {
            if (level < Levels.info)
                return;

#if UNITY_SERVER
            logger.Log(LogType.Log, $"[SimpleWebTransport] INFO_EXCEPTION: {e.GetType().Name} Message: {e.Message}\n{e.StackTrace}\n\n");
#else
            logger.Log(LogType.Log, $"[SimpleWebTransport] INFO_EXCEPTION: <color=cyan>{e.GetType().Name}</color> Message: {e.Message}\n{e.StackTrace}\n\n");
#endif
        }

        [Conditional(SIMPLEWEB_LOG_ENABLED), Conditional(DEBUG)]
        public static void Warn(string msg, bool showColor = true)
        {
            if (level < Levels.warn)
                return;

            if (showColor)
                logger.Log(LogType.Warning, $"[SimpleWebTransport] WARN: <color=orange>{msg}</color>");
            else
                logger.Log(LogType.Warning, $"[SimpleWebTransport] WARN: {msg}");
        }

        [Conditional(SIMPLEWEB_LOG_ENABLED), Conditional(DEBUG)]
        public static void Error(string msg, bool showColor = true)
        {
            if (level < Levels.error)
                return;

            if (showColor)
                logger.Log(LogType.Error, $"[SimpleWebTransport] ERROR: <color=red>{msg}</color>");
            else
                logger.Log(LogType.Error, $"[SimpleWebTransport] ERROR: {msg}");
        }

        public static void Exception(Exception e)
        {
            // always log Exceptions
#if UNITY_SERVER
            logger.Log(LogType.Error, $"[SimpleWebTransport] EXCEPTION: {e.GetType().Name} Message: {e.Message}\n{e.StackTrace}\n\n");
#else
            logger.Log(LogType.Error, $"[SimpleWebTransport] EXCEPTION: <color=red>{e.GetType().Name}</color> Message: {e.Message}\n{e.StackTrace}\n\n");
#endif
        }
    }
}
