package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

/**
 * 盘点初始化请求店铺
 * @author s2139
 * 2012 Aug 28, 2012 5:50:11 PM 
 */
public class HHT_A00_01 extends BusinessService implements ProcessUnit {

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
		String hhtIp = bean.getRequest().getParameter(configpropertyUtil.getValue("hhtIp"));
		hhtIp = hhtIp.substring(0, hhtIp.lastIndexOf("."));
		String sql = sqlpropertyUtil.getValue(null, "hhtA00.01");
		List<Map<String,String>> list = jdbcTemplateUtil.searchForList(sql, new Object[]{hhtIp});
		String sto_no = list.get(0).get("sto_no");
		bean.getResponse().setCode(BusinessService.successcode);
		bean.getResponse().setDesc(sto_no);
	}

}
