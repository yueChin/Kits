using System.Runtime.InteropServices;
using System.Text;

namespace Kits.DevlpKit.Supplements.Closure
{

    internal interface ITupleStruct
    {
        string ToString( StringBuilder sb );
        int Size { get; }
    }

    public static class TupleStruct
    {
        public static TupleStruct<T1> Create<T1>( T1 item1 ) {
            return new TupleStruct<T1>( item1 );
        }

        public static TupleStruct<T1, T2> Create<T1, T2>( T1 item1, T2 item2 ) {
            return new TupleStruct<T1, T2>( item1, item2 );
        }

        public static TupleStruct<T1, T2, T3> Create<T1, T2, T3>( T1 item1, T2 item2, T3 item3 ) {
            return new TupleStruct<T1, T2, T3>( item1, item2, item3 );
        }

        public static TupleStruct<T1, T2, T3, T4> Create<T1, T2, T3, T4>( T1 item1, T2 item2, T3 item3, T4 item4 ) {
            return new TupleStruct<T1, T2, T3, T4>( item1, item2, item3, item4 );
        }

        public static TupleStruct<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>( T1 item1, T2 item2, T3 item3, T4 item4, T5 item5 ) {
            return new TupleStruct<T1, T2, T3, T4, T5>( item1, item2, item3, item4, item5 );
        }

        public static TupleStruct<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>( T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6 ) {
            return new TupleStruct<T1, T2, T3, T4, T5, T6>( item1, item2, item3, item4, item5, item6 );
        }

        public static TupleStruct<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>( T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7 ) {
            return new TupleStruct<T1, T2, T3, T4, T5, T6, T7>( item1, item2, item3, item4, item5, item6, item7 );
        }

        public static TupleStruct<T1, T2, T3, T4, T5, T6, T7, TupleStruct<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>( T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8 ) {
            return new TupleStruct<T1, T2, T3, T4, T5, T6, T7, TupleStruct<T8>>( item1, item2, item3, item4, item5, item6, item7, new TupleStruct<T8>( item8 ) );
        }
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct TupleStruct<T1> : ITupleStruct
    {

        private readonly T1 m_Item1;

        public T1 Item1 { get { return m_Item1; } }

        public TupleStruct( T1 item1 ) {
            m_Item1 = item1;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append( "(" );
            return ( (ITupleStruct)this ).ToString( sb );
        }

        string ITupleStruct.ToString( StringBuilder sb ) {
            sb.Append( m_Item1 );
            sb.Append( ")" );
            return sb.ToString();
        }

        int ITupleStruct.Size {
            get {
                return 1;
            }
        }
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct TupleStruct<T1, T2> : ITupleStruct
    {

        private readonly T1 m_Item1;
        private readonly T2 m_Item2;

        public T1 Item1 { get { return m_Item1; } }
        public T2 Item2 { get { return m_Item2; } }

        public TupleStruct( T1 item1, T2 item2 ) {
            m_Item1 = item1;
            m_Item2 = item2;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append( "(" );
            return ( (ITupleStruct)this ).ToString( sb );
        }

        string ITupleStruct.ToString( StringBuilder sb ) {
            sb.Append( m_Item1 );
            sb.Append( ", " );
            sb.Append( m_Item2 );
            sb.Append( ")" );
            return sb.ToString();
        }

        int ITupleStruct.Size {
            get {
                return 2;
            }
        }
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct TupleStruct<T1, T2, T3> : ITupleStruct
    {

        private readonly T1 m_Item1;
        private readonly T2 m_Item2;
        private readonly T3 m_Item3;

        public T1 Item1 { get { return m_Item1; } }
        public T2 Item2 { get { return m_Item2; } }
        public T3 Item3 { get { return m_Item3; } }

        public TupleStruct( T1 item1, T2 item2, T3 item3 ) {
            m_Item1 = item1;
            m_Item2 = item2;
            m_Item3 = item3;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append( "(" );
            return ( (ITupleStruct)this ).ToString( sb );
        }

        string ITupleStruct.ToString( StringBuilder sb ) {
            sb.Append( m_Item1 );
            sb.Append( ", " );
            sb.Append( m_Item2 );
            sb.Append( ", " );
            sb.Append( m_Item3 );
            sb.Append( ")" );
            return sb.ToString();
        }

        int ITupleStruct.Size {
            get {
                return 3;
            }
        }
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct TupleStruct<T1, T2, T3, T4> : ITupleStruct
    {

        private readonly T1 m_Item1;
        private readonly T2 m_Item2;
        private readonly T3 m_Item3;
        private readonly T4 m_Item4;

        public T1 Item1 { get { return m_Item1; } }
        public T2 Item2 { get { return m_Item2; } }
        public T3 Item3 { get { return m_Item3; } }
        public T4 Item4 { get { return m_Item4; } }

        public TupleStruct( T1 item1, T2 item2, T3 item3, T4 item4 ) {
            m_Item1 = item1;
            m_Item2 = item2;
            m_Item3 = item3;
            m_Item4 = item4;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append( "(" );
            return ( (ITupleStruct)this ).ToString( sb );
        }

        string ITupleStruct.ToString( StringBuilder sb ) {
            sb.Append( m_Item1 );
            sb.Append( ", " );
            sb.Append( m_Item2 );
            sb.Append( ", " );
            sb.Append( m_Item3 );
            sb.Append( ", " );
            sb.Append( m_Item4 );
            sb.Append( ")" );
            return sb.ToString();
        }

        int ITupleStruct.Size {
            get {
                return 4;
            }
        }
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct TupleStruct<T1, T2, T3, T4, T5> : ITupleStruct
    {

        private readonly T1 m_Item1;
        private readonly T2 m_Item2;
        private readonly T3 m_Item3;
        private readonly T4 m_Item4;
        private readonly T5 m_Item5;

        public T1 Item1 { get { return m_Item1; } }
        public T2 Item2 { get { return m_Item2; } }
        public T3 Item3 { get { return m_Item3; } }
        public T4 Item4 { get { return m_Item4; } }
        public T5 Item5 { get { return m_Item5; } }

        public TupleStruct( T1 item1, T2 item2, T3 item3, T4 item4, T5 item5 ) {
            m_Item1 = item1;
            m_Item2 = item2;
            m_Item3 = item3;
            m_Item4 = item4;
            m_Item5 = item5;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append( "(" );
            return ( (ITupleStruct)this ).ToString( sb );
        }

        string ITupleStruct.ToString( StringBuilder sb ) {
            sb.Append( m_Item1 );
            sb.Append( ", " );
            sb.Append( m_Item2 );
            sb.Append( ", " );
            sb.Append( m_Item3 );
            sb.Append( ", " );
            sb.Append( m_Item4 );
            sb.Append( ", " );
            sb.Append( m_Item5 );
            sb.Append( ")" );
            return sb.ToString();
        }

        int ITupleStruct.Size {
            get {
                return 5;
            }
        }
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct TupleStruct<T1, T2, T3, T4, T5, T6> : ITupleStruct
    {

        private readonly T1 m_Item1;
        private readonly T2 m_Item2;
        private readonly T3 m_Item3;
        private readonly T4 m_Item4;
        private readonly T5 m_Item5;
        private readonly T6 m_Item6;

        public T1 Item1 { get { return m_Item1; } }
        public T2 Item2 { get { return m_Item2; } }
        public T3 Item3 { get { return m_Item3; } }
        public T4 Item4 { get { return m_Item4; } }
        public T5 Item5 { get { return m_Item5; } }
        public T6 Item6 { get { return m_Item6; } }

        public TupleStruct( T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6 ) {
            m_Item1 = item1;
            m_Item2 = item2;
            m_Item3 = item3;
            m_Item4 = item4;
            m_Item5 = item5;
            m_Item6 = item6;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append( "(" );
            return ( (ITupleStruct)this ).ToString( sb );
        }

        string ITupleStruct.ToString( StringBuilder sb ) {
            sb.Append( m_Item1 );
            sb.Append( ", " );
            sb.Append( m_Item2 );
            sb.Append( ", " );
            sb.Append( m_Item3 );
            sb.Append( ", " );
            sb.Append( m_Item4 );
            sb.Append( ", " );
            sb.Append( m_Item5 );
            sb.Append( ", " );
            sb.Append( m_Item6 );
            sb.Append( ")" );
            return sb.ToString();
        }

        int ITupleStruct.Size {
            get {
                return 6;
            }
        }
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct TupleStruct<T1, T2, T3, T4, T5, T6, T7> : ITupleStruct
    {

        private readonly T1 m_Item1;
        private readonly T2 m_Item2;
        private readonly T3 m_Item3;
        private readonly T4 m_Item4;
        private readonly T5 m_Item5;
        private readonly T6 m_Item6;
        private readonly T7 m_Item7;

        public T1 Item1 { get { return m_Item1; } }
        public T2 Item2 { get { return m_Item2; } }
        public T3 Item3 { get { return m_Item3; } }
        public T4 Item4 { get { return m_Item4; } }
        public T5 Item5 { get { return m_Item5; } }
        public T6 Item6 { get { return m_Item6; } }
        public T7 Item7 { get { return m_Item7; } }

        public TupleStruct( T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7 ) {
            m_Item1 = item1;
            m_Item2 = item2;
            m_Item3 = item3;
            m_Item4 = item4;
            m_Item5 = item5;
            m_Item6 = item6;
            m_Item7 = item7;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append( "(" );
            return ( (ITupleStruct)this ).ToString( sb );
        }

        string ITupleStruct.ToString( StringBuilder sb ) {
            sb.Append( m_Item1 );
            sb.Append( ", " );
            sb.Append( m_Item2 );
            sb.Append( ", " );
            sb.Append( m_Item3 );
            sb.Append( ", " );
            sb.Append( m_Item4 );
            sb.Append( ", " );
            sb.Append( m_Item5 );
            sb.Append( ", " );
            sb.Append( m_Item6 );
            sb.Append( ", " );
            sb.Append( m_Item7 );
            sb.Append( ")" );
            return sb.ToString();
        }

        int ITupleStruct.Size {
            get {
                return 7;
            }
        }
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct TupleStruct<T1, T2, T3, T4, T5, T6, T7, TRest> : ITupleStruct
    {

        private readonly T1 m_Item1;
        private readonly T2 m_Item2;
        private readonly T3 m_Item3;
        private readonly T4 m_Item4;
        private readonly T5 m_Item5;
        private readonly T6 m_Item6;
        private readonly T7 m_Item7;
        private readonly TRest m_Rest;

        public T1 Item1 { get { return m_Item1; } }
        public T2 Item2 { get { return m_Item2; } }
        public T3 Item3 { get { return m_Item3; } }
        public T4 Item4 { get { return m_Item4; } }
        public T5 Item5 { get { return m_Item5; } }
        public T6 Item6 { get { return m_Item6; } }
        public T7 Item7 { get { return m_Item7; } }
        public TRest Rest { get { return m_Rest; } }

        public TupleStruct( T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest ) {
            m_Item1 = item1;
            m_Item2 = item2;
            m_Item3 = item3;
            m_Item4 = item4;
            m_Item5 = item5;
            m_Item6 = item6;
            m_Item7 = item7;
            m_Rest = rest;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append( "(" );
            return ( (ITupleStruct)this ).ToString( sb );
        }

        string ITupleStruct.ToString( StringBuilder sb ) {
            sb.Append( m_Item1 );
            sb.Append( ", " );
            sb.Append( m_Item2 );
            sb.Append( ", " );
            sb.Append( m_Item3 );
            sb.Append( ", " );
            sb.Append( m_Item4 );
            sb.Append( ", " );
            sb.Append( m_Item5 );
            sb.Append( ", " );
            sb.Append( m_Item6 );
            sb.Append( ", " );
            sb.Append( m_Item7 );
            sb.Append( ", " );
            return ( (ITupleStruct)m_Rest ).ToString( sb );
        }

        int ITupleStruct.Size {
            get {
                return 7 + ( (ITupleStruct)m_Rest ).Size;
            }
        }
    }
}