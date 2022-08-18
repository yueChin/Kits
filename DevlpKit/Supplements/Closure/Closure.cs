//#define CHECK_CLOSURE

using System;
using System.Diagnostics;

namespace Kits.DevlpKit.Supplements.Closure
{
    /// <summary>
    /// 结构值类型 避免int/float一类装拆箱
    /// </summary>
    public struct Closure
    {

        public ValueStruct _0;
        public ValueStruct _1;
        public ValueStruct _2;
        public ValueStruct _3;
        public Delegate _delegate;

        public T ctx_0<T>()
        {
            return ValueStruct.Reader<T>.invoke(ref _0);
        }

        public T ctx_1<T>()
        {
            return ValueStruct.Reader<T>.invoke(ref _1);
        }

        public T ctx_2<T>()
        {
            return ValueStruct.Reader<T>.invoke(ref _2);
        }

        public T ctx_3<T>()
        {
            return ValueStruct.Reader<T>.invoke(ref _3);
        }

        public void Reset()
        {
            _delegate = null;
            _0 = ValueStruct.nil;
            _1 = ValueStruct.nil;
            _2 = ValueStruct.nil;
            _3 = ValueStruct.nil;
        }

        public void Invoke()
        {
            (_delegate as Action)();
        }

        public void Invoke<T>()
        {
            (_delegate as Action<T>)(
                ValueStruct.Reader<T>.invoke(ref _0)
            );
        }

        public void Invoke<T0, T1>()
        {
            (_delegate as Action<T0, T1>)(
                ValueStruct.Reader<T0>.invoke(ref _0),
                ValueStruct.Reader<T1>.invoke(ref _1)
            );
        }

        public void Invoke<T0, T1, T2>()
        {
            (_delegate as Action<T0, T1, T2>)(
                ValueStruct.Reader<T0>.invoke(ref _0),
                ValueStruct.Reader<T1>.invoke(ref _1),
                ValueStruct.Reader<T2>.invoke(ref _2)
            );
        }

        public void Invoke<T0, T1, T2, T3>()
        {
            (_delegate as Action<T0, T1, T2, T3>)(
                ValueStruct.Reader<T0>.invoke(ref _0),
                ValueStruct.Reader<T1>.invoke(ref _1),
                ValueStruct.Reader<T2>.invoke(ref _2),
                ValueStruct.Reader<T3>.invoke(ref _3)
            );
        }

        public TResult RInvoke<TResult>()
        {
            return (_delegate as Func<TResult>)();
        }

        public TResult RInvoke<T, TResult>()
        {
            return (_delegate as Func<T, TResult>)(
                ValueStruct.Reader<T>.invoke(ref _0)
            );
        }

        public TResult RInvoke<T0, T1, TResult>()
        {
            return (_delegate as Func<T0, T1, TResult>)(
                ValueStruct.Reader<T0>.invoke(ref _0),
                ValueStruct.Reader<T1>.invoke(ref _1)
            );
        }

        public TResult RInvoke<T0, T1, T2, TResult>()
        {
            return (_delegate as Func<T0, T1, T2, TResult>)(
                ValueStruct.Reader<T0>.invoke(ref _0),
                ValueStruct.Reader<T1>.invoke(ref _1),
                ValueStruct.Reader<T2>.invoke(ref _2)
            );
        }

        public TResult RInvoke<T0, T1, T2, T3, TResult>()
        {
            return (_delegate as Func<T0, T1, T2, T3, TResult>)(
                ValueStruct.Reader<T0>.invoke(ref _0),
                ValueStruct.Reader<T1>.invoke(ref _1),
                ValueStruct.Reader<T2>.invoke(ref _2),
                ValueStruct.Reader<T3>.invoke(ref _3)
            );
        }

        public ValueStruct SRInvoke<TResult>()
        {
            return ValueStruct.Writer<TResult>.invoke(
                (_delegate as Func<TResult>)()
            );
        }

        public ValueStruct SRInvoke<T, TResult>()
        {
            return ValueStruct.Writer<TResult>.invoke(
                (_delegate as Func<T, TResult>)(
                    ValueStruct.Reader<T>.invoke(ref _0)
                )
            );
        }

        public ValueStruct SRInvoke<T0, T1, TResult>()
        {
            return ValueStruct.Writer<TResult>.invoke(
                (_delegate as Func<T0, T1, TResult>)(
                    ValueStruct.Reader<T0>.invoke(ref _0),
                    ValueStruct.Reader<T1>.invoke(ref _1)
                )
            );
        }

        public ValueStruct SRInvoke<T0, T1, T2, TResult>()
        {
            return ValueStruct.Writer<TResult>.invoke(
                (_delegate as Func<T0, T1, T2, TResult>)(
                    ValueStruct.Reader<T0>.invoke(ref _0),
                    ValueStruct.Reader<T1>.invoke(ref _1),
                    ValueStruct.Reader<T2>.invoke(ref _2)
                )
            );
        }

        public ValueStruct SRInvoke<T0, T1, T2, T3, TResult>()
        {
            return ValueStruct.Writer<TResult>.invoke(
                (_delegate as Func<T0, T1, T2, T3, TResult>)(
                    ValueStruct.Reader<T0>.invoke(ref _0),
                    ValueStruct.Reader<T1>.invoke(ref _1),
                    ValueStruct.Reader<T2>.invoke(ref _2),
                    ValueStruct.Reader<T3>.invoke(ref _3)
                )
            );
        }

        [Conditional("CHECK_CLOSURE")]
        public static void Check(object d)
        {
            //Debug.Assert(((Delegate)d).Target == null);
        }
    }

}
//EOF
