package com.hola.bs.service.hht;

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

public class HHT_1047_03 extends BusinessService implements ProcessUnit{
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
			throw new RuntimeException();
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getRequest().getParameter("sto"));
		log.info("库存调整新增-明细查询, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		// TODO Auto-generated method stub
        String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
        
        String today = DateUtils.date2String(new Date());
        String startTime = bean.getRequest().getParameter(configpropertyUtil.getValue("starttime"));
        String sql = sqlpropertyUtil.getValue(store, "hht1047.03.01");
		Object[] o = new Object[]{today,bean.getUser().getName()};
		
		List<Map> gridList = jdbcTemplateUtil.searchForList(sql, o);
		if(gridList!=null&&gridList.size()>0){
			Config c = new Config("47", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
					+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
			XmlElement[] xml = new XmlElement[1];
			xml[0] = new XmlElement("detail", gridList);
			writerFile(c, xml, bean);
		}else{
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.1047.01"));

		}
	}
}
