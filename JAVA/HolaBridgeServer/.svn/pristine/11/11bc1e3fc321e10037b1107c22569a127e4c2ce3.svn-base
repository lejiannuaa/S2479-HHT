package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.property.SQLPropertyUtil;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

public class HHT_202_01 extends BusinessService implements ProcessUnit {
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht202_01, response="+bean.getResponse().toString());
		log.info("send response: "+bean.getResponse().toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
//		String store = "";
//		if (bean.getUser() != null) {
//			store = bean.getUser().getStore();
//		} else {
//			store = "13101";
//		}

		String bc = String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("bc")));
		List<Map> boxinfoList = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht202.01.01"), new Object[] {bc});
		
		List<Map> bcinfoList = new ArrayList();
		List<Map> infoList = new ArrayList();
		if(boxinfoList != null && boxinfoList.size()>0){
			Map dataMap = boxinfoList.get(0);
			//String hhtcno = (String)dataMap.get("hhtcno");
			String hhtocd = (String)dataMap.get("HHTTYP");
			
			if(hhtocd.equals("RTV")){
				bcinfoList = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht202.01.02"), new Object[] {bc});
				List list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht202.01.03"), new Object[] {bc});
				infoList = list;
			}else{
				bcinfoList = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht202.01.04"), new Object[] {bc});
				infoList = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht202.01.05"), new Object[] {bc});
			}
		}
		Config c = new Config("0", "Server->Client：0", String
				.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil
						.getValue("op"))));
		XmlElement[] xml = new XmlElement[3];
		xml[0] = new XmlElement("bcinfo", bcinfoList);
		xml[1] = new XmlElement("boxinfo", boxinfoList);
		xml[2] = new XmlElement("info", infoList);
		
		writerFile(c, xml, bean);
	}
	
}
