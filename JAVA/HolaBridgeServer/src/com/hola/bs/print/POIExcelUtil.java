package com.hola.bs.print;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;
import java.util.regex.Pattern;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.apache.poi.hssf.usermodel.HSSFCell;
import org.apache.poi.hssf.usermodel.HSSFWorkbook;
import org.apache.poi.ss.usermodel.Cell;
import org.apache.poi.ss.usermodel.CellStyle;
import org.apache.poi.ss.usermodel.ClientAnchor;
import org.apache.poi.ss.usermodel.CreationHelper;
import org.apache.poi.ss.usermodel.DateUtil;
import org.apache.poi.ss.usermodel.Drawing;
import org.apache.poi.ss.usermodel.Font;
import org.apache.poi.ss.usermodel.Picture;
import org.apache.poi.ss.usermodel.Row;
import org.apache.poi.ss.usermodel.Sheet;
import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.ss.usermodel.WorkbookFactory;
import org.apache.poi.ss.util.CellRangeAddress;
import org.apache.poi.ss.util.CellReference;
import org.apache.poi.util.IOUtils;
import org.apache.poi.xssf.usermodel.XSSFCellStyle;
import org.apache.poi.xssf.usermodel.XSSFColor;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;

/**
 * @className:POIExcelUtil.java
 * @classDescription:POI操作类
 */

public class POIExcelUtil {
    private static Log log = LogFactory.getLog(POIExcelUtil.class);

    // ------------------------写Excel-----------------------------------
    /**
     * 创建workBook对象 xlsx(2007以上版本)
     * 
     * @return
     */
    public static Workbook createWorkbook() {
        return createWorkbook(false);
    }

    /**
     * 创建WorkBook对象
     * 
     * @param flag
     *            true:xlsx(1997-2007) false:xls(2007以下)
     * @return
     */
    public static Workbook createWorkbook(boolean flag) {
        Workbook wb;
        if (flag) {
            wb = new XSSFWorkbook();
        } else {
            wb = new HSSFWorkbook();
        }
        return wb;
    }

    /**
     * 添加图片
     * 
     * @param wb
     *            workBook对象
     * @param sheet
     *            sheet对象
     * @param picFileName
     *            图片文件名称（全路径）
     * @param picType
     *            图片类型
     * @param row
     *            图片所在的行
     * @param col
     *            图片所在的列
     */
    public static void addPicture(Workbook wb, Sheet sheet, String picFileName, int picType, int row, int col) {
        InputStream is = null;
        try {
            // 读取图片
            is = new FileInputStream(picFileName);
            byte[] bytes = IOUtils.toByteArray(is);
            int pictureIdx = wb.addPicture(bytes, picType);
            is.close();
            // 写图片
            CreationHelper helper = wb.getCreationHelper();
            Drawing drawing = sheet.createDrawingPatriarch();
            ClientAnchor anchor = helper.createClientAnchor();
            // 设置图片的位置
            anchor.setCol1(col);
            anchor.setRow1(row);
            Picture pict = drawing.createPicture(anchor, pictureIdx);

            pict.resize();
        } catch (Exception e) {
            try {
                if (is != null) {
                    is.close();
                }
            } catch (IOException e1) {
                e1.printStackTrace();
            }
            e.printStackTrace();
        }
    }

    /**
     * 创建Cell 默认为水平和垂直方式都是居中
     * 
     * @param style
     *            CellStyle对象
     * @param row
     *            Row对象
     * @param column
     *            单元格所在的列
     * @return
     */
    public static Cell createCell(CellStyle style, Row row, short column) {
        return createCell(style, row, column, XSSFCellStyle.ALIGN_CENTER, XSSFCellStyle.ALIGN_CENTER);
    }

    /**
     * 创建Cell并设置水平和垂直方式
     * 
     * @param style
     *            CellStyle对象
     * @param row
     *            Row对象
     * @param column
     *            单元格所在的列
     * @param halign
     *            水平对齐方式：XSSFCellStyle.VERTICAL_CENTER.
     * @param valign
     *            垂直对齐方式：XSSFCellStyle.ALIGN_LEFT
     */
    public static Cell createCell(CellStyle style, Row row, short column, short halign, short valign) {
        Cell cell = row.createCell(column);
        setAlign(style, halign, valign);
        cell.setCellStyle(style);
        return cell;
    }

    /**
     * 合并单元格
     * 
     * @param sheet
     * @param firstRow
     *            开始行
     * @param lastRow
     *            最后行
     * @param firstCol
     *            开始列
     * @param lastCol
     *            最后列
     */
    public static void mergeCell(Sheet sheet, int firstRow, int lastRow, int firstCol, int lastCol) {
        sheet.addMergedRegion(new CellRangeAddress(firstRow, lastRow, firstCol, lastCol));
    }

    // ---------------------------------设置样式-----------------------

    /**
     * 设置单元格对齐方式
     * 
     * @param style
     * @param halign
     * @param valign
     * @return
     */
    public static CellStyle setAlign(CellStyle style, short halign, short valign) {
        style.setAlignment(halign);
        style.setVerticalAlignment(valign);
        return style;
    }

    /**
     * 设置单元格边框(四个方向的颜色一样)
     * 
     * @param style
     *            style对象
     * @param borderStyle
     *            边框类型 ：dished-虚线 thick-加粗 double-双重 dotted-有点的 CellStyle.BORDER_THICK
     * @param borderColor
     *            颜色 IndexedColors.GREEN.getIndex()
     * @return
     */
    public static CellStyle setBorder(CellStyle style, short borderStyle, short borderColor) {

        // 设置底部格式（样式+颜色）
        style.setBorderBottom(borderStyle);
        style.setBottomBorderColor(borderColor);
        // 设置左边格式
        style.setBorderLeft(borderStyle);
        style.setLeftBorderColor(borderColor);
        // 设置右边格式
        style.setBorderRight(borderStyle);
        style.setRightBorderColor(borderColor);
        // 设置顶部格式
        style.setBorderTop(borderStyle);
        style.setTopBorderColor(borderColor);

        return style;
    }

    /**
     * 自定义颜色（xssf)
     * 
     * @param style
     *            xssfStyle
     * @param red
     *            RGB red (0-255)
     * @param green
     *            RGB green (0-255)
     * @param blue
     *            RGB blue (0-255)
     */
    public static CellStyle setBackColorByCustom(XSSFCellStyle style, int red, int green, int blue) {
        // 设置前端颜色
        style.setFillForegroundColor(new XSSFColor(new java.awt.Color(red, green, blue)));
        // 设置填充模式
        style.setFillPattern(CellStyle.SOLID_FOREGROUND);

        return style;
    }

    /**
     * 设置前景颜色
     * 
     * @param style
     *            style对象
     * @param color
     *            ：IndexedColors.YELLOW.getIndex()
     * @return
     */
    public static CellStyle setBackColor(CellStyle style, short color) {

        // 设置前端颜色
        style.setFillForegroundColor(color);
        // 设置填充模式
        style.setFillPattern(CellStyle.SOLID_FOREGROUND);

        return style;
    }

    /**
     * 设置背景颜色
     * 
     * @param style
     *            style对象
     * @param color
     *            ：IndexedColors.YELLOW.getIndex()
     * @param fillPattern
     *            ：CellStyle.SPARSE_DOTS
     * @return
     */
    public static CellStyle setBackColor(CellStyle style, short backColor, short fillPattern) {

        // 设置背景颜色
        style.setFillBackgroundColor(backColor);

        // 设置填充模式
        style.setFillPattern(fillPattern);

        return style;
    }

    /**
     * 
     * 设置字体（简单的需求实现，如果复杂的字体，需要自己去实现）尽量重用
     * 
     * @param style
     *            style对象
     * @param fontSize
     *            字体大小 shot(24)
     * @param color
     *            字体颜色 IndexedColors.YELLOW.getIndex()
     * @param fontName
     *            字体名称 "Courier New"
     * @param
     */
    public static CellStyle setFont(Font font, CellStyle style, short fontSize, short color, String fontName) {
        font.setFontHeightInPoints(color);
        font.setFontName(fontName);
        font.setFontHeight((short) (24));
        // font.setItalic(true);// 斜体
        // font.setStrikeout(true);//加干扰线

        font.setColor(color);// 设置颜色
        // Fonts are set into a style so create a new one to use.
        style.setFont(font);

        return style;

    }

    /**
     * 
     * @param createHelper
     *            createHelper对象
     * @param style
     *            CellStyle对象
     * @param formartData
     *            date:"m/d/yy h:mm"; int:"#,###.0000" ,"0.0"
     */
    public static CellStyle setDataFormat(CreationHelper createHelper, CellStyle style, String formartData) {

        style.setDataFormat(createHelper.createDataFormat().getFormat(formartData));

        return style;
    }

    /**
     * 将Workbook写入文件
     * 
     * @param wb
     *            workbook对象
     * @param fileName
     *            文件的全路径
     * @return
     */
    public static boolean createExcel(Workbook wb, String fileName) {
        boolean flag = true;
        FileOutputStream fileOut = null;
        try {
            File file = new File(fileName);
            File parent = file.getParentFile();
            if (parent != null && !parent.exists()) {
                parent.mkdirs();
            }
            fileOut = new FileOutputStream(fileName);
            wb.write(fileOut);
            fileOut.close();

        } catch (Exception e) {
            flag = false;
            if (fileOut != null) {
                try {
                    fileOut.close();
                } catch (IOException e1) {
                    // TODO Auto-generated catch block
                    e1.printStackTrace();
                }
            }
            e.printStackTrace();
        }
        return flag;
    }

    // --------------------读取Excel-----------------------
    /**
     * 读取Excel
     * 
     * @param filePathName
     * @return
     */
    public static Workbook readExcel(String filePathName) {
        InputStream inp = null;
        Workbook wb = null;
        try {
            inp = new FileInputStream(filePathName);
            wb = WorkbookFactory.create(inp);
            inp.close();

        } catch (Exception e) {
            try {
                if (null != inp) {
                    inp.close();
                }
            } catch (IOException e1) {
                // TODO Auto-generated catch block
                e1.printStackTrace();
            }
            e.printStackTrace();
        }
        return wb;
    }

    /**
     * 读取Cell的值
     * 
     * @param sheet
     * @return
     */
    public static Map readCell(Sheet sheet) {
        Map map = new HashMap();
        // 遍历所有行
        for (Row row : sheet) {
            // 便利所有列
            for (Cell cell : row) {
                // 获取单元格的类型
                CellReference cellRef = new CellReference(row.getRowNum(), cell.getColumnIndex());
                // System.out.print(cellRef.formatAsString());
                String key = cellRef.formatAsString();
                // System.out.print(" - ");

                switch (cell.getCellType()) {
                // 字符串
                case Cell.CELL_TYPE_STRING:
                    map.put(key, cell.getRichStringCellValue().getString());
                    // System.out.println(cell.getRichStringCellValue()
                    // .getString());
                    break;
                // 数字
                case Cell.CELL_TYPE_NUMERIC:
                    if (DateUtil.isCellDateFormatted(cell)) {
                        // System.out.println(cell.getDateCellValue());
                        map.put(key, cell.getDateCellValue());
                    } else {
                        // System.out.println(cell.getNumericCellValue());
                        map.put(key, cell.getNumericCellValue());
                    }
                    break;
                // boolean
                case Cell.CELL_TYPE_BOOLEAN:
                    // System.out.println(cell.getBooleanCellValue());
                    map.put(key, cell.getBooleanCellValue());
                    break;
                // 方程式
                case Cell.CELL_TYPE_FORMULA:
                    // System.out.println(cell.getCellFormula());
                    map.put(key, cell.getCellFormula());
                    break;
                // 空值
                default:
                    System.out.println();
                    map.put(key, "");
                }

            }
        }
        return map;

    }

    /**
     * 替换明细内容
     * 
     * @param sheet
     * @param detailList
     *            明细列表
     */
    public static void replaceDetail(Workbook workbook, Sheet sheet, String detailKey,
            List<Map<String, Object>> detailList) {
        // 找到 #detail 的位置，复制该行到到下一行
        if (detailList == null || detailList.size() == 0)
            return;

        int detailSize = detailList.size();

        Cell detailCell = findCell(sheet, detailKey);
        if (detailCell == null)
            return;

        // #detail 所在行号
        int detailRow = detailCell.getRowIndex();
        // #detail 所在列号
        int detailColunm = detailCell.getColumnIndex();

        for (int i = 1; i < detailSize; i++) {
            copyRow(workbook, sheet, detailRow, detailRow + i);
        }

        int ids = 1;
        int[] rownum = getDetailColunmNum(sheet, detailRow, detailColunm, detailList.get(0).size());
        for (Map<String, Object> detailMap : detailList) {
            int colunm = 0;
            Row thisRow = sheet.getRow(detailRow);
            float defaultRowHeight = thisRow.getHeightInPoints();
            float maxHeight = defaultRowHeight;

//            Set set = detailMap.keySet();
//            Iterator iter = set.iterator();
//            while(iter.hasNext()){
//            	String key = (String)iter.next();
//            	Cell thisCell = thisRow.getCell(rownum[colunm]);
//            	String cellValue = detailMap.get(key).toString();
//            	 if(cellValue!=null && cellValue.equalsIgnoreCase("ids"))
//                 	cellValue = String.valueOf(ids++);
//                 
//                 float thisHeight = getExcelCellAutoHeight(cellValue, defaultRowHeight, getMergedCellNum(thisCell)
//                         * sheet.getColumnWidth(rownum[colunm]) / 256);
//                 if (thisHeight > maxHeight)
//                     maxHeight = thisHeight;
//
//                 if (thisCell == null) {
//                     thisCell = thisRow.createCell(rownum[colunm]);
//                     thisCell.setCellValue(cellValue);
//                   //  System.out.println("********again!*******************cell value is "+cellValue);
//                 } else {
//                     thisCell.setCellValue(cellValue);
//                     thisCell.getCellStyle().setWrapText(true);
//                    // System.out.println("********again!*******************cell value is "+cellValue);
//                 }
//
//                 colunm++;
//            }
            for(String key:detailMap.keySet()){
//            for (Entry<String, Object> entry : detailMap.entrySet()) {

                Cell thisCell = thisRow.getCell(rownum[colunm]);
                
                String cellValue = detailMap.get(key) == null?"":detailMap.get(key).toString();
                if(cellValue.equalsIgnoreCase("ids"))
                	cellValue = String.valueOf(ids++);
                
                float thisHeight = getExcelCellAutoHeight(cellValue, defaultRowHeight, getMergedCellNum(thisCell)
                        * sheet.getColumnWidth(rownum[colunm]) / 256);
                if (thisHeight > maxHeight)
                    maxHeight = thisHeight;

                if (thisCell == null) {
                    thisCell = thisRow.createCell(rownum[colunm]);
                    thisCell.setCellValue(cellValue);
                    //System.out.println("********again!*******************cell value is "+cellValue);
                } else {
                	//System.out.println("********again!*******************cell value is "+cellValue);
                	thisCell.setCellValue(cellValue);
                    thisCell.getCellStyle().setWrapText(true);
                    
                    
                }

                colunm++;
            }
            thisRow.setHeightInPoints(maxHeight);
            detailRow++;
        }
    }

    private static int[] getDetailColunmNum(Sheet sheet, int detailRow, int detalColunm, int detailMapSize) {
        int[] rowNum = new int[detailMapSize];
        int mergedRow = 0;
        int j = 0;
        for (int i = 0; i < detailMapSize + mergedRow; i++) {
            Cell cell = sheet.getRow(detailRow).getCell(i + detalColunm);
            if (isMergedRegionNotFirstCell(cell)) {
                mergedRow++;
            } else {
                rowNum[j++] = i + detalColunm;
            }

        }
        return rowNum;

    }

    /**
     * 判断当前单元格是合并单元格的非第一行
     * 
     * @param cell
     * @return
     */
    private static int getMergedCellNum(Cell cell) {
        if (cell == null)
            return 1;
        Sheet sheet = cell.getSheet();

        CellRangeAddress range = null;
        int mergedNum = sheet.getNumMergedRegions();
        for (int i = 0; i < mergedNum; i++) {
            range = sheet.getMergedRegion(i);
            if (cell.getColumnIndex() == range.getFirstColumn() && cell.getRowIndex() == range.getFirstRow()) {
                return range.getLastColumn() - range.getFirstColumn() + 1;
            }
        }
        return 1;
    }

    /**
     * 判断当前单元格是合并单元格的非第一行
     * 
     * @param cell
     * @return
     */
    private static boolean isMergedRegionNotFirstCell(Cell cell) {
        if (cell == null)
            return false;
        Sheet sheet = cell.getSheet();

        CellRangeAddress range = null;
        int mergedNum = sheet.getNumMergedRegions();
        for (int i = 0; i < mergedNum; i++) {
            range = sheet.getMergedRegion(i);
            if (cell.getColumnIndex() > range.getFirstColumn() && cell.getColumnIndex() <= range.getLastColumn()
                    && cell.getRowIndex() == range.getFirstRow()) {
                return true;
            }
        }
        return false;
    }
    
    private static boolean isNumber(String str){
    	return str.matches("^[-+]?((0|[1-9][0-9]+)([.]([0-9]+))?|([.]([0-9]+))?)$");
    	
    }

    /**
     * 替换文本
     * 
     * @param sheet
     *            工作表
     * @param replaceTextMap
     *            替换文本MAP key=原字符串 value=替换字符串
     */
    public static void replaceHeader(Sheet sheet, Map<String, Object> replaceTextMap) {
        if (replaceTextMap == null)
            return;

        // 遍历所有行
        for (Row thisRow : sheet) {
            boolean isFound = false;
            // 便利所有列
            float defaultRowHeight = thisRow.getHeightInPoints();
            float maxHeight = defaultRowHeight;
            for (Cell thisCell : thisRow) {

                // 获取单元格的类型
                CellReference cellRef = new CellReference(thisRow.getRowNum(), thisCell.getColumnIndex());
                switch (thisCell.getCellType()) {
                // 字符串
                case Cell.CELL_TYPE_STRING:
                    String targetText = thisCell.getRichStringCellValue().getString();

                    if (targetText != null && !targetText.trim().equals("") && replaceTextMap.containsKey(targetText)) {
                        String cellValue = replaceTextMap.get(targetText) == null ? "" : String.valueOf(replaceTextMap
                                .get(targetText));

                        float thisHeight = getExcelCellAutoHeight(cellValue, defaultRowHeight,
                                getMergedCellNum(thisCell) * sheet.getColumnWidth(thisCell.getColumnIndex()) / 128);
                        if (thisHeight > maxHeight)
                            maxHeight = thisHeight;
                        isFound = true;
                        thisCell.setCellValue(cellValue);
                        thisCell.getCellStyle().setWrapText(true);
                        log.info(" Sheet[" + sheet.getSheetName() + "]" + "行:" + (thisCell.getRowIndex() + 1) + "列:"
                                + getColLetter(thisCell.getColumnIndex()) + " " + targetText + " replace " + targetText
                                + " -> " + cellValue);
                    }

                    break;
                // 数字
                case Cell.CELL_TYPE_NUMERIC:
                    break;
                // boolean
                case Cell.CELL_TYPE_BOOLEAN:
                    break;
                // 方程式
                case Cell.CELL_TYPE_FORMULA:
                    break;
                // 空值
                default:
                }

            }

            if (isFound)
                thisRow.setHeightInPoints(maxHeight);
            
        }
        
    }

    /**
     * 替换文本
     * 
     * @param sheet
     *            工作表
     * @param key
     *            原字符串
     * @param value
     *            替换字符串
     */
    private static Cell findCell(Sheet sheet, String findStr) {
        // 遍历所有行
        for (Row row : sheet) {
            // 便利所有列
            for (Cell cell : row) {
                // 获取单元格的类型
                CellReference cellRef = new CellReference(row.getRowNum(), cell.getColumnIndex());
                switch (cell.getCellType()) {
                // 字符串
                case Cell.CELL_TYPE_STRING:
                    String targetText = cell.getRichStringCellValue().getString();

                    if (targetText != null && !targetText.trim().equals("")) {
                        if (targetText.contains(findStr))
                            return cell;
                    }

                    break;
                // 数字
                case Cell.CELL_TYPE_NUMERIC:
                    break;
                // boolean
                case Cell.CELL_TYPE_BOOLEAN:
                    break;
                // 方程式
                case Cell.CELL_TYPE_FORMULA:
                    break;
                // 空值
                default:
                }

            }
        }

        return null;

    }

    /**
     * 将列的索引换算成ABCD字母，这个方法要在插入公式时用到。
     * 
     * @param colIndex
     *            列索引。
     * @return ABCD字母。
     */
    private static String getColLetter(int colIndex) {
        String ch = "";
        if (colIndex < 26)
            ch = "" + (char) ((colIndex) + 65);
        else
            ch = "" + (char) ((colIndex) / 26 + 65 - 1) + (char) ((colIndex) % 26 + 65);
        return ch;
    }

    /**
     * 遍历文件夹下的文件，并打印文件夹下文件的路径
     * 
     * @param directoryPath
     *            文件夹目录
     */
    public static void traversal(List<String> filesPathList, String directoryPath) {
        File dir = new File(directoryPath);
        File[] files = dir.listFiles();
        if (files == null) {
            return;
        } else {
            for (int i = 0; i < files.length; i++) {
                if (files[i].isDirectory()) {
                    traversal(filesPathList, files[i].getAbsolutePath());
                } else {
                    filesPathList.add(files[i].getAbsolutePath());
                }
            }
        }
    }

    private static void copyRow(Workbook workbook, Sheet worksheet, int sourceRowNum, int destinationRowNum) {
        // Get the source / new row
        Row newRow = worksheet.getRow(destinationRowNum);
        Row sourceRow = worksheet.getRow(sourceRowNum);

        // If the row exist in destination, push down all rows by 1 else create a new row
        if (newRow != null) {
            worksheet.shiftRows(destinationRowNum, worksheet.getLastRowNum(), 1);
        } else {
            newRow = worksheet.createRow(destinationRowNum);
        }

        // Loop through source columns to add to new row
        for (int i = 0; i < sourceRow.getLastCellNum(); i++) {
            // Grab a copy of the old/new cell
            Cell oldCell = sourceRow.getCell(i);
            Cell newCell = newRow.createCell(i);

            // If the old cell is null jump to next cell
            if (oldCell == null) {
                newCell = null;
                continue;
            }

            // Copy style from old cell and apply to new cell
            CellStyle newCellStyle = workbook.createCellStyle();
            newCellStyle.cloneStyleFrom(oldCell.getCellStyle());
            newCell.setCellStyle(newCellStyle);

            // If there is a cell comment, copy
            if (newCell.getCellComment() != null) {
                newCell.setCellComment(oldCell.getCellComment());
            }

            // If there is a cell hyperlink, copy
            if (oldCell.getHyperlink() != null) {
                newCell.setHyperlink(oldCell.getHyperlink());
            }

            // Set the cell data type
            newCell.setCellType(oldCell.getCellType());

            // Set the cell data value
            switch (oldCell.getCellType()) {
            case Cell.CELL_TYPE_BLANK:
                newCell.setCellValue(oldCell.getStringCellValue());
                break;
            case Cell.CELL_TYPE_BOOLEAN:
                newCell.setCellValue(oldCell.getBooleanCellValue());
                break;
            case Cell.CELL_TYPE_ERROR:
                newCell.setCellErrorValue(oldCell.getErrorCellValue());
                break;
            case Cell.CELL_TYPE_FORMULA:
                newCell.setCellFormula(oldCell.getCellFormula());
                break;
            case Cell.CELL_TYPE_NUMERIC:
                newCell.setCellValue(oldCell.getNumericCellValue());
                break;
            case Cell.CELL_TYPE_STRING:
                newCell.setCellValue(oldCell.getRichStringCellValue());
                break;
            }
        }

        // If there are are any merged regions in the source row, copy to new row
        for (int i = 0; i < worksheet.getNumMergedRegions(); i++) {
            CellRangeAddress cellRangeAddress = worksheet.getMergedRegion(i);
            if (cellRangeAddress.getFirstRow() == sourceRow.getRowNum()) {
                CellRangeAddress newCellRangeAddress = new CellRangeAddress(newRow.getRowNum(),
                        (newRow.getRowNum() + (cellRangeAddress.getFirstRow() - cellRangeAddress.getLastRow())),
                        cellRangeAddress.getFirstColumn(), cellRangeAddress.getLastColumn());
                worksheet.addMergedRegion(newCellRangeAddress);
            }
        }
    }

    public static float getExcelCellAutoHeight(String str, float defaultRowHeight, int fontCountInline) {
        int defaultCount = 0;

        for (int i = 0; i < str.length(); i++) {
            int ff = getregex(str.substring(i, i + 1));
            defaultCount = defaultCount + ff;
        }
        if (defaultCount > fontCountInline) {
            return ((int) (defaultCount / fontCountInline) + 1) * defaultRowHeight;// 计算
        } else {
            return defaultRowHeight;
        }
    }

    public static int getregex(String charStr) {

        if (charStr == " ") {
            return 1;
        }
        // 判断是否为字母或字符
        if (Pattern.compile("^[A-Za-z0-9]+$").matcher(charStr).matches()) {
            return 1;
        }
        // 判断是否为全角

        if (Pattern.compile("[\u4e00-\u9fa5]+$").matcher(charStr).matches()) {
            return 2;
        }
        // 全角符号 及中文
        if (Pattern.compile("[^x00-xff]").matcher(charStr).matches()) {
            return 2;
        }
        return 1;

    }

    /**
     * @param args
     * @throws Exception
     */
    public static void main(String[] args) throws Exception {
    	
//        Map<String, Object> replaceTextMap = new HashMap<String, Object>();
//        replaceTextMap.put("寄单地址", "[111111111111111111111111]");
//        replaceTextMap.put("厂商编号", "[厂商编号厂商编号厂商编号厂商编号厂商编号厂商编号厂商编号厂商编号]");
//        replaceTextMap.put("厂商名称", "[厂商名称厂商编号厂商编号厂商编号厂商编号厂商编号厂商编号厂商编号]");
//
//        Workbook wb = readExcel("F:\\PO差异报表.xls");
//        Sheet sheet = wb.getSheetAt(0);
//        replaceHeader(sheet, replaceTextMap);
    	
    	
        List<Map<String, Object>> detailList = new ArrayList<Map<String, Object>>(3);
        Map<String, Object> detailMap1 = new TreeMap<String, Object>();
        detailMap1.put("D6LCRQ", "");
        detailList.add(detailMap1);
        
        Map<String, Object> detailMap2 = new TreeMap<String, Object>();
        detailMap2.put("D6LCRQ", "2");
        detailList.add(detailMap2);
        
        Map<String, Object> detailMap3 = new TreeMap<String, Object>();
        detailMap3.put("D6LCRQ", "3");
        detailList.add(detailMap3);
        
        for(String key:detailMap1.keySet()){
        	System.out.println(detailMap1.get(key).toString());
        }
//        Map<String, Object> detailMap = new TreeMap<String, Object>();
//        
//        
//        detailMap.put("ID11", "厂商编号");
//        detailMap.put("ID12", "厂商编号");
//        detailMap.put("ID13", "厂商编号厂商编号厂商编号厂商编号厂商编号厂商编号厂商编号");
//        detailMap.put("ID14", "ID14");
//        detailMap.put("ID15", "ID15");
//        detailMap.put("ID16", "ID16");
//        detailList.add(detailMap);
//        detailMap = new TreeMap<String, Object>();
//        detailMap.put("ID21", "ID21");
//        detailMap.put("ID22", "ID22");
//        detailMap.put("ID23", "ID23");
//        detailMap.put("ID24", "ID24");
//        detailMap.put("ID25", "ID25");
//        detailMap.put("ID26", "ID26");
//        detailList.add(detailMap);
//        replaceDetail(wb, sheet, "#detail", detailList);
//        replaceDetail(wb, sheet, "#detail1", detailList);
//        createExcel(wb, "F:\\PO差异报表_replace.xls");

    }

}
