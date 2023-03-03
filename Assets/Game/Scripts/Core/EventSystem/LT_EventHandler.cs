using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Game.Scripts
{
    public abstract class LT_EventHandler : MonoBehaviour
    {
        protected sealed class ScriptMethods
        {
            public List<MethodInfo> Events = new List<MethodInfo>();

            public ScriptMethods(Type type)
            {
                Events = GetMethods(type);
            }

            private static List<MethodInfo> GetMethods(Type type)
            {
                List<MethodInfo> methodInfos = new List<MethodInfo>();
                List<string> existingMethodNames = new List<string>();

                while (type != null)
                {
                    foreach (var methodInfo in type.GetMethods((BindingFlags.Public | BindingFlags.NonPublic |
                                                                BindingFlags.Instance | BindingFlags.DeclaredOnly)))
                    {
                        if (methodInfo.Name.Contains("m__"))
                            continue;

                        if (existingMethodNames.Contains(methodInfo.Name))
                            continue;

                        foreach (var prefix in _SupportedPrefixes)
                        {
                            if (methodInfo.Name.Contains(prefix))
                                goto FoundMethodWithSupportedPrefix;
                        }

                        continue;

                        FoundMethodWithSupportedPrefix:
                        methodInfos.Add(methodInfo);
                        existingMethodNames.Add(methodInfo.Name);
                    }

                    type = type.BaseType;
                }

                return methodInfos;
            }
        }

#if UNITY_EDITOR
        public static bool RefreshEditor = false;
#endif

        protected bool _Initialized = false;
        protected Dictionary<string, LT_Event> _EventsByCallback = new Dictionary<string, LT_Event>();
        protected List<LT_Event> _Events = new List<LT_Event>();

        // register 대기중인 핸들러 (깨어나기 전) -> 다시 방문 필요함
        protected List<object> _PendingRegistrants = new List<object>();

        protected static Dictionary<Type, ScriptMethods> _StoredScriptMethodTypes =
            new Dictionary<Type, ScriptMethods>();

        protected static string[] _SupportedPrefixes = new string[]
        {
            "OnMessage_", "CanStart_", "CanStop_", "OnStart_", "OnStop_", "OnAttempt_", "get_OnValue_", "set_OnValue_",
            "OnFailStart_", "OnFailStop_"
        };

        protected virtual void Awake()
        {
            StoreHandlerEvents();

            _Initialized = true;

            for (var i = _PendingRegistrants.Count - 1; i > -1; --i)
            {
                // Register(_PendingRegistrants[i]);
                _PendingRegistrants.Remove(_PendingRegistrants[i]);
            }
        }

        protected void StoreHandlerEvents()
        {
            object o = null;

            List<FieldInfo> info = GetFields();
            if (info == null || info.Count == 0)
                return;

            foreach (FieldInfo i in info)
            {
                try
                {
                    o = Activator.CreateInstance(i.FieldType, i.Name);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error: (" + this + ") does not support the type of '" + i.Name + "' in '" +
                                   i.DeclaringType + "'.");
                    throw;
                }

                if (o == null)
                    continue;

                i.SetValue(this, o);

                if (!_Events.Contains((LT_Event)o))
                    _Events.Add((LT_Event)o);


                foreach (string prefixesKey in ((LT_Event)o).Prefixes.Keys)
                {
                    _EventsByCallback.Add(prefixesKey + i.Name, (LT_Event)o);
                }
            }
        }
        
        public List<FieldInfo> GetFields()
        {

            List<FieldInfo> info = new List<FieldInfo>();
            Type currentType = this.GetType();
            Type nextType = null;
            do
            {
                if (nextType != null)
                    currentType = nextType;
                info.AddRange(currentType.GetFields((BindingFlags.Public | BindingFlags.NonPublic |
                                                     BindingFlags.Instance | BindingFlags.DeclaredOnly)));
                if (currentType.BaseType != typeof(vp_StateEventHandler) &&
                    currentType.BaseType != typeof(LT_EventHandler))
                    nextType = currentType.BaseType;
            }
            while (currentType.BaseType != typeof(vp_StateEventHandler) &&
                   currentType.BaseType != typeof(LT_EventHandler) &&
                   currentType != null);

            if(info == null || info.Count == 0)
                Debug.LogWarning("Warning: (" + this + ") Found no fields to store as events.");
		
            return info;

        }
    }
}