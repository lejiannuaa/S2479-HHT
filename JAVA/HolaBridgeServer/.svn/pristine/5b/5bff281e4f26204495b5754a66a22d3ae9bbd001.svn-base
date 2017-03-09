package com.hola.bs.util;

import java.io.File;
import java.io.FileInputStream;
import java.util.ArrayList;
import java.util.List;

import org.apache.poi.hssf.usermodel.HSSFCell;
import org.apache.poi.hssf.usermodel.HSSFDateUtil;
import org.apache.poi.hssf.usermodel.HSSFRow;
import org.apache.poi.hssf.usermodel.HSSFSheet;
import org.apache.poi.hssf.usermodel.HSSFWorkbook;
import org.apache.poi.xssf.usermodel.XSSFCell;
import org.apache.poi.xssf.usermodel.XSSFRow;
import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;

public class XmlUtil {


	public static List<Object[]> getObjectByExcel(File excel) {
		List<Object[]> arrayDatas = new ArrayList<Object[]>();
		try {
			
				File file = excel;
				FileInputStream fileIs = new FileInputStream(file);
				XSSFWorkbook rwb = new XSSFWorkbook(fileIs);
				if (rwb.getNumberOfSheets() > 0) {
					XSSFSheet sheet = rwb.getSheetAt(0);
					int rows = sheet.getLastRowNum();

					// 固定开头列数
					XSSFRow rowlineTitle = sheet.getRow(0);
					short lastCellNumTitle = rowlineTitle.getLastCellNum();
					// i从1开始，跳过标题 循环行
					for (int rowNum = 1; rowNum <= rows; rowNum++) {
						XSSFRow rowline = sheet.getRow(rowNum);
						if (rowline != null) {
							Object[] data = new Object[lastCellNumTitle];
							for (int cellNum = 0; cellNum <= lastCellNumTitle - 1; cellNum++) {

								XSSFCell cell = rowline.getCell(cellNum);
								if (cell == null) {
									data[cellNum] = null;
								} else {

									switch (cell.getCellType()) {
									case HSSFCell.CELL_TYPE_NUMERIC:
										if (HSSFDateUtil.isCellDateFormatted(cell)) {
											data[cellNum] = cell.getDateCellValue();
										} else {
											data[cellNum] = cell.getNumericCellValue();
										}
										break;
									case HSSFCell.CELL_TYPE_STRING:
										data[cellNum] = cell.getStringCellValue();
										break;
									default:
										break;
									}
								}

							}
							arrayDatas.add(data);
						}
					}
				}
			
		} catch (Exception e) {
			try {
			
					File file = excel;
					FileInputStream fileIs = new FileInputStream(file);
					HSSFWorkbook rwb = new HSSFWorkbook(fileIs);
					if (rwb.getNumberOfSheets() > 0) {
						HSSFSheet sheet = rwb.getSheetAt(0);
						int rows = sheet.getLastRowNum();

						// 固定开头列数
						HSSFRow rowlineTitle = sheet.getRow(0);
						short lastCellNumTitle = rowlineTitle.getLastCellNum();
						// i从1开始，跳过标题 循环行
						for (int rowNum = 1; rowNum <= rows; rowNum++) {
							HSSFRow rowline = sheet.getRow(rowNum);
							if (rowline != null) {
								Object[] data = new Object[lastCellNumTitle];
								for (int cellNum = 0; cellNum <= lastCellNumTitle - 1; cellNum++) {

									HSSFCell cell = rowline.getCell(cellNum);
									if (cell == null) {
										data[cellNum] = null;
									} else {

										switch (cell.getCellType()) {
										case HSSFCell.CELL_TYPE_NUMERIC:
											if (HSSFDateUtil.isCellDateFormatted(cell)) {
												data[cellNum] = cell.getDateCellValue();
											} else {
												data[cellNum] = cell.getNumericCellValue();
											}
											break;
										case HSSFCell.CELL_TYPE_STRING:
											data[cellNum] = cell.getStringCellValue();
											break;
										default:
											break;
										}
									}

								}
								arrayDatas.add(data);
							}
						}
					}
				
			} catch (Exception e2) {
				e2.printStackTrace();
				return null;
			}
		}
		return arrayDatas;
	}
}
