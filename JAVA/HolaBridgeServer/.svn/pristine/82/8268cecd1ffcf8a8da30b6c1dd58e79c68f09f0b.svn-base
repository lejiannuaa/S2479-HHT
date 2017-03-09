package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

/**
 * 请求显示具体出货商品的箱明细
 * @author S2139
 * 2012 Sep 6, 2012 1:27:41 PM 
 */
public class HHT_204_01 extends BusinessService implements ProcessUnit {

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
		log.info("send response: HHT_204_01,"+bean.getResponse().toString());
		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getUser()==null?"13101":bean.getUser().getStore();
		String sku = bean.getRequest().getParameter("sku");
		String bc = bean.getRequest().getParameter("bc");
		
		String sql = sqlpropertyUtil.getValue(store, "hht204.01.01");
		Object[] o = new Object[]{bc,sku};
		List<Map> list = jdbcTemplateUtil.searchForList(sql, o);
		Config c = new Config("0", "Server->Client：0", String
				.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil
						.getValue("op"))));
		XmlElement[] xml = new XmlElement[1];
		if(list!=null&&list.size()>0){
			xml[0] = new XmlElement("info", list);
			writerFile(c, xml, bean);
		}else{
			writerFile(c, null, bean);
		}
		
	}

}
