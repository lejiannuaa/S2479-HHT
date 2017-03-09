package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

/**
 *  某项功能的RTV退货明细信息 
 * @author s1713 modify s2139
 *
 */
public class HHT_107_01  extends BusinessService implements ProcessUnit {
	


	public String process(Request request) {
		BusinessBean bean=new BusinessBean();
		try {
			 bean=resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。"+e.getMessage());
			log.error("", e);
		}
//		log.info("response=" + bean.getResponse().toString());
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht107.01RTV退货明细信息, response="+bean.getResponse().toString());

		// System.out.println("001_01:response="+response.toString());
		return  bean.getResponse().toString();
	}
	
	
	

	private void processData(BusinessBean bean) throws Exception{
		
		String store="";
		if(bean.getUser()!=null){
			store=bean.getUser().getStore();
		}else{
			store="13101";
		}
		
		List<Map> list=jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store,"hht107.01"),new Object[]{bean.getRequest().getParameter(configpropertyUtil.getValue("bc")), bean.getRequest().getParameter(configpropertyUtil.getValue("sku"))});
		Config c=new Config("0", "Server->Client：0",String.valueOf(bean.getRequest().getParameter("request"))+String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		XmlElement[] xml=new XmlElement[1];
		xml[0]=new XmlElement("info", list);
		writerFile(c, xml,bean);

		
	}
}
