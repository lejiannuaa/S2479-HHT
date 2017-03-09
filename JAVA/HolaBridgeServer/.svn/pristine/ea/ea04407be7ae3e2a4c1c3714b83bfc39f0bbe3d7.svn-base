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
 * 调拨采购--商品检索
 * @author s1713 modify s2139
 * 
 */
public class HHT_105_01 extends BusinessService implements ProcessUnit {

	public String process(Request request) {
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
		log.info("operation hht105.01调拨采购--商品检索, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		String store = bean.getUser().getStore();
		String sku = bean.getRequest().getParameter(configpropertyUtil.getValue("sku"));
		String hhtno = bean.getRequest().getParameter(configpropertyUtil.getValue("bc"));
		
		if(sku.length()>UPC_LENGTH_TWELVE){
			sku = tranUPCtoSKU(store, sku);
		}else if(sku.length()>SKU_LENGTH && sku.length()<=UPC_LENGTH_TWELVE){
			sku = tranUPCtoSKUforLengthNinetoEleven(store, sku);
		}else if(sku.length() < IPC_LENGTH){
			sku = fullSKU(sku);
		}else if(sku.length() == IPC_LENGTH ){//ipc转换成SKU\
			
			if(sku.substring(0, 1).equals("2")){
				String sql = sqlpropertyUtil.getValue(store, "hht1011.01.04");
				List<Map> skuList = jdbcTemplateUtil.searchForList(sql,new Object[]{sku});
				if(skuList!=null&&skuList.size()>0){
					sku = skuList.get(0).get("hhtsku").toString();
				}
			}else{
				String sql = sqlpropertyUtil.getValue(store, "hht1011.01.00");
				List<Map> skuList = jdbcTemplateUtil.searchForList(sql,new Object[]{sku,sku,fullUPCTwelve(sku),fullUPCThirteen(sku)});
				if(skuList!=null&&skuList.size()>0){
					sku = skuList.get(0).get("hhtsku").toString();
				}
			}
		}
			

		List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),
				"hht105.01.01"), new Object[] { sku, sku, sku, store });
		
		Config c = new Config("0", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		XmlElement[] xml = new XmlElement[1];
		xml[0] = new XmlElement("info", list);

		writerFile(c, xml, bean);

		bean.getResponse().setCode(BusinessService.errorcode);
		bean.getResponse().setDesc("非此单据内商品，请确认");
		
		List<Map> hhtskulist = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),
				"hht105.01.02"), new Object[] { hhtno });
		
		if(hhtskulist!=null&&hhtskulist.size()>0){
			for(int i=0;i<hhtskulist.size();i++)
			{
				if(sku.equals(hhtskulist.get(i).get("hhtsku").toString()))
				{
					bean.getResponse().setCode(BusinessService.successcode);
					bean.getResponse().setDesc(null);
					break;
				}
			}
		}
	}
}
