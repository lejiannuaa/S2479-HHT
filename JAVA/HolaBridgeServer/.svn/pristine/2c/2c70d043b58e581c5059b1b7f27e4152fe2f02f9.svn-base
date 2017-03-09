package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

/**
 * 实物出货失败时明细信息的显示
 * @author S2139
 * 2013 Jan 4, 2013 2:55:21 PM 
 */
public class HHT_205_01 extends BusinessService {

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
		log.info("send response: HHT_205_01,实物出货失败原因显示。"+bean.getResponse().toString());
		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		String store = bean.getUser()==null?"13101":bean.getUser().getStore();
		String bc = bean.getRequest().getParameter("bc");
		String headsql = sqlpropertyUtil.getValue(store, "hht205.01.01");
		Object[] o = new Object[]{bc};
		List<Map> headList = jdbcTemplateUtil.searchForList(headsql, o);
		
		String detailsql = sqlpropertyUtil.getValue(store, "hht205.01.02");
		List<Map> detailList = jdbcTemplateUtil.searchForList(detailsql, o);
		
		Config c = new Config("0", "Server->Client：0", String
				.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil
						.getValue("op"))));
		XmlElement[] xml = new XmlElement[2];
		
		
		if(headList!=null&&headList.size()>0){
			xml[0] = new XmlElement("info", headList);
		}else{
			xml[0] = null;
		}
		
		if(detailList!=null&&detailList.size()>0){
			xml[1] = new XmlElement("detail", detailList);
		}else{
			xml[1] = null;
		}
		writerFile(c, xml, bean);
	}
}
