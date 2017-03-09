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
 * 复盘明细初始化：请求复盘数据
 * @author S2139
 * 2012 Aug 27, 2012 9:34:36 AM 
 */
public class HHT_601_01 extends BusinessService implements ProcessUnit {

	public String process(Request request) {
		// TODO Auto-generated method stub
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
		MDC.put("stoNo", bean.getRequest().getParameter(configpropertyUtil.getValue("sto")));
		log.info("获取复盘明细数据, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String userId = bean.getUser()!=null?bean.getUser().getName():"s2139";
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String stk_no = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_no"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
		String stk_second_type = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_second_type"));
		
		String sql = sqlpropertyUtil.getValue(store, "hht601.01.01");
		Object o[] = new Object[]{stk_no,loc_no,stk_second_type};
		List<Map> list = jdbcTemplateUtil.searchForList(sql, o);
		Config c = new Config("6", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		XmlElement[] xml = new XmlElement[1];
		if(list != null && list.size()>0){
			xml[0] = new XmlElement("detail", list);
			writerFile(c, xml, bean);
		}else{
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(
					configpropertyUtil.getValue("msg.601.01"));
		}
	}
}
