package com.hola.bs.service.hht;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.core.UserContainer;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

/**
 * 注销
 * 
 * @author s1713
 * 
 */
public class Logout extends BusinessService implements ProcessUnit {

	private UserContainer uc = null;

	/**
	 * 1.查询用户是否存在 2.查询用户的密码是否一致
	 */
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		try {
			bean = resolveRequest(request);
			// 返回成功代码
			uc.logout(bean.getRequest().getParameter("usr").toString());
			bean.getResponse().setCode(BusinessService.successcode);
			bean.getResponse().setDesc("");
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation 登出系统, response="+bean.getResponse().toString());

//		log.info("response=" + bean.getResponse().toString());
		// 返回错误代码
		return bean.getResponse().toString();

	}

	public UserContainer getUc() {
		return uc;
	}

	public void setUc(UserContainer uc) {
		this.uc = uc;
	}

}
