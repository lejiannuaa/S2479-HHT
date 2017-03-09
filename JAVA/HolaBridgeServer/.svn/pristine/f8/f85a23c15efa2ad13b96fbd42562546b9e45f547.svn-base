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

/**
 * 初始化请求柜号下的商品明细
 * @author s2139
 * 2012 Sep 5, 2012 1:10:21 PM 
 */
public class HHT_1033_01 extends BusinessService implements ProcessUnit {

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
		log.info("盘点检核柜号下商品信息, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter("sto");
		String loc_no = bean.getRequest().getParameter("loc_no");
		String date = DateUtils.date2String2(new Date());
		String sql = sqlpropertyUtil.getValue(store, "hht1033.01.02");
		Object[] o = new Object[]{store,loc_no,date};
		
		List<Map> list = jdbcTemplateUtil.searchForList(sql, o);
		Config c = new Config("33", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		XmlElement[] xml = new XmlElement[1];
		xml[0] = new XmlElement("detail", list);
		writerFile(c, xml, bean);

	}
	
	
}
