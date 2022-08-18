
namespace Kits.DevlpKit.Supplements 
{
    //public delegate void Action<T1, T2, T3, T4, T5>( T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5 );
    //public delegate void Action<T1, T2, T3, T4, T5, T6>( T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6 );
    //public delegate void Action<T1, T2, T3, T4, T5, T6, T7>( T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6, T7 obj7 );
    //public delegate void Action<T1, T2, T3, T4, T5, T6, T7, T8>( T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6, T7 obj7, T8 obj8 );

    public delegate void ActionRef<T>( ref T obj1 );
    public delegate void ActionRef<T1, T2>( ref T1 obj1, ref T2 obj2 );
    public delegate void ActionRef<T1, T2, T3>( ref T1 obj1, ref T2 obj2, ref T3 obj3 );
    public delegate void ActionRef<T1, T2, T3, T4>( ref T1 obj1, ref T2 obj2, ref T3 obj3, ref T4 obj4 );
    public delegate void ActionRef<T1, T2, T3, T4, T5>( ref T1 obj1, ref T2 obj2, ref T3 obj3, ref T4 obj4, ref T5 obj5 );
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6>( ref T1 obj1, ref T2 obj2, ref T3 obj3, ref T4 obj4, ref T5 obj5, ref T6 obj6 );
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6, T7>( ref T1 obj1, ref T2 obj2, ref T3 obj3, ref T4 obj4, ref T5 obj5, ref T6 obj6, ref T7 obj7 );
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6, T7, T8>( ref T1 obj1, ref T2 obj2, ref T3 obj3, ref T4 obj4, ref T5 obj5, ref T6 obj6, ref T7 obj7, ref T8 obj8 );
  
}
//EOF
