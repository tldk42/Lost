using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Game.Scripts
{
    public abstract class LT_Event
    {
        protected string _Name;
        public string EventName => _Name;

        protected Type _Type;
        public Type Type => _Type;

        protected Type _ArgumentType;
        public Type ArgumentType => _ArgumentType;

        protected Type _ReturnType;
        public Type ReturnType => _ReturnType;


        protected FieldInfo[] FieldInfos;
        protected Type[] _DelegateTypes;
        protected MethodInfo[] _DefaultMethods;

        public string[] InvokerFieldNames;
        public Dictionary<string, int> Prefixes;

        public abstract void Register(object target, string method, int var);
        public abstract void UnRegister(object target);

        protected abstract void InitFields();

        public LT_Event(string name = null)
        {
            _Name = name;
            _Type = GetType();
            _ArgumentType = GetArgumentType;
            _ReturnType = GetGenericReturnType;
        }

        protected void StoreInvokerFieldNames()
        {
            InvokerFieldNames = new string[FieldInfos.Length];
            for (var v = 0; v < FieldInfos.Length; ++v)
            {
                InvokerFieldNames[v] = FieldInfos[v].Name;
            }
        }

        protected Type MakeGenericType(Type type)
        {
            if (ReturnType == typeof(void))
                return type.MakeGenericType(ArgumentType, ArgumentType);
            return type.MakeGenericType(ArgumentType, ReturnType, ArgumentType, ReturnType);
        }

        protected void SetFieldToExternalMethod(object target, FieldInfo fieldInfo, string method, Type type)
        {
            Delegate assignment = Delegate.CreateDelegate(type, target, method, false, false);

            if (assignment == null)
            {
                Debug.LogError(this + "FAILED TO BIND : " + target + " -> " + method);
                return;
            }

            fieldInfo.SetValue(this, assignment);
        }

        protected void SetFieldToLocalMethod(FieldInfo fieldInfo, MethodInfo methodInfo, Type type)
        {
            if (methodInfo == null)
                return;

            Delegate assignment = Delegate.CreateDelegate(type, methodInfo);

            fieldInfo.SetValue(this, assignment);
        }

        protected void RemoveExternalMethodFromField(object target, FieldInfo fieldInfo)
        {
            List<Delegate> assignment = new List<Delegate>(((Delegate)fieldInfo.GetValue(this)).GetInvocationList());

            for (int v = assignment.Count - 1; v > -1; v--)
            {
                if (assignment[v].Target == target)
                    assignment.Remove(assignment[v]);
            }

            fieldInfo.SetValue(this, Delegate.Combine(assignment.ToArray()));
        }

        protected MethodInfo GetStaticGenericMethodInfo(Type type, string name, Type parameterType, Type returnType)
        {
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                                   BindingFlags.FlattenHierarchy))
            {
                if (method.Name != name)
                    continue;

                MethodInfo methodInfo;

                if (GetGenericReturnType == typeof(void))
                    methodInfo = method.MakeGenericMethod(ArgumentType);
                else

                    methodInfo = method.MakeGenericMethod(new Type[] { _ArgumentType, _ReturnType });

                switch (methodInfo.GetParameters().Length)
                {
                    case > 1:
                    case 1 when (parameterType == typeof(void)):
                    case 0 when (parameterType != typeof(void)):
                    case 1 when (methodInfo.GetParameters()[0].ParameterType != parameterType):
                        continue;
                }

                if (returnType != methodInfo.ReturnType)
                    continue;
            }

            return null;
        }

        private Type GetArgumentType => !Type.IsGenericType ? typeof(void) : Type.GetGenericArguments()[0];

        private Type GetGenericReturnType
        {
            get
            {
                if (!Type.IsGenericType)
                    return typeof(void);

                if (Type.GetGenericArguments().Length != 2)
                    return typeof(void);

                return Type.GetGenericArguments()[1];
            }
        }

        public Type GetParameterType(int index)
        {
            if (!Type.IsGenericType)
                return typeof(void);

            if (index > FieldInfos.Length - 1)
                Debug.LogError("Error: (" + this + ") Event '" + EventName + "' only supports " + FieldInfos.Length +
                               " indices. 'GetParameterType' referenced index " + index + ".");

            if (_DelegateTypes[index].GetMethod("Invoke")!.GetParameters().Length == 0)
                return typeof(void);

            return _ArgumentType;
        }

        public Type GetReturnType(int index)
        {
            if (index > FieldInfos.Length - 1)
            {
                Debug.LogError("Error: (" + this + ") Event '" + EventName + "' only supports " + FieldInfos.Length +
                               " indices. 'GetReturnType' referenced index " + index + ".");
                return null;
            }

            if (Type.GetGenericArguments().Length > 1)
                return GetGenericReturnType;

            Type t = _DelegateTypes[index].GetMethod("Invoke")!.ReturnType;
            if (t.IsGenericType)
                return _ArgumentType;

            return t;
        }

        protected void Refresh()
        {
#if UNITY_EDITOR
            vp_EventHandler.RefreshEditor = true;
#endif
        }
    }
}