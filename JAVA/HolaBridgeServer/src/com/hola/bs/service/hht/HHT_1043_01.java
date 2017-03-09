package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;
import java.util.Properties;

import org.apache.commons.lang.StringUtils;
import org.apache.log4j.MDC;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

/**
 * 查询端架商品明细信息
 * @author S2139
 * 2012 Aug 30, 2012 4:32:25 PM 
 */
public class HHT_1043_01 extends BusinessService implements ProcessUnit {
	
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
		log.info("查询端架商品明细, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean)throws Exception {
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String groupId = bean.getRequest().getParameter(configpropertyUtil.getValue("groupId"));

		String sql = sqlpropertyUtil.getValue(store, "hht1043.01.01");
		Object[] o = new Object[]{store,groupId};
		List<Map> list = jdbcTemplateUtil.searchForList(sql, o);
		
		Config c = new Config("43", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		XmlElement[] xml = new XmlElement[1];
		if(list != null && list.size()>0){
			xml[0] = new XmlElement("detail", list);
			writerFile(c, xml, bean);
		}else{
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.1035.01"));
		}
	}
	

}
