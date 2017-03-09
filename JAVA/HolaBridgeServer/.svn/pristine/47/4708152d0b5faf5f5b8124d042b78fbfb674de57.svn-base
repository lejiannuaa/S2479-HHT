package com.hola.bs.service.hht;

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
 * 请求获取端架基本信息
 * @author s2139
 * 2012 Sep 5, 2012 1:47:46 PM 
 */
public class HHT_1041_02 extends BusinessService implements ProcessUnit {

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
		MDC.put("stoNo", bean.getRequest().getParameter("sto"));
		log.info("请求获取端架基本信息, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter("sto");
		String groupId = bean.getRequest().getParameter("groupId");
		
		String sql = sqlpropertyUtil.getValue(store, "hht1041.01.03");
		Object[] o = new Object[]{store, groupId};
		List<Map> list = jdbcTemplateUtil.searchForList(sql, o);
		if(list!=null&&list.size()>0){
			Map map = list.get(0);
			String beginDate = map.get("start_valid_date").toString();
			beginDate = DateUtils.date2String(DateUtils.string2Date(beginDate, DateUtils.ID_YYYYMMDD));
			map.put("start_valid_date", beginDate);
			
			String endDate = map.get("end_valid_date").toString();
			endDate = DateUtils.date2String(DateUtils.string2Date(beginDate, DateUtils.ID_YYYYMMDD));
			map.put("end_valid_date", endDate);
			
			Config c = new Config("44", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
					+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
			XmlElement[] xml = new XmlElement[1];
			xml[0] = new XmlElement("detail", list);
			writerFile(c, xml, bean);
		}else{
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.1044.01"));
		}

		
	}

}
