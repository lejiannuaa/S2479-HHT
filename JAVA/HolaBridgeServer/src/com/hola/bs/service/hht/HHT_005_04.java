package com.hola.bs.service.hht;

import org.apache.log4j.MDC;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.remoting.RemoteLookupFailureException;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.print.rmi.PrintServer;
import com.hola.bs.print.template.PrintTemplate1;
import com.hola.bs.service.BusinessService;

/**
 * PO差异报表
 * @author S1608
 *
 */
public class HHT_005_04 extends BusinessService implements ProcessUnit {

	@Autowired(required = true)
	private PrintTemplate1 printTemplate1;

	private PrintServer printServer;

	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			String bc = request.getParameter("bc");
			while (bc.length() < 6) {
				bc = "0" + bc;
			}
			String file = printTemplate1.createReport(bean.getUser().getOwnerFilePath(), bean.getUser().getStore(),
					new String[] { bc }, bc);
			printServer = super.createPrintClient(bean.getUser().getStore());
			printServer.print(bean.getUser().getName() + "\\" + file);
		}catch(RemoteLookupFailureException rlfe){
			//如果是print端的异常（未开启print服务或者，print端未连接打印机等等情况）
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("调用远程服务失败，请检查店铺"+ bean.getUser().getStore()+"的网络线路状况以及打印服务是否正常启动");
		}
		catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
				//其他异常情况的一些处理（可能是开发人员代码造成）
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
		}
		
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht005.04打印PO差异单, response="+bean.getResponse().toString());

//		log.info("response=" + bean.getResponse().toString());
		return bean.getResponse().toString();
	}

	public static void main(String[] args) {
		// ApplicationContext ctx=new
		// ClassPathXmlApplicationContext("spring.xml");
		// new HHT_005_04().process("request=005;usr=test;op=04;bc=00001");
	}
}
