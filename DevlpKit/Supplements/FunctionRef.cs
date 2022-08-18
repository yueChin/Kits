
namespace Kits.DevlpKit.Supplements 
{

    //public delegate TResult Func<in T1,in T2,in T3,in T4,in T5,out TResult>( T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5 );
    //public delegate TResult Func<in T1,in T2,in T3,in T4,in T5,in T6,out TResult>( T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6 );
    //public delegate TResult Func<in T1,in T2,in T3,in T4,in T5,in T6,in T7,out TResult>( T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6, T7 obj7 );
    //public delegate TResult Func<in T1,in T2,in T3,in T4,in T5,in T6,in T7,in T8,out TResult>( T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6, T7 obj7, T8 obj8 );

    public delegate TResult FuncRef<T, out TResult>( ref T obj1 );
    public delegate TResult FuncRef<T1, T2,out TResult>( ref T1 obj1, ref T2 obj2 );
    public delegate TResult FuncRef<T1, T2, T3,out TResult>( ref T1 obj1, ref T2 obj2, ref T3 obj3 );
    public delegate TResult FuncRef<T1, T2, T3, T4,out TResult>( ref T1 obj1, ref T2 obj2, ref T3 obj3, ref T4 obj4 );
    public delegate TResult FuncRef<T1, T2, T3, T4, T5,out TResult>( ref T1 obj1, ref T2 obj2, ref T3 obj3, ref T4 obj4, ref T5 obj5 );
    public delegate TResult FuncRef<T1, T2, T3, T4, T5, T6,out TResult>( ref T1 obj1, ref T2 obj2, ref T3 obj3, ref T4 obj4, ref T5 obj5, ref T6 obj6 );
    public delegate TResult FuncRef<T1, T2, T3, T4, T5, T6, T7,out TResult>( ref T1 obj1, ref T2 obj2, ref T3 obj3, ref T4 obj4, ref T5 obj5, ref T6 obj6, ref T7 obj7 );
    public delegate TResult FuncRef<T1, T2, T3, T4, T5, T6, T7, T8,out TResult>( ref T1 obj1, ref T2 obj2, ref T3 obj3, ref T4 obj4, ref T5 obj5, ref T6 obj6, ref T7 obj7, ref T8 obj8 );
}
//EOF
