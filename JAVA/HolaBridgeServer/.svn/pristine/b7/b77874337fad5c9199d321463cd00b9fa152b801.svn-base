package com.hola.bs.service.hht;

import org.apache.log4j.MDC;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.remoting.RemoteLookupFailureException;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.print.rmi.PrintServer;
import com.hola.bs.print.template.PrintTemplate4;
import com.hola.bs.service.BusinessService;

/**
 * 列印收货
 * @author S1608
 *
 */
public class HHT_002_03 extends BusinessService implements ProcessUnit {
	
	@Autowired(required = true)
	private PrintTemplate4 printTemplate4;
	
	private PrintServer printServer;
	
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			String bc = request.getParameter("bc");
			String file = printTemplate4.createReport(bean.getUser().getOwnerFilePath(), bean.getUser().getStore(),new String[]{bc}, bc);
			printServer = super.createPrintClient(bean.getUser().getStore());
			printServer.print(bean.getUser().getName()+"\\"+file);
		}catch(RemoteLookupFailureException rlfe){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("调用远程服务失败，请检查店铺"+ bean.getUser().getStore()+"的网络线路状况以及打印服务是否正常启动。");
		}catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht002.03, response="+bean.getResponse().toString());
//		log.info("response=" + bean.getResponse().toString());
		return bean.getResponse().toString();
	}

}
