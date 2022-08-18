using System;
using System.Collections;
using System.Collections.Generic;

namespace Kits.DevlpKit.Supplements.Collections
{
    public class TransformEnumerator : IEnumerator
    {
        private readonly IEnumerator m_Enumerator;
        private readonly Converter<object, object> m_Converter;
        public TransformEnumerator(IEnumerator enumerator, Converter<object, object> converter)
        {
            this.m_Enumerator = enumerator;
            this.m_Converter = converter;
        }

        public object Current { get; private set; }

        public bool MoveNext()
        {
            if (this.m_Enumerator.MoveNext())
            {
                object current = this.m_Enumerator.Current;
                this.Current = m_Converter(current);
                return true;
            }
            return false;
        }

        public void Reset()
        {
            this.m_Enumerator.Reset();
        }
    }

    public class TransformEnumerator<TInput, TOutput> : IEnumerator<TOutput>
    {
        private IEnumerator<TInput> m_Enumerator;
        private Converter<TInput, TOutput> m_Converter;
        public TransformEnumerator(IEnumerator<TInput> enumerator, Converter<TInput, TOutput> converter)
        {
            this.m_Enumerator = enumerator;
            this.m_Converter = converter;
        }

        public TOutput Current { get; private set; }

        object IEnumerator.Current { get { return this.Current; } }

        public bool MoveNext()
        {
            if (this.m_Enumerator.MoveNext())
            {
                TInput current = this.m_Enumerator.Current;
                this.Current = m_Converter(current);
                return true;
            }
            return false;
        }

        public void Reset()
        {
            this.m_Enumerator.Reset();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                this.Reset();
                this.m_Enumerator = null;
                this.m_Converter = null;
                this.disposedValue = true;
            }
        }

        ~TransformEnumerator()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
