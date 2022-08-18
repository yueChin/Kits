using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kits.DevlpKit.Helpers.IOHelpers
{
    public static class IOHelper 
    {

        public enum TreeWalkerCmd {
            Continue,
            Skip,
            Exit,
        }

        public interface ITreeWalker {
            bool IsRecursive();
            // will be called for each file while WalkTree is running
            TreeWalkerCmd DoFile( string name );
            // will be called for each directory while WalkTree is running
            TreeWalkerCmd DoDirectory( string name );
            // wildmatch pattern
            string FileSearchPattern();
            string DirectorySearchPattern();
        }

        public static void WalkTree( string dirName, ITreeWalker walker) 
        {
            int dirCount = 0;
            dirName = PathHelper.FormatPath( dirName );
            Stack<string> dirStack = new Stack<string>();
            dirStack.Push( dirName );
            while ( dirStack.Count > 0 ) 
            {
                string lastPath = dirStack.Pop();
                DirectoryInfo di = new DirectoryInfo( lastPath );
                if ( !di.Exists || ( ( di.Attributes & FileAttributes.Hidden ) != 0 && dirCount > 0 ) ) 
                {
                    continue;
                }
                ++dirCount;
                foreach ( FileInfo fileInfo in di.GetFiles( walker.FileSearchPattern() ) )
                {
                    // compose full file name from dirName
                    string f = lastPath;
                    if ( f[ f.Length - 1 ] == '/' ) 
                    {
                        f += fileInfo.Name;
                    } 
                    else 
                    {
                        f = f + "/" + fileInfo.Name;
                    }
                    TreeWalkerCmd cmd = walker.DoFile( f );
                    switch ( cmd ) 
                    {
                        case TreeWalkerCmd.Skip:
                            continue;
                        case TreeWalkerCmd.Exit:
                            goto EXIT;
                    }
                }
                if ( walker.IsRecursive() ) 
                {
                    foreach ( DirectoryInfo dirInfo in di.GetDirectories( walker.DirectorySearchPattern())) 
                    {
                        // compose full path name from dirName
                        string p = lastPath;
                        if ( p[ p.Length - 1 ] == '/' ) 
                        {
                            p += dirInfo.Name;
                        } 
                        else 
                        {
                            p = p + "/" + dirInfo.Name;
                        }
                        FileAttributes fa = File.GetAttributes( p );
                        if ( ( fa & FileAttributes.Hidden ) == 0 ) 
                        {
                            TreeWalkerCmd cmd = walker.DoDirectory( p );
                            switch ( cmd ) 
                            {
                                case TreeWalkerCmd.Skip:
                                    continue;
                                case TreeWalkerCmd.Exit:
                                    goto EXIT;
                            }
                            dirStack.Push( p );
                        }
                    }
                }
            }
            EXIT:
            ;
        }

        public class BaseTreeWalker : ITreeWalker 
        {
            public virtual bool IsRecursive() { return true; }
            public virtual TreeWalkerCmd DoFile( string name ) { return TreeWalkerCmd.Continue; }
            public virtual TreeWalkerCmd DoDirectory( string name ) { return TreeWalkerCmd.Continue; }
            public virtual string FileSearchPattern() { return "*"; }
            public virtual string DirectorySearchPattern() { return "*"; }
        }

        class FileScanner : BaseTreeWalker 
        {
            readonly List<string> m_AllFiles;
            readonly Func<string, Boolean> m_FileFilter;
            readonly Func<string, Boolean> m_DirectoryFilter;
            readonly bool m_Recursive;
            
            public FileScanner( List<string> fs, Func<string, Boolean> filter, bool recursive, Func<string, Boolean> directoryFilter ) {
                m_AllFiles = fs;
                m_FileFilter = filter;
                m_Recursive = recursive;
                m_DirectoryFilter = directoryFilter;
            }
            
            public override bool IsRecursive() 
            {
                return m_Recursive;
            }
            
            public override TreeWalkerCmd DoDirectory( string name ) {
                if ( m_DirectoryFilter != null && !m_DirectoryFilter( name ) ) 
                {
                    return TreeWalkerCmd.Skip;
                }
                return base.DoDirectory( name );
            }
            
            public override TreeWalkerCmd DoFile( string name ) 
            {
                if ( m_FileFilter == null || m_FileFilter( name ) ) 
                {
                    m_AllFiles.Add( name );
                }
                return TreeWalkerCmd.Continue;
            }
        }

        public static List<string> GetFileList( string path, Func<string, Boolean> filter, bool recursive = true ) 
        {
            List<string> ret = new List<string>();
            if ( !string.IsNullOrEmpty( path ) ) 
            {
                FileScanner fs = new FileScanner( ret, filter, recursive, null );
                WalkTree( path, fs );
            }
            return ret;
        }

        // create a directory
        // each sub directories will be created if any of them don't exist.
        public static bool CreateDirectory( string path ) 
        {
            try 
            {
                // first remove file name and extension;
                string ext = Path.GetExtension( path );
                string fileNameAndExt = Path.GetFileName( path );
                if ( !string.IsNullOrEmpty( fileNameAndExt ) && !string.IsNullOrEmpty( ext ) )
                {
                    path = path.Substring( 0, path.Length - fileNameAndExt.Length );
                }
                int i = 0;
                StringBuilder sb = new StringBuilder();
                string[] folderNames = path.Split( '/', '\\' );
                while ( string.IsNullOrEmpty( folderNames[ i ] ) )
                {
                    // 跳过前面的空分割文件夹名
                    // 比如path = "/a/b/c".Split( '/' ) => [ "", "a", "b", "c" ];
                    // 注意前面会有空字符串产生，跳过
                    ++i;
                }
                if ( folderNames.Length > i ) 
                {
                    // 如果输入路径以'/'起始，说明是posix系统的全路径
                    if ( path[ 0 ] == '/' ) 
                    {
                        // 追加全路径标记
                        folderNames[ i ] = "/" + folderNames[ i ];
                    }
                }
                for ( ; i < folderNames.Length; ++i ) 
                {
                    if ( string.IsNullOrEmpty( folderNames[ i ] ) ) 
                    {
                        // 如果中间有空格，忽略
                        // 如果输入路径最后是'/'，那么最后一个dirs[ i ]一定是空，跳过
                        continue;
                    }
                    if ( sb.Length > 0 && sb[ sb.Length - 1 ] != '/' ) {
                        sb.Append( '/' );
                    }
                    sb.Append( folderNames[ i ] );
                    string cur = sb.ToString();
                    if ( string.IsNullOrEmpty( cur ) ) 
                    {
                        continue;
                    }
                    if ( !Directory.Exists( cur ) )
                    {
                        DirectoryInfo info = Directory.CreateDirectory( cur );
                        if ( null == info ) {
                            return false;
                        }
                    }
                }
                return true;
            } 
            catch ( Exception e )
            {
                //Debug.LogException( e );
            }
            return false;
        }
    }
}
