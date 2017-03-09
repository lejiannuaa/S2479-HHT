package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

/**
 * PO收货检索
 * @author s1713
 * 
 */
public class HHT_004_01 extends BusinessService implements ProcessUnit {

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
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		String status = String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("state")));
		List<Map> list = null;
		String bc = String.valueOf(bean.getRequest().getParameter("bc"));
        while(bc.length()<6){
			
			bc = "0"+bc;
			
		}
		String sql = sqlpropertyUtil.getValue(bean.getUser().getStore(), "hht004.01");
		Object o[] = null;
		String user = String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("opusr")));

		if (status == null || status.equals("")) {
			status = "%";
		}
		if (user == null || user.equals("")) {
			user = "%";
		}
		if (bc == null || bc.equals("")) {
			bc = "%";
		}
		o = new Object[] { bean.getRequest().getParameter(configpropertyUtil.getValue("from")),
				bean.getRequest().getParameter(configpropertyUtil.getValue("to")), user, status, bc };
		
		list = jdbcTemplateUtil.searchForList(sql, o);
		Config c = new Config("0", "Server->Client:0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		XmlElement[] xml = new XmlElement[1];
		xml[0] = new XmlElement("info", list);

		writerFile(c, xml, bean);

	}
}
