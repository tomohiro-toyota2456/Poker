namespace Excel
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using NPOI;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using NPOI.XSSF.UserModel;
    using NPOI.SS.UserModel;

    //エクセルのセルの中身を文字列取得するためのスクリプト
    //xlsx形式はテスト済み
    //複数シート対応
    //Open
    //GetCellData
    //Close
    public class ExcelReader
    {
        string m_sheetName = null;//取得するシート名
        IWorkbook m_workBook = null;

        public ExcelReader() { }

        public bool Open(string _filePath)
        {
            if (m_workBook == null)
            {
                m_workBook = CreateWorkBook(_filePath);
            }

            return m_workBook != null;
        }

        //終了処理
        public void Close()
        {
            if(m_workBook == null)
            {
                return;
            }

            m_sheetName = null;

            m_workBook.Close();
        }

        //指定エクセルのワークブックを作成
        IWorkbook CreateWorkBook(string _filePath)
        {
            FileStream fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            //ファイル拡張子をチェック
            int idx = _filePath.LastIndexOf('.');
            string checkString = _filePath.Substring(idx + 1);

            IWorkbook workBook = null;

            if(string.Compare(checkString,"XLSX",true) == 0)
            {
                workBook = new XSSFWorkbook(fileStream);
            }
            else if(string.Compare(checkString, "XLS", true) == 0)
            {
                workBook = new HSSFWorkbook(fileStream);
            }

            fileStream.Close();
            return workBook;
        }

        //シート名設定
        public void SetSheet(string _sheetName)
        {
            if (m_workBook != null)
            {
                m_sheetName = _sheetName;
            }
        }

        public void SetSheet(int _sheetIndex)
        {
            if(m_workBook != null)
            {
                m_sheetName = m_workBook.GetSheetName(_sheetIndex);
            }
        }

        //行と列指定して情報取得
        public string GetCellData(int _row, int _column)
        {
            if(m_workBook == null)
            {
                return null;
            }
            //setされているシート名から取得
            //別シートを取得したい場合はSetSheetを再度呼び出す
            if(string.IsNullOrEmpty(m_sheetName))
            {
                SetSheet(0);
            }

            ISheet sheet = m_workBook.GetSheet(m_sheetName);
            IRow row = sheet.GetRow(_row);

            if(row == null)
            {
                return null;
            }

            ICell cell = row.GetCell(_column);  
            
            if(cell == null)
            {
                return null;
            }

            string ans = cell.ToString();
            return ans;

        }

    }
}
