//#define CHECK_CLOSURE

using System;

namespace Kits.DevlpKit.Supplements.Closure
{
    public struct FuncClosure
    {

        Closure _context;
        Func<Closure, ValueStruct> _wrapper;

        public void Reset()
        {
            _wrapper = null;
            _context.Reset();
        }

        public T Invoke<T>()
        {
            if (_wrapper != null)
            {
                ValueStruct s = _wrapper(_context);
                return ValueStruct.Reader<T>.invoke(ref s);
            }
            return default(T);
        }

        public void Invoke()
        {
            if (_wrapper != null)
            {
                _wrapper(_context);
            }
        }

        public static FuncClosure Create<TResult>(Func<TResult> func)
        {
            Closure.Check(func);
            return new FuncClosure
            {
                _context = new Closure
                {
                    _delegate = func
                },
                _wrapper = FuncClosureWrapper<TResult>._default
            };
        }

        public static FuncClosure Create<T, TResult>(Func<T, TResult> func, T ctx)
        {
            Closure.Check(func);
            return new FuncClosure
            {
                _context = new Closure
                {
                    _0 = ValueStruct.Writer<T>.invoke(ctx),
                    _delegate = func
                },
                _wrapper = FuncClosureWrapper<T, TResult>._default
            };
        }

        public static FuncClosure Create<T0, T1, TResult>(Func<T0, T1, TResult> func, T0 ctx0, T1 ctx1)
        {
            Closure.Check(func);
            return new FuncClosure
            {
                _context = new Closure
                {
                    _0 = ValueStruct.Writer<T0>.invoke(ctx0),
                    _1 = ValueStruct.Writer<T1>.invoke(ctx1),
                    _delegate = func
                },
                _wrapper = FuncClosureWrapper<T0, T1, TResult>._default
            };
        }

        public static FuncClosure Create<T0, T1, T2, TResult>(Func<T0, T1, T2, TResult> func, T0 ctx0, T1 ctx1, T2 ctx2)
        {
            Closure.Check(func);
            return new FuncClosure
            {
                _context = new Closure
                {
                    _0 = ValueStruct.Writer<T0>.invoke(ctx0),
                    _1 = ValueStruct.Writer<T1>.invoke(ctx1),
                    _2 = ValueStruct.Writer<T2>.invoke(ctx2),
                    _delegate = func
                },
                _wrapper = FuncClosureWrapper<T0, T1, T2, TResult>._default
            };
        }

        public static FuncClosure Create<T0, T1, T2, T3, TResult>(Func<T0, T1, T2, T3, TResult> func, T0 ctx0, T1 ctx1, T2 ctx2, T3 ctx3)
        {
            Closure.Check(func);
            return new FuncClosure
            {
                _context = new Closure
                {
                    _0 = ValueStruct.Writer<T0>.invoke(ctx0),
                    _1 = ValueStruct.Writer<T1>.invoke(ctx1),
                    _2 = ValueStruct.Writer<T2>.invoke(ctx2),
                    _3 = ValueStruct.Writer<T3>.invoke(ctx3),
                    _delegate = func
                },
                _wrapper = FuncClosureWrapper<T0, T1, T2, T3, TResult>._default
            };
        }

        public static FuncClosure Create(Action func)
        {
            Closure.Check(func);
            return new FuncClosure
            {
                _context = new Closure
                {
                    _delegate = func
                },
                _wrapper = e => { e.Invoke(); return ValueStruct.nil; }
            };
        }

        public static FuncClosure Create<T>(Action<T> func, T ctx)
        {
            Closure.Check(func);
            return new FuncClosure
            {
                _context = new Closure
                {
                    _0 = ValueStruct.Writer<T>.invoke(ctx),
                    _delegate = func
                },
                _wrapper = FuncClosureWrapperVoidResult<T>._default
            };
        }

        public static FuncClosure Create<T0, T1>(Action<T0, T1> func, T0 ctx0, T1 ctx1)
        {
            Closure.Check(func);
            return new FuncClosure
            {
                _context = new Closure
                {
                    _0 = ValueStruct.Writer<T0>.invoke(ctx0),
                    _1 = ValueStruct.Writer<T1>.invoke(ctx1),
                    _delegate = func
                },
                _wrapper = FuncClosureWrapperVoidResult<T0, T1>._default
            };
        }

        public static FuncClosure Create<T0, T1, T2>(Action<T0, T1, T2> func, T0 ctx0, T1 ctx1, T2 ctx2)
        {
            Closure.Check(func);
            return new FuncClosure
            {
                _context = new Closure
                {
                    _0 = ValueStruct.Writer<T0>.invoke(ctx0),
                    _1 = ValueStruct.Writer<T1>.invoke(ctx1),
                    _2 = ValueStruct.Writer<T2>.invoke(ctx2),
                    _delegate = func
                },
                _wrapper = FuncClosureWrapperVoidResult<T0, T1, T2>._default
            };
        }

        public static FuncClosure Create<T0, T1, T2, T3>(Action<T0, T1, T2, T3> func, T0 ctx0, T1 ctx1, T2 ctx2, T3 ctx3)
        {
            Closure.Check(func);
            return new FuncClosure
            {
                _context = new Closure
                {
                    _0 = ValueStruct.Writer<T0>.invoke(ctx0),
                    _1 = ValueStruct.Writer<T1>.invoke(ctx1),
                    _2 = ValueStruct.Writer<T2>.invoke(ctx2),
                    _3 = ValueStruct.Writer<T3>.invoke(ctx3),
                    _delegate = func
                },
                _wrapper = FuncClosureWrapperVoidResult<T0, T1, T2, T3>._default
            };
        }
    }

    internal class FuncClosureWrapper<TResult>
    {
        internal static Func<Closure, ValueStruct> _default = e => e.SRInvoke<TResult>();
    }

    internal class FuncClosureWrapper<T, TResult>
    {
        internal static Func<Closure, ValueStruct> _default = e => e.SRInvoke<T, TResult>();
    }

    internal class FuncClosureWrapper<T0, T1, TResult>
    {
        internal static Func<Closure, ValueStruct> _default = e => e.SRInvoke<T0, T1, TResult>();
    }

    internal class FuncClosureWrapper<T0, T1, T2, TResult>
    {
        internal static Func<Closure, ValueStruct> _default = e => e.SRInvoke<T0, T1, T2, TResult>();
    }

    internal class FuncClosureWrapper<T0, T1, T2, T3, TResult>
    {
        internal static Func<Closure, ValueStruct> _default = e => e.SRInvoke<T0, T1, T2, T3, TResult>();
    }

    internal class FuncClosureWrapperVoidResult<T>
    {
        internal static Func<Closure, ValueStruct> _default = e => { e.Invoke<T>(); return ValueStruct.nil; };
    }

    internal class FuncClosureWrapperVoidResult<T0, T1>
    {
        internal static Func<Closure, ValueStruct> _default = e => { e.Invoke<T0, T1>(); return ValueStruct.nil; };
    }

    internal class FuncClosureWrapperVoidResult<T0, T1, T2>
    {
        internal static Func<Closure, ValueStruct> _default = e => { e.Invoke<T0, T1, T2>(); return ValueStruct.nil; };
    }

    internal class FuncClosureWrapperVoidResult<T0, T1, T2, T3>
    {
        internal static Func<Closure, ValueStruct> _default = e => { e.Invoke<T0, T1, T2, T3>(); return ValueStruct.nil; };
    }
}
//EOF
