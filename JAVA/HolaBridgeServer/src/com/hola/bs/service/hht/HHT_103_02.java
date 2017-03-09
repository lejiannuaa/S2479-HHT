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
 * 出货到大仓商品检索
 * @author s1713 modify s2139
 * 
 */
public class HHT_103_02 extends BusinessService implements ProcessUnit {

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
		log.info("operation hht103.02出货到大仓商品检索, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {

		String store = bean.getUser() != null?bean.getUser().getStore():"13101";
		
		String sku = bean.getRequest().getParameter(configpropertyUtil.getValue("bc"));
		if (sku.length() <= 9)
			sku = super.fullSKU(sku);
		else
			sku = super.tranUPCtoSKU(bean.getUser().getStore(), sku);

		System.out.println("store is " + store);
		List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil
				.getValue(bean.getUser().getStore(), "hht103.02"), new Object[] { store, sku });
		Config c = new Config("0", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		XmlElement[] xml = new XmlElement[1];
		xml[0] = new XmlElement("info", list);
		writerFile(c, xml, bean);

	}
}
