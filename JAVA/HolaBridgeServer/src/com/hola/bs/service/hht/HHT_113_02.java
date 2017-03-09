package com.hola.bs.service.hht;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.remoting.RemoteLookupFailureException;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.print.rmi.PrintServer;
import com.hola.bs.print.template.PrintTemplate12;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;

public class HHT_113_02 extends BusinessService implements ProcessUnit{
	
	
	@Autowired(required = true)
	private PrintTemplate12 printTemplate12;

	public PrintTemplate12 getPrintTemplate12() {
		return printTemplate12;
	}

	public void setPrintTemplate12(PrintTemplate12 printTemplate12) {
		this.printTemplate12 = printTemplate12;
	}
	
	private PrintServer printServer;
	
	public PrintServer getPrintServer() {
		return printServer;
	}

	public void setPrintServer(PrintServer printServer) {
		this.printServer = printServer;
	}

	public String process(Request request) {
		// TODO Auto-generated method stub
		BusinessBean bean = new BusinessBean();
		
		try {
			
			bean = resolveRequest(request);
			printServer = super.createPrintClient(bean.getUser().getStore());
			String store = bean.getUser().getStore();
			String startDate = bean.getRequest().getParameter(configpropertyUtil.getValue("from"));
			String endDate = bean.getRequest().getParameter(configpropertyUtil.getValue("to"));
			startDate = DateUtils.date2StringDate(DateUtils.string2Date(startDate));
			endDate = DateUtils.date2StringDate(DateUtils.string2Date(endDate));
			String fileMettle = printTemplate12.createReport(bean.getUser().getOwnerFilePath(), 
					store, new String[]{startDate,endDate});
			printServer.print(bean.getUser().getName()+"\\"+fileMettle);
			Thread.sleep(1000);
			
		}catch(RemoteLookupFailureException rlfe){
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

}
