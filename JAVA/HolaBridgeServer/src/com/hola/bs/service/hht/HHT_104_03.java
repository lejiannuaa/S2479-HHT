package com.hola.bs.service.hht;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.bean.JsonBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.JsonUtil;

/**
 * 店店调拨，提交调拨单
 * @author s1713
 * 
 */

@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_104_03 extends BusinessService implements ProcessUnit {
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
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht104.03店店调拨，提交调拨单, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) {
		
		bean.getResponse().setCode("1");
		bean.getResponse().setDesc("调出门店已禁用此功能，请调入门店JDA开单走采购出货功能发货。");
		
//		
//		String sysId = "";
//		String username = "";
//		String store = "";
//		if (bean.getUser() != null) {
//			sysId = bean.getUser().getStore() + this.systemUtil.getSysid();
//			username = bean.getUser().getName();
//			store = bean.getUser().getStore();
//		}else{
//			try {
//				throw new Exception("当前用户已登出，或系统异常，找不到操作用户");
//			} catch (Exception e) {
//				// TODO Auto-generated catch block
//				e.printStackTrace();
//			}
//		}
//
//		int cnt = 0;
//
//		List<String> l = new ArrayList<String>();
//		l.add(configpropertyUtil.getValue("hht001nodeName.01"));
//		JsonBean json = null;
//		try {
//			json = JsonUtil.jsonToList(String.valueOf(bean.getRequest().getParameter(
//					configpropertyUtil.getValue("xml"))), l);
//		} catch (Exception e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		}
//
//		Long D1SHQT;
//		String sku = "";
//		String reason = "";// 退货原因
//		String numid = null;
//
//		// 如果传入的箱号不为空，则使用传入的箱号，如果传入的箱号为空，则创建新的箱号
//		if (bean.getRequest().getParameter(configpropertyUtil.getValue("bc")) != null
//				&& !bean.getRequest().getParameter(configpropertyUtil.getValue("bc")).toString().equals("")) {
//			numid = bean.getRequest().getParameter(configpropertyUtil.getValue("bc")).toString();
//		} else {
//			numid = this.getSystemUtil().getNumId(store);// 箱号
//		}
//
//		String vendorcode = "";// 厂商代码
//		Map data[] = json.getData().get(configpropertyUtil.getValue("hht001nodeName.01"));
//
//
//		
//		String[] deleteSqls = new String[2];
//		Object[] oo = new Object[2];
//		String deleteSql = sqlpropertyUtil.getValue(store,"hht104.03.04");// 删除明细
//		deleteSqls[0] = deleteSql;
//		oo[0] = new Object[]{numid};
//		deleteSqls[1] =  sqlpropertyUtil.getValue(store,"hht104.03.05");// 删除头
//		oo[1] = new Object[]{numid};
//		super.newUpdate(deleteSqls,oo);
//
//		
//		cnt = 0;
//		String sql[] = new String[data.length + 1];
//		Object o[] = new Object[data.length + 1];
//		for (Map m : data) {
//			D1SHQT = new Long(String.valueOf(m.get(configpropertyUtil.getValue("HHTQTY"))));
//			sku = String.valueOf(m.get(configpropertyUtil.getValue("sku")));
//			reason = String.valueOf(m.get(configpropertyUtil.getValue("reason")));
//			if(reason.equals("null"))
//				reason = "";
//
//			vendorcode = String.valueOf(m.get(configpropertyUtil.getValue("vendorcode")));
//			sql[cnt] = sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht104.03.01");
//
//			o[cnt] = new Object[] { sysId, sku, D1SHQT, reason, numid,
//
//			sysId.substring(sysId.length() - 10, sysId.length())};
//
//			cnt++;
//		}
//
//		sql[cnt] = sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht104.03.02.01");
//		
//		
//		
//		List A5flgList = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht104.05.01"), new Object[] {vendorcode});
//		System.out.println("A5flgList.size()=================================>"+A5flgList.size());
//		if(A5flgList == null || A5flgList.size() == 0){
//				bean.getResponse().setCode("1");
//				bean.getResponse().setDesc("不可往"+vendorcode+"调拨！");
//		}else{
//			o[cnt] = new Object[] { store, 
//					vendorcode, 
//					sysId.substring(sysId.length() - 10, sysId.length()),
//					username,
//					sysId,
//					username,
//					numid
//			};
//			String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
//			String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
//			super.update(bean.getRequest().getRequestID(), sysId, sql, o ,bean.getUser().getStore(),guid,requestValue,numid);
//			
//			//返回箱号
//			bean.getResponse().setDesc(numid);
//		}
	}
}
