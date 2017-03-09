package com.hola.bs.service.hht;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.remoting.RemoteLookupFailureException;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.core.exception.Template6Exception;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.print.rmi.PrintServer;
import com.hola.bs.print.template.PrintTemplate6;
import com.hola.bs.service.BusinessService;

/**
 * 打印整箱明细清单
 * @author S1608
 *
 */
public class HHT_104_04 extends BusinessService implements ProcessUnit{

	private PrintServer printServer;
	
	@Autowired(required = true)
	private PrintTemplate6 printTemplate6;
	
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			String bc = request.getParameter("bc");
			String fileName = printTemplate6.createReport(bean.getUser().getOwnerFilePath(), bean.getUser().getStore(),new String[]{bc}, bc);
			printServer = super.createPrintClient(bean.getUser().getStore());
			printServer.print(bean.getUser().getName() + "\\" + fileName);
		} catch (Template6Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("未取得调入店信息。");
//			log.error("", e);
		} catch(RemoteLookupFailureException rlfe){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("调用远程服务失败，请检查店铺"+ bean.getUser().getStore()+"的网络线路状况以及打印服务是否正常启动。");
//			log.error("", rlfe);
		}catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
		}
		return bean.getResponse().toString();
	}

}
