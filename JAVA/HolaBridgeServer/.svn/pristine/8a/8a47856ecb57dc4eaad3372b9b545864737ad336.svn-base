package com.hola.bs.print;

import com.hola.bs.print.rmi.DynamicRmiClient;
import com.hola.bs.print.rmi.PrintServer;

public class Test {
    public static void main(String[] args) throws Exception {
        // init PrintTemplate
//       PrintTemplateFactory.init();
//        ApplicationContext ctx=new ClassPathXmlApplicationContext("spring.xml");
//        PrintTemplate.printByTemplateNo("1",new String[]{"1"});
//        Printer.printExcel("F:\\PO差异报表_replace.xls", PrintConfigUtil.getPrinter1Name());
//    	Log log = LogFactory.getLog(Test.class);
//    	log.error("test error",new Exception());
//    	log.info("testlog634500");
       	PrintServer pServer = new DynamicRmiClient().getRmiPrintClient("10.130.2.219", DynamicRmiClient.REMOTE_PRINT_NAME, DynamicRmiClient.REMOTE_PRINT_PORT);
        pServer.testRemotePrint();

    }
}
