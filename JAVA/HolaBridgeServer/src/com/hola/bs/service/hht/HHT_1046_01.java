package com.hola.bs.service.hht;

import java.math.BigDecimal;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

public class HHT_1046_01 extends BusinessService implements ProcessUnit {
	
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
		log.info("下单申请新增--查询, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
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
		Object[] o = new Object[]{sku,store};
		String sql = sqlpropertyUtil.getValue(store, "hht1046.01.01");
		sql = replaceSqlPram(sql,new Object[]{sku,store});
		List<Map> detailList = jdbcTemplateUtil.searchForList(sql);

		String sqlSom = sqlpropertyUtil.getValue(store, "htt1021.01.04");
		Object[] oSom = new Object[]{store,sku};
		sqlSom = replaceSqlPram(sqlSom,oSom);
		List<Map> storageList = jdbcTemplateSomUtil.searchForList(sqlSom);

		Object[] o2 = new Object[]{sku,sku,sku,store};
		String sql2 = sqlpropertyUtil.getValue(store, "hht1021.01.02");
		sql2 = replaceSqlPram(sql2,o2);
//		String sql2 = "select '23' as po_on_the_way";
		List<Map> poOnTheWay = jdbcTemplateUtil.searchForList(sql2);

//		List<Map> poOnTheWay = jdbcTemplateUtil.searchForList(sql2);
		Object[] o3 = new Object[]{sku,sku,sku,store};
		String sql3 = sqlpropertyUtil.getValue(store, "hht1021.01.03");
		sql3 = replaceSqlPram(sql3,o3);
		List<Map> trfOnTheWay = jdbcTemplateUtil.searchForList(sql3);
		if(detailList!=null&&detailList.size()>0){
			
			
			if(detailList.get(0).get("vtn_type_desc").equals("大仓")){
				if((detailList.get(0).get("hhtvf5")!=null)){
					if(!detailList.get(0).get("hhtvf5").equals("Y")){
						bean.getResponse().setCode(BusinessService.errorcode);
						bean.getResponse().setDesc(configpropertyUtil.getValue("msg.1046.01"));
					}
				}else{
					bean.getResponse().setCode(BusinessService.errorcode);
					bean.getResponse().setDesc(configpropertyUtil.getValue("msg.1046.01"));
				}


			}
			
			
			if(poOnTheWay!=null && poOnTheWay.size()>0){
				detailList.get(0).put("po_on_the_way", poOnTheWay.get(0).get("po_on_the_way").toString());
			}else{
				detailList.get(0).put("po_on_the_way", "0");
			}
			
			
			if(trfOnTheWay!=null && trfOnTheWay.size()>0){
				detailList.get(0).put("trf_on_the_way", trfOnTheWay.get(0).get("trf_on_the_way").toString());
//				detailList.addAll(trfOnTheWay);
			}else{
				detailList.get(0).put("trf_on_the_way", "0");
			}
			
			if(storageList!=null&&storageList.size()>0){
				detailList.get(0).put("inv_no", (((BigDecimal)detailList.get(0).get("inv_no")).intValue()-((BigDecimal)storageList.get(0).get("SALES_QTY")).intValue()));
				detailList.get(0).put("c_week_sale", ((BigDecimal)(detailList.get(0).get("c_week_sale"))).intValue()+((BigDecimal)storageList.get(0).get("SALES_QTY")).intValue());
			}else{
				detailList.get(0).put("c_week_sale", ((BigDecimal)(detailList.get(0).get("c_week_sale"))).intValue());
			}
			
			detailList.get(0).put("l_week_sale",((BigDecimal)(detailList.get(0).get("l_week_sale"))).intValue());
			Config c = new Config("46", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
					+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
			XmlElement[] xml = new XmlElement[1];
			xml[0] = new XmlElement("detail", detailList);
			writerFile(c, xml, bean);
		}else{
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.1011.01"));

		}
	}
}
