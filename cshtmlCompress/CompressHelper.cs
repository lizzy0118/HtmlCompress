using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace cshtmlCompress
{
    /// <summary>
    /// compress
    /// </summary>
    public static class CompressHelper
    {
        public static bool compressor(string oldpath, string newpath)
        {
            if (string.IsNullOrEmpty(oldpath) || string.IsNullOrEmpty(newpath)) return false;
            Encoding code = Encoding.GetEncoding("utf-8");
            StreamReader sr = null;
            string str = "";
            try
            {
                sr = new StreamReader(oldpath, code);
                str = sr.ReadToEnd();
                sr.Close();
            }
            catch
            {
                return false;
            }
            StreamWriter sw1 = null;
            try
            {//将注释去掉
                sw1 = new StreamWriter(newpath, false);
                string[] strs = str.Split('\n');
                if (strs != null && strs.Length > 0)
                {
                    for (int i = 0, len = strs.Length; i < len; i++)
                    {
                        if (strs[i].Contains("\t//"))
                        {
                            string str1 = strs[i].Replace("\t//", "§");
                            str = str.Replace("\t//" + str1.Substring(str1.IndexOf('§') + 1), "");
                            continue;
                        }
                        if (strs[i].Contains("//\t"))
                        {
                            string str1 = strs[i].Replace("//\t", "§");
                            str = str.Replace("//\t" + str1.Substring(str1.IndexOf('§') + 1), "");
                            continue;
                        }
                        if (strs[i].Contains("///"))
                        {
                            string str1 = strs[i].Replace("///", "§");
                            str = str.Replace("///" + str1.Substring(str1.IndexOf('§') + 1), "");
                            continue;
                        }
                        if (strs[i].Contains(" //"))
                        {
                            string str1 = strs[i].Replace(" //", "§");
                            str = str.Replace("//" + str1.Substring(str1.IndexOf('§') + 1), "");
                            continue;
                        }
                        if (strs[i].Contains("// "))
                        {
                            string str1 = strs[i].Replace("// ", "§ ");
                            str = str.Replace("//" + str1.Substring(str1.IndexOf('§') + 1), "");
                            continue;
                        }
                        if (strs[i].Contains("{//"))
                        {
                            string str1 = strs[i].Replace("{//", "{§");
                            str = str.Replace("//" + str1.Substring(str1.IndexOf('§') + 1), "");
                            continue;
                        }
                        if (strs[i].Contains("}//"))
                        {
                            string str1 = strs[i].Replace("}//", "}§");
                            str = str.Replace("//" + str1.Substring(str1.IndexOf('§') + 1), "");
                            continue;
                        }
                    }
                }
                if (str.Contains("// "))
                {
                    str = str.Replace("//  ", "");
                }
                sw1.Write(str);
                sw1.Flush();
            }
            catch
            {
                return false;
            }
            finally
            {
                if (sw1 != null)
                    sw1.Close();
            }
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(newpath, false, code);
                str = str.Replace("/*", "ˇ");
                str = str.Replace("*/", "§");
                str = str.Replace("@*", "ˇ");
                str = str.Replace("*@", "§");
                UpdateA(ref str);
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Replace("\r\n", " ");
                    str = str.Replace("\n", " ");
                    str = str.Replace("\t", "");
                    str = str.Replace("                               ", " ");
                    str = str.Replace("                     ", " ");
                    str = str.Replace("            ", " ");
                    str = str.Replace("        ", " ");
                    str = str.Replace("   ", "");
                    str = str.Replace("  ", " ");
                    str = str.Replace("　　　", "");
                    str = str.Replace("　　", " ");
                    if (str.Contains("@Html"))
                    {
                        str = str.Replace("@Html", " @Html");
                        str = str.Replace("' @Html", "'@Html");
                        str = str.Replace("\" @Html", "\"@Html");
                    }
                    if (str.Contains("@{"))
                    {
                        str = str.Insert(str.IndexOf("@{"), "\r\n");
                    }
                }
                str = str.Replace("function ", ";function ");
                str = str.Replace("(;function ", "(function ");
                str = str.Replace(";;function ", ";function ");
                str = str.Replace("; ;function ", ";function ");
                str = str.Replace(": ;function ", ": function ");
                sw.Write(str);
                sw.Flush();
            }
            catch
            {
                return false;
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
            return true;
        }

        private static void UpdateA(ref string str)
        {
            if (str.Contains("ˇ") && str.Contains("§"))
            {
                int a = str.IndexOf('ˇ');
                int b = str.IndexOf('§');
                if (a <= b)
                {
                    str = str.Replace(str.Substring(a, b - a + 1), "");
                }
                else
                {
                    str = str.Replace(str.Substring(b, a - b + 1), "");
                }
                UpdateA(ref str);
            }
        }
    }
}
