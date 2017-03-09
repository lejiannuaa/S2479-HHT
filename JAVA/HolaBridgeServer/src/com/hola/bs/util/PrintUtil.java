package com.hola.bs.util;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;

import javax.print.Doc;
import javax.print.DocFlavor;
import javax.print.DocPrintJob;
import javax.print.PrintException;
import javax.print.PrintService;
import javax.print.PrintServiceLookup;
import javax.print.ServiceUI;
import javax.print.SimpleDoc;
import javax.print.attribute.HashPrintRequestAttributeSet;
import javax.print.attribute.PrintRequestAttributeSet;
import javax.print.attribute.standard.Copies;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;



/** 
* 打印图片的类 
* 
*/ 
public class PrintUtil {
	
	private Log log = LogFactory.getLog(PrintUtil.class);
	/**
	 * 画图片的方法
	 * 
	 * @param fileName
	 *            [图片的路径]
	 * @throws PrintException 
	 * @throws IOException 
	 */
	public void printImage(String fileName,String devName) throws PrintException, IOException {
			DocFlavor dof = null;
			// 根据用户选择不同的图片格式获得不同的打印设备
			if (fileName.toLowerCase().endsWith(".gif")) {
				// gif
				dof = DocFlavor.INPUT_STREAM.GIF;
			} else if (fileName.toLowerCase().endsWith(".jpg")) {
				// jpg
				dof = DocFlavor.INPUT_STREAM.JPEG;
			} else if (fileName.toLowerCase().endsWith(".png")) {
				// png
				dof = DocFlavor.INPUT_STREAM.PNG;
			}else {
				// 非gif,jpg,png格式终止程序
				log.error("error Image file type, support gif,jpg,png only!");
				return;
			}
			
			// 字节流获取图片信息
			FileInputStream fin = new FileInputStream(fileName);
			// 获得打印属性
			PrintRequestAttributeSet pras = new HashPrintRequestAttributeSet();
			// 每一次默认打印一页
			pras.add(new Copies(1));
			
			// 获得打印设备 ，字节流方式，图片格式
			PrintService pss[] = PrintServiceLookup.lookupPrintServices(dof,
					pras);
			// 如果没有获取打印机
			if (pss.length == 0) {
				// 终止程序
				log.error("No print device found!");
				return;
			}
			// 获取打印机
			PrintService ps =null;
			for(int i=0;i<pss.length;i++){
				if (pss[i].getName().toLowerCase().indexOf(devName.toLowerCase())>=0) {
					ps = pss[i];
					break;
				}
			}
			if (ps==null){
				// 没找到指定的打印机，终止程序
				log.error("could not found print device: "+devName);
				return;
			}
			
			log.info("Printing image ["+fileName+"] at " + ps);
			// 获得打印工作
			DocPrintJob job = ps.createPrintJob();

			// 设置打印内容
			Doc doc = new SimpleDoc(fin, dof, null);
			// 出现设置对话框
//			PrintService service = ServiceUI.printDialog(null, 50, 50, pss, ps,
//					dof, pras);
			
			if (ps != null) {
				// 开始打印
				job.print(doc, pras);
				fin.close();
			}
			log.info("Print image ["+fileName+"] at " + ps+" sucess.");

	}

	/**
	 * 主函数
	 * 
	 * @param args
	 * 
	 */
	public static void main(String args[]) {
		PrintUtil dp = new PrintUtil();
		try {
			dp.printImage("d:/IMG_0455.jpg","c4300-3f-right");
		} catch (PrintException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
