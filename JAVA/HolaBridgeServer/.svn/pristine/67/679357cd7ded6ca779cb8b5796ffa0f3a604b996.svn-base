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
 * 他店库存查询
 * @author S2139
 * 2012 Aug 29, 2012 4:22:20 PM 
 */
public class HHT_1024_01 extends BusinessService implements ProcessUnit {

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
		log.info("商品的他店库存查询, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String sku = bean.getRequest().getParameter(configpropertyUtil.getValue("sku"));
		
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
		
		List skuInfoList = getSkuInfo(store,sku);
		Object[] o = new Object[]{store,sku,sku,sku};
		String sql = sqlpropertyUtil.getValue(store, "hht1024.01.01");
		List<Map> detailList = jdbcTemplateSomUtil.searchForList(sql, o);
		
		XmlElement[] xml = null;
		XmlElement[] finalXml = null;
		Config c = new Config("24", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		if(skuInfoList!=null&&skuInfoList.size()>0){
//			basicXml = new XmlElement("head",basicList);
			xml = new XmlElement[1];
			xml[0] = new XmlElement("info", skuInfoList);
			finalXml = xml;
		}
		if(detailList!=null&&detailList.size()>0){
			String bigStoreageSql = sqlpropertyUtil.getValue(store, "hht1024.01.02");
			List<Map> storeageList = jdbcTemplateUtil.searchForList(bigStoreageSql, new Object[]{sku,store});
			if(storeageList!=null&&storeageList.size()>0){
				detailList.add(storeageList.get(0));
			}
			if(xml == null){
				xml = new XmlElement[1];
				xml[0] = new XmlElement("detail", detailList);
				finalXml = xml;
			}else{
				finalXml = Arrays.copyOf(xml, xml.length+1);
				finalXml[xml.length] = new XmlElement("detail", detailList);
			}
		}
		writerFile(c, finalXml, bean);

	}

}
