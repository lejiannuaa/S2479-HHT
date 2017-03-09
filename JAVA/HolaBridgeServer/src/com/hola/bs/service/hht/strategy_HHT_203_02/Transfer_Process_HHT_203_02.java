package com.hola.bs.service.hht.strategy_HHT_203_02;

import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.print.template.PrintTemplate3;
import com.hola.bs.print.template.PrintTemplate4;

public class Transfer_Process_HHT_203_02 extends AbstractProcess_HHT_203_02 {
	
	//调拨差异
	@Autowired(required = true)
	private PrintTemplate3 printTemplate3;
	
	//调拨收货
	@Autowired(required = true)
	private PrintTemplate4 printTemplate4;

	public void executePrint(Map[] data, BusinessBean bean) throws Exception{
		for(int i=0;i<data.length;i++){
//			try{
			Map datamap = data[i];
			String po = String.valueOf(datamap.get("OddNum"));
			String file1 = printTemplate3.createReport(bean.getUser().getOwnerFilePath(), bean.getUser().getStore(),
					new String[] { po }, po);
			printServer.print(bean.getUser().getName() + "\\" + file1);
			
			String file2 = printTemplate4.createReport(bean.getUser().getOwnerFilePath(), bean.getUser().getStore(),
					new String[] { po }, po);
			printServer.print(bean.getUser().getName() + "\\" + file2);
//			throw new Exception();
		}
	}

}
