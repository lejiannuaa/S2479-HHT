package com.hola.bs.service.hht;

import java.util.Date;
import java.util.List;
import java.util.Map;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.DateUtils;

/**
 * 价标维护--确认是否新增柜号
 * @author S2139
 * 2012 Aug 29, 2012 4:48:48 PM 
 */
public class HHT_1031_01 extends BusinessService implements ProcessUnit {

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
		return bean.getResponse().toString();

	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
		String date = DateUtils.date2String2(new Date());
		Object[] o = new Object[]{store,loc_no,date};
		String sql = sqlpropertyUtil.getValue(store, "hht1031.01.01");
		List<Map> list = jdbcTemplateUtil.searchForList(sql, o);
		if(list!=null&&list.size()>0){
			if(Integer.parseInt(list.get(0).get("loc_count").toString())>0){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(configpropertyUtil.getValue("msg.1031.01"));
			}else{
				bean.getResponse().setCode(BusinessService.successcode);
			}
		}else{
			bean.getResponse().setCode(BusinessService.successcode);
		}
	}

}
