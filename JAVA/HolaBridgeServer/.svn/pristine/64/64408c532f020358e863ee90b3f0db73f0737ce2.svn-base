package com.hola.bs.service.hht;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

/**
 * 删除端架信息，此操作上传至JDA
 * @author S2139
 * 2012 Sep 4, 2012 3:40:52 PM 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_1044_02 extends BusinessService implements ProcessUnit {

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
			throw new RuntimeException();
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getRequest().getParameter("sto"));
		log.info("删除该端架所有的数据信息, response="+bean.getResponse().toString());

		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception{
		// TODO Auto-generated method stub
		String store = bean.getRequest().getParameter(configpropertyUtil.getValue("sto"));
		String groupId = bean.getRequest().getParameter(configpropertyUtil.getValue("groupId"));
		
		String sql = sqlpropertyUtil.getValue(store, "hht1044.01.02");
		Object o = new Object[]{store,groupId};
		jdbcTemplateUtil.update(new String[]{sql}, new Object[]{o});
	}
}