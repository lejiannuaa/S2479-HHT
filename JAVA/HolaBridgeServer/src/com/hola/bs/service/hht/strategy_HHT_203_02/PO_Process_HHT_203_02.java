package com.hola.bs.service.hht.strategy_HHT_203_02;

import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.print.template.PrintTemplate1;
import com.hola.bs.print.template.PrintTemplate2;

public class PO_Process_HHT_203_02 extends AbstractProcess_HHT_203_02 {
	
	//PO差异报表
	@Autowired(required = true)
	private PrintTemplate1 printTemplate1;
	
	//PO收货报表
	@Autowired(required = true)
	private PrintTemplate2 printTemplate2;
	
	public void executePrint(Map[] data, BusinessBean bean) throws Exception{
		for(int i=0;i<data.length;i++){
//			try{
				Map datamap = data[i];
				String po = String.valueOf(datamap.get("OddNum"));
				String file1 = printTemplate1.createReport(bean.getUser().getOwnerFilePath(), bean.getUser().getStore(),
						new String[] { po }, po);
				printServer.print(bean.getUser().getName() + "\\" + file1);
				
				String file2 = printTemplate2.createReport(bean.getUser().getOwnerFilePath(), bean.getUser().getStore(),
						new String[] { po }, po);
				printServer.print(bean.getUser().getName() + "\\" + file2);
//			}catch()
		}
	}

}
