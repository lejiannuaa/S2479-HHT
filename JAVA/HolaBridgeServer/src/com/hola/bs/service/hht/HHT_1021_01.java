package com.hola.bs.service.hht;

import java.math.BigDecimal;
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
 * 库存检索--商品基本信息
 * @author S2139
 * 2012 Aug 29, 2012 1:30:33 PM 
 */
public class HHT_1021_01 extends BusinessService implements ProcessUnit {

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
		log.info("营业课--库存检索--商品基本信息, response="+bean.getResponse().toString());

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
		String today = DateUtils.date2StringDate(new Date());
		Object[] o = new Object[]{sku,store};
		String sql = sqlpropertyUtil.getValue(store, "hht1021.01.01");
		sql = replaceSqlPram(sql,new Object[]{sku,store});
		List<Map> detailList = jdbcTemplateUtil.searchForList(sql);

		String sqlSom = sqlpropertyUtil.getValue(store, "htt1021.01.04");
		Object[] oSom = new Object[]{store,sku};
		sqlSom = replaceSqlPram(sqlSom,oSom);
		List<Map> storageList = jdbcTemplateSomUtil.searchForList(sqlSom);

		Object[] o2 = new Object[]{sku,store,today};
		String sql2 = sqlpropertyUtil.getValue(store, "hht1021.01.02");
		sql2 = replaceSqlPram(sql2,o2);
//		String sql2 = "select '23' as po_on_the_way";
		List<Map> poOnTheWay = jdbcTemplateUtil.searchForList(sql2);

//		List<Map> poOnTheWay = jdbcTemplateUtil.searchForList(sql2);
		Object[] o3 = new Object[]{sku,store,today};
		String sql3 = sqlpropertyUtil.getValue(store, "hht1021.01.03");
		sql3 = replaceSqlPram(sql3,o3);
		List<Map> trfOnTheWay = jdbcTemplateUtil.searchForList(sql3);
		if(detailList!=null&&detailList.size()>0){
			String mssql = sqlpropertyUtil.getValue(store, "hht1011.01.06");
			List<Map> cprList = null;
			
			cprList = jdbcTemplateSqlServerUtil.searchForList(mssql, new Object[]{ detailList.get(0).get("HHTIPC").toString() });
			/*
			if (store.equals("11108"))
			{
				cprList = jdbcTemplateSqlServerUtil2.searchForList(mssql, new Object[]{ detailList.get(0).get("HHTIPC").toString() });
			}
			else
			{
				cprList = jdbcTemplateSqlServerUtil.searchForList(mssql, new Object[] { detailList.get(0).get("HHTIPC").toString() });
			}
			*/
				
			
			if(cprList!=null&&cprList.size()>0){
				detailList.get(0).put("CPR", cprList.get(0).get("CPR"));
			}
			
			
			if(poOnTheWay!=null && poOnTheWay.size()>0){
				detailList.get(0).put("po_on_the_way", poOnTheWay.get(0).get("PO_ON_THE_WAY").toString());
			}else{
				detailList.get(0).put("po_on_the_way", "0");
			}
			
			
			if(trfOnTheWay!=null && trfOnTheWay.size()>0){
				detailList.get(0).put("trf_on_the_way", trfOnTheWay.get(0).get("TRF_ON_THE_WAY").toString());
//				detailList.addAll(trfOnTheWay);
			}else{
				detailList.get(0).put("trf_on_the_way", "0");
			}
			
			if(storageList!=null&&storageList.size()>0){
				detailList.get(0).put("INV_NUMBER", (((BigDecimal)detailList.get(0).get("INV_NUMBER")).intValue()-((BigDecimal)storageList.get(0).get("SALES_QTY")).intValue()));
				detailList.get(0).put("C_WEEK_SALE", ((BigDecimal)(detailList.get(0).get("C_WEEK_SALE"))).intValue()+((BigDecimal)storageList.get(0).get("SALES_QTY")).intValue());
			}else{
				detailList.get(0).put("C_WEEK_SALE", ((BigDecimal)(detailList.get(0).get("C_WEEK_SALE"))).intValue());
			}
			
			detailList.get(0).put("L_WEEK_SALE",((BigDecimal)(detailList.get(0).get("L_WEEK_SALE"))).intValue());
			Config c = new Config("21", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
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
