package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

/**
 * 做採購調撥時，先檢索可用出貨调拨单
 * @author s1713 modify s2139
 * 
 */
public class HHT_105_04 extends BusinessService implements ProcessUnit {

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
		
		List<Map> jdaCountList = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(), "hht106.02.search")
				, new Object[] { bean.getRequest().getParameter(configpropertyUtil.getValue("bc")) });
		
		if(jdaCountList!=null && jdaCountList.size()>0){
			String hcount = jdaCountList.get(0).get("HCOUNT").toString();
			if(hcount=="" || hcount.equals("0")){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc("查无此单据，请至JDA查询该单状态。如果状态为w,请联系开单人员；如果状态为A，请联系IT。");
				return;
			}
		}else{
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("查无此单据，请至JDA查询该单状态。如果状态为w,请联系开单人员；如果状态为A，请联系IT。");
			return;
		}
		
		
		List<Map> condition = jdbcTemplateUtil
				.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(), "hht106.01.search"),
						new Object[] { bean.getRequest().getParameter(configpropertyUtil.getValue("bc")) });
		if(condition!=null && condition.size()>0){
			Map map = condition.get(0);
			String hhtsts = map.get("HHTSTS").toString();
			
			if (hhtsts.equals("3")) 
			{
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(configpropertyUtil.getValue("msg.106.01"));
				return;
			}
			else if(hhtsts.equals("1"))
			{
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc("请至修改菜单维护");
				return;
			}

			
		}else{
			
		}
	}

}
