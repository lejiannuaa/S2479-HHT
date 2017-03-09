package com.hola.bs.service.hht;

import java.util.Date;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.remoting.RemoteLookupFailureException;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.print.rmi.PrintServer;
import com.hola.bs.print.template.PrintTemplate10;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;

public class HHT_1047_04 extends BusinessService implements ProcessUnit{

	@Autowired(required = true)
	private PrintTemplate10 printTemplate10;
	


	public PrintTemplate10 getPrintTemplate10() {
		return printTemplate10;
	}

	public void setPrintTemplate10(PrintTemplate10 printTemplate10) {
		this.printTemplate10 = printTemplate10;
	}

	private PrintServer printServer;

	public String process(Request request) {
		// TODO Auto-generated method stub
		
		BusinessBean bean = new BusinessBean();
		
		try {
			bean = resolveRequest(request);
			printServer = super.createPrintClient(bean.getUser().getStore());
			String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
			String today = DateUtils.date2String(new Date());
			String[] fileMettle = printTemplate10.createReportList(
					bean.getUser().getOwnerFilePath(), 
					store,
					new String[]{store},today,bean.getUser().getName());
			
			for(int i=0;i<fileMettle.length;i++){
				printServer.print(bean.getUser().getName()+"\\"+fileMettle[i]);
			}

				Thread.sleep(1000);
			
		} catch(RemoteLookupFailureException rlfe){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("调用远程服务失败，请检查店铺"+ bean.getUser().getStore()+"的网络线路状况以及打印服务是否正常启动。");
		} catch (Exception e) {
			// TODO Auto-generated catch block
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e);
			log.error("", e);
		}
		return bean.getResponse().toString();
	}
	
	public PrintServer getPrintServer() {
		return printServer;
	}

	public void setPrintServer(PrintServer printServer) {
		this.printServer = printServer;
	}

}
