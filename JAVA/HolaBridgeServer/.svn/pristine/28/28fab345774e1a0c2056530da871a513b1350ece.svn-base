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
 * 显示所有的退货原因（在下拉框）
 * @author s1713
 * 
 */
public class HHT_102_01 extends BusinessService implements ProcessUnit {

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
		List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),
				"hht102.01.01"), new Object[] {});
		Config c = new Config("0", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		XmlElement[] xml = new XmlElement[1];
		xml[0] = new XmlElement("info", list);

		writerFile(c, xml, bean);

	}
}
