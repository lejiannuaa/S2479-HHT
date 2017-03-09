package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

/**
 * 抽盘柜号验证
 * @author s2139
 * 2012 Aug 28, 2012 1:16:00 PM 
 */
public class HHT_700_02 extends BusinessService implements ProcessUnit {

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

	private void processData(BusinessBean bean)throws Exception {
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String stk_no = bean.getRequest().getParameter(configpropertyUtil.getValue("stk_no"));
		String loc_no = bean.getRequest().getParameter(configpropertyUtil.getValue("loc_no"));
		
		String sql = sqlpropertyUtil.getValue(store, "hht700.01.02");
		Object[] o = new Object[]{stk_no,loc_no};
		List<Map> count_list = jdbcTemplateUtil.searchForList(sql, o);
		String count = count_list.get(0).get("rcd_count").toString();
		if(Integer.parseInt(count) == 0){
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.700.02") +" "+loc_no);
		}
	}

}
