
using K4os.Compression.LZ4.Streams;
using SevenZip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XjjXmm.Infrastructure.ToolKit
{
    public class CompressKit
    {
        public static void CompressLz4(string sourceFile)
        {
            LZ4EncoderSettings lZ4EncoderSettings = new LZ4EncoderSettings();
            lZ4EncoderSettings.CompressionLevel = K4os.Compression.LZ4.LZ4Level.L12_MAX;
            using (var source = File.OpenRead(sourceFile))
            using (var target = LZ4Stream.Encode(File.Create(sourceFile + ".lz4")))
            {
                source.CopyTo(target);
            }
        }

        /*        var tmp = new SevenZipCompressor(); //7z压缩
                tmp.ScanOnlyWritable = true; //只可写
                        //tmp.CompressFiles()这个有三个重载，这里只讲其中一个比较常用的。
                        //public void CompressFiles(string archiveName, params string[] fileFullNames)
                        //archiveName:这个是代表生成的7z文件存在哪里
                        //fileFullNames:这个参数是要压缩的文件是一个params数组，特别注意必须是完整的路径名才有效
                        tmp.CompressFiles(@"D:\max\arch.7z", @"D:\max\SourceCode\DataExch\SevenZipSharpDemo\bin\Debug\test.txt", @"D:\max\SourceCode\DataExch\SevenZipSharpDemo\bin\Debug\test1.txt");

                        //tmp.CompressDirectory 压缩指定路径下面的所有文件,这个有12个重载，也只讲其中一个简单的。
                       // public void CompressDirectory( string directory, string archiveName) 
                        tmp.CompressDirectory(@"D:\max\SourceCode\DataExch\SevenZipSharpDemo\bin\Debug", @"arch.7z");*/
        public static async Task Compress7z(string sourceFile)
        {
            //var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "7z.dll");

            SevenZipBase.SetLibraryPath(path);

            FileInfo fileInfo = new FileInfo(sourceFile);
            //fileInfo.DirectoryName + fileInfo.
            var tmp = new SevenZipCompressor(); //7z压缩
            tmp.ScanOnlyWritable = true; //只可写
                                         //tmp.CompressFiles()这个有三个重载，这里只讲其中一个比较常用的。
                                         //public void CompressFiles(string archiveName, params string[] fileFullNames)
                                         //archiveName:这个是代表生成的7z文件存在哪里
                                         //fileFullNames:这个参数是要压缩的文件是一个params数组，特别注意必须是完整的路径名才有效
            await tmp.CompressFilesAsync(fileInfo + ".7z", sourceFile);

            //tmp.CompressDirectory 压缩指定路径下面的所有文件,这个有12个重载，也只讲其中一个简单的。
            // public void CompressDirectory( string directory, string archiveName) 
            // tmp.CompressDirectory(@"D:\max\SourceCode\DataExch\SevenZipSharpDemo\bin\Debug", @"arch.7z");
        }

        public static async Task UnCompress7z(string sourceFile)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "7z.dll");

            SevenZipBase.SetLibraryPath(path);

            FileInfo fileInfo2 = new FileInfo(sourceFile);
            using (var tmp = new SevenZipExtractor(sourceFile))
            {
                await tmp.ExtractArchiveAsync(fileInfo2.DirectoryName);
                //7z文件路径
                /* for (int i = 0; i < tmp.ArchiveFileData.Count; i++)
                 {
                     tmp.ExtractFiles(@"d:\max\", tmp.ArchiveFileData[i].Index); //解压文件路径
                 }*/
            }
        }


    }
}
