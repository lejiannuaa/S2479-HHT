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
 * 采购调拨检索界面
 * @author s1713
 * 
 */
public class HHT_106_01 extends BusinessService implements ProcessUnit {

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
//		log.info("response=" + bean.getResponse().toString());
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht106.01, response="+bean.getResponse().toString());

		// System.out.println("001_01:response="+response.toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		String store = "";
		if (bean.getUser() != null) {
			store = bean.getUser().getStore();
		} else {
			store = "13101";
		}

		List<Map> condition = jdbcTemplateUtil.searchForList(sqlpropertyUtil
				.getValue(bean.getUser().getStore(),"hht106.01.search"), new Object[] {
				bean.getRequest().getParameter(configpropertyUtil.getValue("bc"))});
		if (condition.size() < 1) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.03"));
		} else {
			Map map = condition.get(0);
			String hhttyp = map.get("HHTTYP").toString();
			String hhtsts = map.get("HHTSTS").toString();
			
			if(hhtsts.equals("3") || hhtsts.equals("0")){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(configpropertyUtil.getValue("msg.106.01"));
				return;
			}
			
			XmlElement[] xml = new XmlElement[2];
			Config c = new Config("0", "Server->Client：0", String
					.valueOf(bean.getRequest().getParameter("request"))
					+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil
							.getValue("op"))));
			
			String bc = bean.getRequest().getParameter(configpropertyUtil.getValue("bc"));

			if(hhttyp.equalsIgnoreCase("RTV")){
				List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil
						.getValue(bean.getUser().getStore(),"hht106.01.01_head"), new Object[] {
					bc,
					bc });		
				xml[0] = new XmlElement("bcinfo", list);

				List<Map> list2 = jdbcTemplateUtil.searchForList(sqlpropertyUtil
						.getValue(bean.getUser().getStore(),"hht106.01.01_detail"), new Object[] {
					bc,
					bc,bc,bc });
				xml[1] = new XmlElement("info", list2);			
			}else{
				List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil
						.getValue(bean.getUser().getStore(),"hht106.01.02_head"), new Object[] {
					bc,
					bc });		
				xml[0] = new XmlElement("bcinfo", list);

				List<Map> list2 = jdbcTemplateUtil.searchForList(sqlpropertyUtil
						.getValue(bean.getUser().getStore(),"hht106.01.02_detail"), new Object[] {
					bc,
					bc,
					bc,
					bc});
				xml[1] = new XmlElement("info", list2);	
			}
			
			writerFile(c, xml, bean);
		}
	}
}
