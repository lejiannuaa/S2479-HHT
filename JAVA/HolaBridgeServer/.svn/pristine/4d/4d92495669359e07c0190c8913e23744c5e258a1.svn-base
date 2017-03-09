package com.hola.bs.service.hht;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

/**
 * 
 * @author s1713
 *
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_005_05  extends BusinessService implements ProcessUnit {
	


	public String process(Request request) {
		BusinessBean bean=new BusinessBean();
		try {
			 bean=resolveRequest(request);
			processData(bean);
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。"+e.getMessage());
			log.error("", e);
			throw new RuntimeException();
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht005.05, response="+bean.getResponse().toString());
		// System.out.println("001_01:response="+response.toString());
		return  bean.getResponse().toString();
	}
	
	
	

	private void processData(BusinessBean bean) throws Exception{
		String sql[]=new String[]{ sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht005.05.01")};
		Object o[]=new Object[1];
		String bc = bean.getRequest().getParameter(configpropertyUtil.getValue("bc"));
		while (bc.length() < 6) {
			bc = "0" + bc;
		}
		o[0]=new Object[]{bean.getUser().getName(), bc};
//		this.jdbcTemplateUtil.update(sql, o);
		String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
		String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
		super.updateForValidation(sql, o, bean.getUser().getStore(), guid, requestValue, "");
	}
}
