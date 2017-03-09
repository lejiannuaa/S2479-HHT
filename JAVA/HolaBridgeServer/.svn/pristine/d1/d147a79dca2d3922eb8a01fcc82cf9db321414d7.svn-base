package com.hola.bs.service.hht;

import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.DateUtils;
import com.hola.bs.util.XmlElement;

/**
 * 初盘首页初盘柜号检索
 * @author S2139
 * 2012 Aug 22, 2012 3:45:03 PM 
 */
public class HHT_500_01 extends BusinessService implements ProcessUnit {

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
		log.info("初盘首页初盘柜号检索, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}
	
	private void processData(BusinessBean bean) throws Exception{
		String store = bean.getRequest().getParameter(configpropertyUtil
				.getValue("sto"));
		String sql = sqlpropertyUtil.getValue(store,"hht500.00.01");
		Object o[] = new Object[4];
		Date date1 = new Date();
		o[0] = store;//参数一 店号
		o[1] = DateUtils.date2String2(date1);
		o[2] = store;
		Calendar date2 = Calendar.getInstance();
		date2.setTime(date1);
		date2.set(Calendar.DATE, date2.get(Calendar.DATE) - 1);
		o[3] = DateUtils.date2String2(date2.getTime());//参数二 时间
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
					configpropertyUtil.getValue("msg.500.01"));
		}
	}
}
