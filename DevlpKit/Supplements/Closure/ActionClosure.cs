//#define CHECK_CLOSURE

using System;

namespace Kits.DevlpKit.Supplements.Closure
{
    public struct ActionClosure
    {
        Closure _closure;
        Action<Closure> _wrapper;

        public void Reset()
        {
            _wrapper = null;
            _closure.Reset();
        }

        public void Invoke()
        {
            if (_wrapper != null)
            {
                _wrapper(_closure);
            }
        }

        public static ActionClosure Create(Action action)
        {
            Closure.Check(action);
            return new ActionClosure
            {
                _closure = new Closure { _delegate = action },
                _wrapper = e => e.Invoke()
            };
        }

        public static ActionClosure Create<T>(Action<T> action, T ctx)
        {
            Closure.Check(action);
            return new ActionClosure
            {
                _closure = new Closure
                {
                    _0 = ValueStruct.Writer<T>.invoke(ctx),
                    _delegate = action
                },
                _wrapper = ActionClosureWrapper<T>._default
            };
        }

        public static ActionClosure Create<T0, T1>(Action<T0, T1> action, T0 ctx0, T1 ctx1)
        {
            Closure.Check(action);
            return new ActionClosure
            {
                _closure = new Closure
                {
                    _0 = ValueStruct.Writer<T0>.invoke(ctx0),
                    _1 = ValueStruct.Writer<T1>.invoke(ctx1),
                    _delegate = action
                },
                _wrapper = ActionClosureWrapper<T0, T1>._default
            };
        }

        public static ActionClosure Create<T0, T1, T2>(Action<T0, T1, T2> action, T0 ctx0, T1 ctx1, T2 ctx2)
        {
            Closure.Check(action);
            return new ActionClosure
            {
                _closure = new Closure
                {
                    _0 = ValueStruct.Writer<T0>.invoke(ctx0),
                    _1 = ValueStruct.Writer<T1>.invoke(ctx1),
                    _2 = ValueStruct.Writer<T2>.invoke(ctx2),
                    _delegate = action
                },
                _wrapper = ActionClosureWrapper<T0, T1, T2>._default
            };
        }

        public static ActionClosure Create<T0, T1, T2, T3>(Action<T0, T1, T2, T3> action, T0 ctx0, T1 ctx1, T2 ctx2, T3 ctx3)
        {
            Closure.Check(action);
            return new ActionClosure
            {
                _closure = new Closure
                {
                    _0 = ValueStruct.Writer<T0>.invoke(ctx0),
                    _1 = ValueStruct.Writer<T1>.invoke(ctx1),
                    _2 = ValueStruct.Writer<T2>.invoke(ctx2),
                    _3 = ValueStruct.Writer<T3>.invoke(ctx3),
                    _delegate = action
                },
                _wrapper = ActionClosureWrapper<T0, T1, T2, T3>._default
            };
        }
    }

    internal class ActionClosureWrapper<T>
    {
        internal static Action<Closure> _default = e => e.Invoke<T>();
    }

    internal class ActionClosureWrapper<T0, T1>
    {
        internal static Action<Closure> _default = e => e.Invoke<T0, T1>();
    }

    internal class ActionClosureWrapper<T0, T1, T2>
    {
        internal static Action<Closure> _default = e => e.Invoke<T0, T1, T2>();
    }

    internal class ActionClosureWrapper<T0, T1, T2, T3>
    {
        internal static Action<Closure> _default = e => e.Invoke<T0, T1, T2, T3>();
    }
}
//EOF
