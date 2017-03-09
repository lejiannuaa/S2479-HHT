package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Config;
import com.hola.bs.util.XmlElement;

/**
 * PO收貨單商品明細顯示
 * @author s1713 modify s2139
 * 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_005_01 extends BusinessService implements ProcessUnit {

	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
			throw new RuntimeException();
		}
		return bean.getResponse().toString();
	}
	

	private void processData(BusinessBean bean) throws Exception {
		String store = bean.getUser()!=null?bean.getUser().getStore():"13101";
		
		String bc = bean.getRequest().getParameter(configpropertyUtil.getValue("bc")).toUpperCase();
		
		while(bc.length()<6){
			
			bc = "0"+bc;
			
		}
		
		List<Map> headList = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht005.01.00"), new Object[] { bc });
		if(headList!=null && headList.size()>0){
			//这里需要加一个判断，是否是当前人在操作
			String pousr = headList.get(0).get("crtuser").toString();
			if(!pousr.equalsIgnoreCase("MW") && !pousr.equals("sys")){
				if(!bean.getUser().getName().equals(pousr)){
					bean.getResponse().setCode(BusinessService.errorcode);
					bean.getResponse().setDesc("该PO单已由用户："+pousr+"在执行收货中");
					return;
				}
			}
//			if(!bean.getUser().getName().equals("DATAHUB") || !bean.getUser().getName().equals(pousr) ){
//				bean.getResponse().setCode(BusinessService.errorcode);
//				bean.getResponse().setDesc("该PO单已由用户："+pousr+"在执行收货中");
//			}
			// Tony于20130911在sql中新增[ifnull(B.D1SHQT,0) as 收货数量] 字段，xml也会增加收货数量，供以后使用
			List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht005.01.01"), new Object[] { bc });
			Config c = new Config("0", "Server->Client：0", String.valueOf(bean.getRequest().getParameter("request"))
					+ String.valueOf(bean.getRequest().getParameter(configpropertyUtil.getValue("op"))));
			
			List<Map> list_ = new ArrayList<Map>();
			
			for(int i=0;i<list.size();i++){
				//原始的对象
				Map om = list.get(i);
				//新的对象
				Map am = skuExist(list_ , om.get("SKU").toString(), "SKU");
				//如果新的对象已经存在，则修改对象，如果不存在，则新增对象
				if(am!=null){
					am.put("UPC", om.get("UPC")+","+am.get("UPC"));
				}else{
					list_.add(om);
				}
			}
			
			
			XmlElement[] xml = new XmlElement[2];
			xml[0] = new XmlElement("info", headList);
			xml[1] = new XmlElement("detail", list_);

			// 当用户进入时获得单据起点，To Location的值，并存入缓存中，（PO专属）
			
			bean.getUser().setAttribute("D1STLC", list.get(0).get("厂商").toString());
			bean.getUser().setAttribute("D1TOLC", list.get(0).get("调入店").toString());
			
			/*
			List<Map> list4 = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"getLocation1"), new Object[]{bc});
			
			// 存入缓存
			if (list4 != null && list4.size() > 0 && list4.get(0) != null) {
				Map map = list4.get(0);
				bean.getUser().setAttribute("D1STLC", map.get("D1STLC").toString());
				bean.getUser().setAttribute("D1TOLC", map.get("D1TOLC").toString());
			}
			*/

			writerFile(c, xml, bean);


		}else{
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("HHT中未查到单号为"+bc+"收货单");
		}
	}
	
}
