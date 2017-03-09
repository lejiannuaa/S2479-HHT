package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.property.PullDownUtils;
import com.hola.bs.service.BusinessService;
import com.hola.bs.util.Root;

/**
 * 
 * @author s1713
 * 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_001_06 extends BusinessService implements ProcessUnit {

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
//		log.info("response=" + bean.getResponse().toString());
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht001.06, response="+bean.getResponse().toString());
		// System.out.println("001_01:response="+response.toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil
				.getValue(bean.getUser().getStore(), "hht001.06"), new Object[] { bean.getRequest().getParameter(
				configpropertyUtil.getValue("bc")) });

		Root r = new Root();
		if (list.size() < 1) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.01"));
		} else {
			String s = String.valueOf(list.get(0).get("HHTSTA"));
			if (s == null || !"1".equals(s.trim())) {

				bean.getResponse().setCode(BusinessService.errorcode);
				// response.setDesc(configpropertyUtil.getValue("msg.02"));
				bean.getResponse().setDesc("该箱状态为" + PullDownUtils.getHHTStatusList().get(s) + "，不可解除收货");
			} else {

				String sql[] = new String[] { sqlpropertyUtil.getValue(bean.getUser().getStore(), "hht001.06.01") };
				Object o[] = new Object[1];
				String username = "";
//				String username= bean.getUser()!=null?"s1777":bean.getUser().getName();
				if (bean.getUser() != null) {
					username = bean.getUser().getName();
				} else {
					throw new Exception("当前用户已登出，或系统异常，找不到操作用户");
				}
				o[0] = new Object[] { username, bean.getRequest().getParameter(configpropertyUtil.getValue("bc")) };
				String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
				String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
				super.updateForValidation(sql, o, bean.getUser().getStore(), guid, requestValue, "");
//				this.jdbcTemplateUtil.update(sql, o);
			}

		}
	}
}
