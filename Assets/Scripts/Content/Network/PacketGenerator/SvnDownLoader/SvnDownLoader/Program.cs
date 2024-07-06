using System;
using System.IO;
using SharpSvn;

public class SVNDownloader
{
    private static string svnUrl = "https://211.105.26.250:9438/svn/FOR/";
    private static string destinationDir = AppDomain.CurrentDomain.BaseDirectory;

    public static void Main(string[] args)
    {
        DownloadAndCopyFiles();
    }

    public static void DownloadAndCopyFiles()
    {
        using (SvnClient client = new SvnClient())
        {
            try
            {
                // 목적지 디렉토리가 존재하지 않으면 생성
                if (!Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }

                // SVN checkout
                SvnCheckOutArgs checkoutArgs = new SvnCheckOutArgs();
                client.CheckOut(new Uri(svnUrl), destinationDir, checkoutArgs);

                Console.WriteLine("다운로드 및 파일 복사가 완료되었습니다.");
            }
            catch (SvnSystemException ex)
            {
                Console.WriteLine("SVN 오류가 발생했습니다: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("오류가 발생했습니다: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}