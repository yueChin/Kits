using System.Threading;

namespace Kits.DevlpKit.Tools
{
#if !UNITY_WEBGL    
    public struct ReaderWriterLockSlimEx
    {
        public void EnterReadLock()           { m_SlimLock.EnterReadLock(); }
        public void ExitReadLock()            { m_SlimLock.ExitReadLock(); }
        public void EnterUpgradableReadLock() { m_SlimLock.EnterUpgradeableReadLock(); }
        public void ExitUpgradableReadLock()  { m_SlimLock.ExitUpgradeableReadLock(); }
        public void EnterWriteLock()          { m_SlimLock.EnterWriteLock(); }
        public void ExitWriteLock()           { m_SlimLock.ExitWriteLock(); }

        ReaderWriterLockSlim m_SlimLock;

        public static ReaderWriterLockSlimEx Create()
        {
            return new ReaderWriterLockSlimEx() {m_SlimLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion)};
        }
    }
#else
    public struct ReaderWriterLockSlimEx
    {
        public void EnterReadLock() { ThrowOnMoreThanOne(ref _readLock); }
        public void ExitReadLock() { ReleaseLock(ref _readLock); }
        
        public void EnterUpgradableReadLock() { ThrowOnMoreThanOne(ref _upgradableReadLock); }
        public void ExitUpgradableReadLock()  { ReleaseLock(ref _upgradableReadLock); }
        public void EnterWriteLock()          { ThrowOnMoreThanOne(ref _writeLock); }
        public void ExitWriteLock()           { ReleaseLock(ref _writeLock); }
        
        void ThrowOnMoreThanOne(ref int readLock)
        {
            if (_readLock++ > 0) throw new System.Exception("RecursiveLock not supported");
        }

        void ReleaseLock(ref int readLock) { --readLock; }

        int _readLock;
        int _upgradableReadLock;
        int _writeLock;

        public static ReaderWriterLockSlimEx Create() { return new ReaderWriterLockSlimEx(); }
    }
#endif
}