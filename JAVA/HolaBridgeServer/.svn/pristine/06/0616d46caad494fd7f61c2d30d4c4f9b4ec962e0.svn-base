package com.hola.bs.print;

import java.awt.print.PrinterJob;

import javax.print.PrintService;

import com.hola.bs.print.template.PrintTemplate;
import com.jacob.activeX.ActiveXComponent;
import com.jacob.com.ComThread;
import com.jacob.com.Dispatch;
import com.jacob.com.Variant;

/**
 * 打印类
 * 
 * @author roy
 * 
 */
public class Printer{

    public static String print(PrintTemplate template, String sqlParams[], String... detailsqlParams) throws Exception {
        // template.getPrintData();

//        try {
            printByPname(template.getPrinterName(), template,sqlParams,detailsqlParams);
//        } catch (Exception e) {
//            e.printStackTrace();
//            return template.getPrinterName() + " error:" + e.getMessage();
//        }

        return template.getPrinterName() + " printed";
    }


    /**
     * 打印Excel文件
     * 
     * @param path
     *            Excel文件路径
     * @throws Exception 
     */
    public static void printExcel(String path, String printerName) throws Exception {
        ComThread.InitSTA();
        ActiveXComponent xl = new ActiveXComponent("Excel.Application");
        try {

            Dispatch workbooks = xl.getProperty("Workbooks").toDispatch();

            Dispatch workbook = Dispatch.call(workbooks, "Open", path).toDispatch();

            Dispatch.callN(workbook, "PrintOut", new Object[] { Variant.VT_MISSING, Variant.VT_MISSING, new Integer(1),
                    new Boolean(false), printerName, new Boolean(true), Variant.VT_MISSING, "" });
            Dispatch.call(workbook, "Close");

        } catch (Exception e) {
            e.printStackTrace();
            throw e;
        } finally {
            // 始终释放资源
            xl.invoke("Quit", new Variant[] {});
            ComThread.Release();
        }
    }

    /**
     * 实际的打印方法
     * 
     * @param printerName
     * @param template
     * @throws Exception 
     */
    protected static void printByPname(String printerName, PrintTemplate template, String sqlParams[], String... detailsqlParams) throws Exception {
        PrintService[] pss = PrinterJob.lookupPrintServices();
        
        PrintService ps = null;
        for (PrintService p : pss) {
            System.out.println(p.getName());
            // 如果打印机名称相同
            if (p.getName().equalsIgnoreCase(printerName)) {
                ps = p;
                break;
            }
        }
        // 未找到指定打印机，返回默认打印机
        if (ps == null) {
            ps = PrinterJob.getPrinterJob().getPrintService();
        }
        
        if (ps == null) {
            throw new Exception("printer no found");
        }
        
//        printExcel(template.createReport(sqlParams,detailsqlParams), ps.getName());
    }
}
