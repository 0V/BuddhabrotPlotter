using System;
using System.IO;
using System.Text;

namespace Common
{
    public class Utils
    {

        /// <summary>
        /// ファイル名として使用可能な形にする。
        /// </summary>
        /// <param name="s">元のファイル名</param>
        public static string ValidFileName(string s)
        {

            string valid = s;
            char[] invalidch = Path.GetInvalidFileNameChars();

            foreach (char c in invalidch)
            {
                valid = valid.Replace(c, '_');
            }
            return valid;
        }

        /// <summary>
        /// ログを残しアプリケーションを通常終了
        /// </summary>
        /// <param name="comment">option コメント</param>
        public static void Exit(string message = "No message")
        {
            try
            {
                WriteNormalLog(message);
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 例外オブジェクトの情報をファイルに出力しアプリケーションを終了
        /// </summary>
        /// <param name="ex">Exeption型のオブジェクト</param>
        /// <param name="comment">option 例外のコメント</param>
        public static void Exit(Exception ex, string comment = "例外が発生しました")
        {
            try
            {
                WriteErrorLog(ex, comment);
                Console.Write("\n\nPlease exit by pressing the Enter key > ");
                Console.ReadLine();
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// ファイルに出力
        /// </summary>
        /// <param name="comment">出力内容</param>
        /// <param name="directoryName">option ディレクトリ名</param>
        public static void WriteNormalLog(string comment,
          string directoryName = "Log")
        {
            string fileName = string.Format("Log{0}.txt", GetTimeString());
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            string contents = comment + "\n";

            File.WriteAllText(directoryName + "/" + fileName, contents);
        }

        /// <summary>
        /// ファイルに出力
        /// </summary>
        /// <param name="comment">出力内容</param>
        /// <param name="directoryName">option ディレクトリ名</param>
        public static void WriteExitLog(string comment,
          string directoryName = "ExitLog")
        {
            string fileName = string.Format("ExitLog{0}.txt", GetTimeString());
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            string contents = comment + "\n\n" + "プログラムを通常終了しました";

            File.WriteAllText(directoryName + "/" + fileName, contents);
        }

        /// <summary>
        /// 例外オブジェクトの情報をファイルに出力
        /// </summary>
        /// <param name="ex">例外オブジェクト</param>
        /// <param name="comment">option 例外のコメント</param>
        /// <param name="directoryName">option ディレクトリ名</param>
        public static void WriteExitLog(Exception ex, string comment = "例外が発生しました",
          string directoryName = "ErrorExitLog")
        {
            string fileName = string.Format("ErrorExitLog{0}.txt", GetTimeString());
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            string contents = comment + "\n" + GetExceptionInfoString(ex) + "\n\n" + "プログラムを例外終了しました";

            File.WriteAllText(@directoryName + "/" + fileName, contents);
        }

        /// <summary>
        /// 例外オブジェクトの情報をファイルに出力
        /// </summary>
        /// <param name="ex">例外オブジェクト</param>
        /// <param name="comment">option 例外のコメント</param>
        /// <param name="directoryName">option ディレクトリ名</param>
        public static void WriteErrorLog(Exception ex, string comment = "例外が発生しました",
          string directoryName = "ErrorLog")
        {
            string fileName = string.Format("ErrorLog{0}.txt", GetTimeString());
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            string contents = comment + "\n" + GetExceptionInfoString(ex) + "\n";

            File.WriteAllText(@directoryName + "/" + fileName, contents);
        }

        /// <summary>
        /// 例外オブジェクトからデバッグに必要な情報を文字列で返す
        /// </summary>
        /// <param name="ex">例外オブジェクト</param>
        /// <returns>例外情報の文字列</returns>
        public static string GetExceptionInfoString(Exception ex)
        {
            var str = new StringBuilder();
            str.AppendFormat("Message:{0}\n", ex.Message);
            str.AppendFormat("Source:{0}\n", ex.Source);
            str.AppendFormat("HelpLink:{0}\n", ex.HelpLink);
            str.AppendFormat("TargetSite:{0}\n", ex.TargetSite.ToString());
            str.AppendFormat("StackTrace:{0}\n", ex.StackTrace);
            return str.ToString();
        }

        /// <summary>
        /// 呼び出された時間を年から秒までの文字列で返す
        /// </summary>
        /// <returns>時間の文字列</returns>
        public static string GetTimeString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }
    }
}
