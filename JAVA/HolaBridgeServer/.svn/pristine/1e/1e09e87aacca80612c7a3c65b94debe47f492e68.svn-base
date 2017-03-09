package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

/**
 * 判断是否能新增端架
 * 
 * @author S2139 2012 Aug 30, 2012 3:17:28 PM
 */
public class HHT_1041_01 extends BusinessService implements ProcessUnit {

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

	private void processData(BusinessBean bean) throws Exception {
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(
				configpropertyUtil.getValue("sto"));
		String groupId = bean.getRequest().getParameter(
				configpropertyUtil.getValue("groupId"));

		String sql = sqlpropertyUtil.getValue(store, "hht1041.01.01");
		Object[] o = new Object[] { store, groupId };
		List<Map<String, Object>> list = jdbcTemplateUtil.searchForList(sql, o);
		if (list != null && list.size() > 0) {
			Long lcount_str = (Long) list.get(0).get("group_cnt");
			long count_str = lcount_str.longValue();
			if (count_str == 0l) {
				// bean.getResponse().setCode(BusinessService.successcode);
				// bean.getResponse().setDesc("false");//标识：新增
			} else {
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(
						configpropertyUtil.getValue("msg.1041.01"));
			}
		} else {
			// bean.getResponse().setCode(BusinessService.successcode);
			// bean.getResponse().setDesc("false");//标识：新增
		}
	}

}
