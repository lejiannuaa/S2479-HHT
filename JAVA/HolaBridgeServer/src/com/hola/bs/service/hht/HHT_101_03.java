package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

/**
 * 出货到厂商，商品信息检索
 * @author s1713
 * 
 */
public class HHT_101_03 extends BusinessService implements ProcessUnit {

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
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		String store = bean.getUser().getStore();
		String sku = bean.getRequest().getParameter(configpropertyUtil.getValue("bc"));
		
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
			

		String vnd = bean.getRequest().getParameter("vnd");
		List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),
				"hht101.03.02"), new Object[] { sku, sku, sku, store, vnd });
		
		List<Map> list_ = new ArrayList<Map>();
		
		for(int i=0;i<list.size();i++){
			//原始的对象
			Map om = list.get(i);
			//新的对象
			Map am = skuExist(list_ , om.get("HHTSKU").toString(), "HHTSKU");
			//如果新的对象已经存在，则修改对象，如果不存在，则新增对象
			if(am!=null){
				am.put("UPC", om.get("UPC")+","+am.get("UPC"));
			}else{
				list_.add(om);
			}
		}

		Config c = new Config("0", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
				+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
		XmlElement[] xml = new XmlElement[1];
		xml[0] = new XmlElement("info", list_);
		writerFile(c, xml, bean);

		if (list != null && list.size() > 0 && list.get(0) != null) {
			Map map = list.get(0);
			Object hhtdac = map.get("HHTDAC");
			Object hhtvf1 = map.get("HHTVF1");
			Object hhtvf2 = map.get("HHTVF2");
			Object hhtvtp = map.get("HHTVTP");
			Object status = map.get("STATUS");
			if (status != null && !status.equals("D-R")){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc("该SKU不允许退货！");
			}
			
			
			if(hhtvtp!=null && hhtvtp.equals("1")){
				if (hhtdac != null && hhtdac.equals("R")) {
					// xml[0]=new XmlElement("info", list);
					// writerFile(c, xml,bean);
				} else if (hhtvf1 != null && hhtvf1.equals("2")) {
					// xml[0]=new XmlElement("info", list);
					// writerFile(c, xml,bean);
				} else if (hhtvf2 != null && hhtvf2.equals("Y")) {
					// xml[0]=new XmlElement("info", list);
					// writerFile(c, xml,bean);
				}

			}else if(hhtvtp!=null && hhtvtp.equals("3")){
				
			}else{
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc("非法的供应商类型或供应商的类型不符合业务需求，请确认原始数据是否正确。");

			}
		}
	}
	
}
