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
 * 根据商品条码号检索初盘时相应柜号上的初盘结果
 * @author s2139
 *
 */
public class HHT_701_01 extends BusinessService implements ProcessUnit {

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
		MDC.put("stoNo", bean.getRequest().getParameter(configpropertyUtil.getValue("sto")));
		log.info("获取要做抽盘商品的信息, response="+bean.getResponse().toString());

		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean)throws Exception {
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String stk_no = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_no"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
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
		List<Map> skuInfoList = super.getSkuInfo(store, sku);
		if(skuInfoList == null){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(
					configpropertyUtil.getValue("msg.701.01"));
		}else{
			String sql = sqlpropertyUtil.getValue(store, "hht701.01.01");
			Object[] o = new Object[]{store,stk_no,loc_no,sku};
			List<Map> detailList = jdbcTemplateUtil.searchForList(sql, o); 
			if(detailList!=null&&detailList.size()>0){
				Config c = new Config("7", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
						+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
				XmlElement[] xml = new XmlElement[2];
				xml[0] = new XmlElement("head", skuInfoList);
				xml[1] = new XmlElement("detail", detailList);
				writerFile(c, xml, bean);
			}else{
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(new StringBuffer().append(configpropertyUtil.getValue("msg.701.02.01"))
						  .append(loc_no).append(configpropertyUtil.getValue("msg.701.02.02")).toString());
			}
		}
		
	}

}
