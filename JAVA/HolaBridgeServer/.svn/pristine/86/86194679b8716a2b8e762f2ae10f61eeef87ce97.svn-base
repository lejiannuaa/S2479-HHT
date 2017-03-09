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
 * 根据输入的箱号条码获取收货單列表信息
 * @author s1713 modify s2139
 * 
 */
public class HHT_001_01 extends BusinessService implements ProcessUnit {

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
		log.info("operation hht001.01根据输入的箱号条码获取收货單列表信息, response="+bean.getResponse().toString());
		// System.out.println("001_01:response="+response.toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		
		String store = bean.getUser().getStore();
		String bc = bean.getRequest().getParameter(configpropertyUtil.getValue("bc"));
		List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store,"hht001.01"), new Object[] {bc});
		List<Map> list2 = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store,"hht001.02"), new Object[] {bc});
		Config c = new Config("0", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));

		// 当用户进入时获得单据起点，To Location的值，并存入缓存中，（调拨专属）
		List<Map> list3 = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store,"getLocation2"), new Object[]{bc.toUpperCase()});
		
		//如果数据存在，则存入缓存
		if (list3 != null && list3.size() > 0 && list3.get(0) != null) {
			Map map = list3.get(0);
			bean.getUser().setAttribute("HHTFLC", map.get("HHTFLC").toString());
			bean.getUser().setAttribute("HHTTLC", map.get("HHTTLC").toString());
		}

		XmlElement[] xml = new XmlElement[2];
		xml[0] = new XmlElement("info", list);
		xml[1] = new XmlElement("detail", list2);
		writerFile(c, xml, bean);

	}
}
