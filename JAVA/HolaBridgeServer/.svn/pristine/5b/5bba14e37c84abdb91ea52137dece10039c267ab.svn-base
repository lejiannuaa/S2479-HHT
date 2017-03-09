package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.property.PullDownUtils;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

/**
 * 国际条码查询
 * @author S2139
 * 2012 Aug 29, 2012 11:43:52 AM 
 */
public class HHT_1012_01 extends BusinessService implements ProcessUnit {

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
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("营业课--价格查询--商品国际条码, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getUser().getStore();
		String sku = bean.getRequest().getParameter(configpropertyUtil.getValue("sku"));
		
		String sql = sqlpropertyUtil.getValue(store, "hht1012.01.01");
		List<Map> detailList = jdbcTemplateUtil.searchForList(sql, new Object[]{sku,sku,sku});
		if(detailList!=null&&detailList.size()>0){
			Config c = new Config("11", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
					+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
			XmlElement[] xml = new XmlElement[1];
			xml[0] = new XmlElement("detail", detailList);
			writerFile(c, xml, bean);
		}else {
			bean.getResponse().setCode(BusinessService.warncode);
			// response.setDesc(configpropertyUtil.getValue("msg.02"));
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.1011.01"));
		
		}
	}

}
