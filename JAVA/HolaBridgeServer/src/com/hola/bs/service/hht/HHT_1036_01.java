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
 * 下单建议商品详细信息检索
 * @author S2139
 * 2012 Aug 30, 2012 1:07:41 PM 
 */
public class HHT_1036_01 extends BusinessService implements ProcessUnit {

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
		log.info("检索下单建议商品信息, response="+bean.getResponse().toString());

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
		Object[] o = new Object[]{store,sku};
		String sqlBasic = sqlpropertyUtil.getValue(store, "hht1036.01.01");
		List<Map> basicList = jdbcTemplateUtil.searchForList(sqlBasic, o);
		List<Map> poOnTheWay = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store, "hht1021.01.02"), new Object[]{sku,store});
		List<Map> trfOnTheWay = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store, "hht1021.01.03"), new Object[]{sku,store});
		if(basicList!=null&&basicList.size()>0){
			if(poOnTheWay!=null && poOnTheWay.size()>0){
				basicList.get(0).put("po_on_the_way", poOnTheWay.get(0).get("po_on_the_way").toString());
			}else{
				basicList.get(0).put("po_on_the_way", "0");
			}
			if(trfOnTheWay!=null && trfOnTheWay.size()>0){
				basicList.get(0).put("trf_on_the_way", trfOnTheWay.get(0).get("trf_on_the_way").toString());
//				detailList.addAll(trfOnTheWay);
			}else{
				basicList.get(0).put("trf_on_the_way", "0");
			}

			Config c = new Config("36", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
					+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
			XmlElement[] xml = new XmlElement[1];
			xml[0] = new XmlElement("detail", basicList);
			writerFile(c, xml, bean);
		}else{
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.1035.01"));
		}
	}

	
}
