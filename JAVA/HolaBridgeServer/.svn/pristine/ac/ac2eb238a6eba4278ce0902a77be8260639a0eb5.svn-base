package com.hola.bs.service.hht;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

/**
 * 解除收货
 * 
 * @author S1608
 * 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_002_05 extends BusinessService implements ProcessUnit {

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
		log.info("operation hht002.05, response="+bean.getResponse().toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		
		String[] sql = new String[]{sqlpropertyUtil
				.getValue(bean.getUser().getStore(), "hht002.05")};
		Object[] o = new Object[]{ bean.getUser().getName(), bean.getRequest().getParameter(
				configpropertyUtil.getValue("bc")) };
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));

		super.updateForValidation(sql, new Object[]{o}, bean.getUser().getStore(), guid, requestValue, "");
		
	}

}
