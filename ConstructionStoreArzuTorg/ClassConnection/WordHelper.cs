using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;
namespace ConstructionStoreArzuTorg.ClassConnection
{
    internal class WordHelper
    {
        private FileInfo _fileInfo;
        public WordHelper(string fileName)
        {
            if (File.Exists(fileName))
                _fileInfo = new FileInfo(fileName);
            else MessageBox.Show("Ошибка");
        }

        public bool Process(Dictionary<string, string> items)
        {
            try
            {
                var app = new Word.Application();
                object file = _fileInfo.FullName;

                object missing = Type.Missing;

                app.Documents.Open(file);

                foreach (var item in items)
                {
                    Word.Find find = app.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;

                    object wrap = Word.WdFindWrap.wdFindContinue;
                    object replace = Word.WdReplace.wdReplaceAll;

                    find.Execute(
                        FindText: Type.Missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: missing,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: missing, Replace: replace);
                }

                object newFileName = Path.Combine(_fileInfo.DirectoryName, DateTime.Now.ToString("yyyyMMdd HHmmss") + _fileInfo.Name);
                app.ActiveDocument.SaveAs2(newFileName);



                app.ActiveDocument.Close();
                app.Quit();

                return true;
            }
            catch { MessageBox.Show("Ошибка"); }

            return false;
        }
    }
}
