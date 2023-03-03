using System;
using System.Collections.Generic;
using System.Reflection;
#if (UNITY_IOS || UNITY_WII || UNITY_PS3 || UNITY_PS4 || UNITY_XBOXONE)
// (see the 'AOT platform' comment in vp_Event.cs for info on this)
#define AOT
#endif

namespace Game.Scripts
{
    public class LT_Value<V> : LT_Event
    {
#if (!AOT)
        protected static T Empty<T>()
        {
            return default(T);
        }

        protected static void Empty<T>(T value)
        {
        }
#endif

        public delegate T Getter<T>();

        public delegate void Setter<T>(T o);

        public Getter<V> Get;
        public Setter<V> Set;

        private FieldInfo[] Fields => FieldInfos;

        public LT_Value(string name) : base(name)
        {
            InitFields();
        }

        protected sealed override void InitFields()
        {
            FieldInfos = new FieldInfo[]
            {
                Type.GetField("Get"),
                Type.GetField("Set")
            };

            StoreInvokerFieldNames();

            _DelegateTypes = new Type[]
            {
                typeof(vp_Value<>.Getter<>),
                typeof(vp_Value<>.Setter<>)
            };

#if (!AOT)
            _DefaultMethods = new MethodInfo[]
            {
                GetStaticGenericMethodInfo(Type, "Empty", typeof(void), _ArgumentType),
                GetStaticGenericMethodInfo(Type, "Empty", _ArgumentType, typeof(void))
            };
#endif

            Prefixes = new Dictionary<string, int>() { { "Get_OnValue_", 0 }, { "Set_OnValue_", 1 } };
            if (_DefaultMethods != null)
                SetFieldToLocalMethod(FieldInfos[0], _DefaultMethods[0], MakeGenericType(_DelegateTypes[0]));
            if ((_DefaultMethods != null) && (_DefaultMethods[1] != null))
                SetFieldToLocalMethod(FieldInfos[1], _DefaultMethods[1], MakeGenericType(_DelegateTypes[1]));

            throw new System.NotImplementedException();
        }

        public override void Register(object target, string method, int var)
        {
            if (method == null)
                return;

            SetFieldToExternalMethod(target, FieldInfos[var], method, MakeGenericType(_DelegateTypes[var]));
            Refresh();
        }

        public override void UnRegister(object target)
        {
            if ((_DefaultMethods != null) && (_DefaultMethods[0] != null))
                SetFieldToLocalMethod(FieldInfos[0], _DefaultMethods[0], MakeGenericType(_DelegateTypes[0]));

            if ((_DefaultMethods != null) && (_DefaultMethods[1] != null))
                SetFieldToLocalMethod(FieldInfos[1], _DefaultMethods[1], MakeGenericType(_DelegateTypes[1]));

            Refresh();
        }
    }
}