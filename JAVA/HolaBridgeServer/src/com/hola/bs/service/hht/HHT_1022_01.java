package com.hola.bs.service.hht;

import java.util.Arrays;
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
 * 在途PO查询
 * @author S2139
 * 2012 Aug 29, 2012 2:30:01 PM 
 */
public class HHT_1022_01 extends BusinessService implements ProcessUnit {

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
		log.info("商品库存检索，在途PO信息, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String sku = bean.getRequest().getParameter(configpropertyUtil.getValue("sku"));
		if(sku.length()>SKU_LENGTH){
			sku = super.tranUPCtoSKU(store, sku);
		}else{
			sku = super.fullSKU(sku);
		}
		Object[] o = new Object[]{store,sku,sku,sku};
		String basicInfoSql = sqlpropertyUtil.getValue(store, "hht1022.01.01");
		List<Map> basicList = jdbcTemplateUtil.searchForList(basicInfoSql, o);
		String detailSql = sqlpropertyUtil.getValue(store, "hht1022.01.02");
//		detailSql = replaceSqlPram(detailSql, o);
		List<Map> detailList = jdbcTemplateUtil.searchForList(detailSql, o);
//		List<Map> detailList = jdbcTemplateUtil.searchForList(detailSql);
		XmlElement[] xml = null;
		XmlElement[] finalXml = null;
		Config c = new Config("10", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
//		XmlElement basicXml = null;
		if(basicList!=null&&basicList.size()>0){
//			basicXml = new XmlElement("head",basicList);
			xml = new XmlElement[1];
			xml[0] = new XmlElement("head", basicList);
		}
		//if(detailList!=null&&detailList.size()>0){
			if(xml == null){
				xml = new XmlElement[1];
				xml[0] = new XmlElement("detail", detailList);
				finalXml = xml;
			}else{
				finalXml = Arrays.copyOf(xml, xml.length+1);
				finalXml[xml.length] = new XmlElement("detail", detailList);
			}
		//}
		writerFile(c, finalXml, bean);
	}

}
