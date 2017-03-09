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
 * 检索初盘柜号
 * @author S2139
 * 2012 Aug 22, 2012 5:55:13 PM 
 */
public class HHT_500_02 extends BusinessService implements ProcessUnit {

	
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
		log.info("检索初盘柜号, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception{
		String userId = bean.getUser() != null?bean.getUser().getName():"s2139";
		String stk_no = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_no"));
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String sql = sqlpropertyUtil.getValue(store,"hht500.00.02");
		Object o[] = new Object[2];
		o[0] = stk_no;
		o[1] = userId;//参数二 盘点单号
		List<Map> list = jdbcTemplateUtil.searchForList(sql, o);
		Config c = new Config("5", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		XmlElement[] xml = new XmlElement[1];
		if(list != null && list.size()>0){
			xml[0] = new XmlElement("detail", list);
			writerFile(c, xml, bean);
		}else{
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(
					configpropertyUtil.getValue("msg.500.02"));
		}
	}
}
