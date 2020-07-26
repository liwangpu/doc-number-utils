using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EpplusHelper
{
    public static class XLSToXLSXConverter
    {
        public static void Convert(string xlsPath, string xlsxPath)
        {
            using (var sourceStream = new FileStream(xlsPath, FileMode.Open, FileAccess.Read))
            {

                var source = new HSSFWorkbook(sourceStream);
                var destination = new XSSFWorkbook();
                for (int i = 0; i < source.NumberOfSheets; i++)
                {
                    var xssfSheet = (XSSFSheet)destination.CreateSheet(source.GetSheetAt(i).SheetName);
                    var hssfSheet = (HSSFSheet)source.GetSheetAt(i);
                    CopySheets(hssfSheet, xssfSheet);
                }
                using (var stream = File.Create(xlsxPath))
                {
                    destination.Write(stream);
                }
            }
        }

        private static void CopySheets(HSSFSheet source, XSSFSheet destination)
        {
            var maxColumnNum = 0;
            var mergedRegions = new List<CellRangeAddress>();
            var styleMap = new Dictionary<int, HSSFCellStyle>();
            for (int i = source.FirstRowNum; i <= source.LastRowNum; i++)
            {
                var srcRow = (HSSFRow)source.GetRow(i);
                var destRow = (XSSFRow)destination.CreateRow(i);
                if (srcRow != null)
                {
                    CopyRow(source, destination, srcRow, destRow, mergedRegions);
                    if (srcRow.LastCellNum > maxColumnNum)
                    {
                        maxColumnNum = srcRow.LastCellNum;
                    }
                }
            }
            for (int i = 0; i <= maxColumnNum; i++)
            {
                destination.SetColumnWidth(i, source.GetColumnWidth(i));
            }
        }

        private static void CopyRow(HSSFSheet srcSheet, XSSFSheet destSheet, HSSFRow srcRow, XSSFRow destRow, List<CellRangeAddress> mergedRegions)
        {
            destRow.Height = srcRow.Height;

            for (int j = srcRow.FirstCellNum; srcRow.LastCellNum >= 0 && j <= srcRow.LastCellNum; j++)
            {
                var oldCell = (HSSFCell)srcRow.GetCell(j);
                var newCell = (XSSFCell)destRow.GetCell(j);
                if (oldCell != null)
                {
                    if (newCell == null)
                    {
                        newCell = (XSSFCell)destRow.CreateCell(j);
                    }

                    CopyCell(oldCell, newCell);

                    var mergedRegion = GetMergedRegion(srcSheet, srcRow.RowNum,
                            (short)oldCell.ColumnIndex);

                    if (mergedRegion != null)
                    {
                        var newMergedRegion = new CellRangeAddress(mergedRegion.FirstRow,
                                mergedRegion.LastRow, mergedRegion.FirstColumn, mergedRegion.LastColumn);
                        if (IsNewMergedRegion(newMergedRegion, mergedRegions))
                        {
                            mergedRegions.Add(newMergedRegion);
                            destSheet.AddMergedRegion(newMergedRegion);
                        }
                    }
                }
            }
        }

        private static void CopyCell(HSSFCell oldCell, XSSFCell newCell)
        {
            CopyCellValue(oldCell, newCell);
        }

        private static void CopyCellValue(HSSFCell oldCell, XSSFCell newCell)
        {
            switch (oldCell.CellType)
            {
                case CellType.String:
                    newCell.SetCellValue(oldCell.StringCellValue);
                    break;
                case CellType.Numeric:
                    newCell.SetCellValue(oldCell.NumericCellValue);
                    break;
                case CellType.Blank:
                    newCell.SetCellType(CellType.Blank);
                    break;
                case CellType.Boolean:
                    newCell.SetCellValue(oldCell.BooleanCellValue);
                    break;
                case CellType.Error:
                    newCell.SetCellErrorValue(oldCell.ErrorCellValue);
                    break;
                case CellType.Formula:
                    newCell.SetCellFormula(oldCell.CellFormula);
                    break;
                default:
                    break;
            }
        }

        private static void CopyCellStyle(HSSFCell oldCell, XSSFCell newCell)
        {
            if (oldCell.CellStyle == null) return;
            newCell.CellStyle = newCell.Sheet.Workbook.GetCellStyleAt((short)(oldCell.CellStyle.Index + 1));


        }

        private static CellRangeAddress GetMergedRegion(HSSFSheet sheet, int rowNum, short cellNum)
        {
            for (var i = 0; i < sheet.NumMergedRegions; i++)
            {
                var merged = sheet.GetMergedRegion(i);
                if (merged.IsInRange(rowNum, cellNum))
                {
                    return merged;
                }
            }
            return null;
        }

        private static bool IsNewMergedRegion(CellRangeAddress newMergedRegion,
                List<CellRangeAddress> mergedRegions)
        {
            return !mergedRegions.Any(r =>
            r.FirstColumn == newMergedRegion.FirstColumn &&
            r.LastColumn == newMergedRegion.LastColumn &&
            r.FirstRow == newMergedRegion.FirstRow &&
            r.LastRow == newMergedRegion.LastRow);
        }
    }
}
