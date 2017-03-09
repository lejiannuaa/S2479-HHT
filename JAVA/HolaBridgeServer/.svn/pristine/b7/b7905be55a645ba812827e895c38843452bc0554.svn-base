package com.hola.bs.service.hht;

import java.util.Date;
import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

/**
 * 
 * @author s1713
 * 
 */
public class Login extends BusinessService implements ProcessUnit {

	/**
	 * 1.查询用户是否存在 2.查询用户的密码是否一致
	 */
	public String process(Request request) {
		BusinessBean bean = new BusinessBean();
		String userIp = request.getParameter("userip");
		try {
			bean = resolveRequest(request);
			processData(bean,userIp);
			// 返回成功代码
		} catch (Exception e) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("系统错误，请联系管理员。" + e.getMessage());
			log.error("", e);
		}
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation 登录系统, response="+bean.getResponse().toString());

//		log.info("response=" + bean.getResponse().toString());
		// 返回错误代码
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean,String userIp) throws Exception {
		// bean.getRequest().getParameter("storeip")
//		System.out.println(bean.getUser().getIp());
		String[] pdaIpFields = userIp.split("\\.");
		String pdaIp = pdaIpFields[2];//获取PDA上IP地址的第三段
		
		List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(null,"sys1000.02"), new Object[] { pdaIp });

		if (list.size() < 1) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc("门店IP服务段尚未设定，请联系管理员");
		} else {
			// 不在本门店内
			boolean isSameStore = true;
			
			String stoNo = String.valueOf(list.get(0).get("STO_NO"));
			// 判断用户ip是否在指定门店内
//			for (Map map : list) {
//				bean.getResponse().setCode(BusinessService.errorcode);
//				bean.getResponse().setDesc("该PDA不属于本店访问，请重新连接");
//				if (bean.getRequest().getParameter("userip").toString().contains(map.get("ip").toString())) {
//					bean.getResponse().setCode(BusinessService.successcode);
//					bean.getResponse().setDesc("");
//					// 用户在门店范围内，可以跳出
//					isSameStore = true;
//					break;
//				}
//			}

			// 如果用户在本店范围内
			if (isSameStore) {

				String sql[] = new String[] { sqlpropertyUtil.getValue(null,"login.02") };
				Object o[] = new Object[] { bean.getRequest().getParameter(configpropertyUtil.getValue("sn")),
						bean.getRequest().getParameter("usr"), com.hola.bs.util.DateUtils.DateFormatToString(new Date(), "yyyyMMdd") };
				// this.jdbcTemplateUtil.update(sql, o);

				list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(null,"login.03"), new Object[] {
						bean.getRequest().getParameter("usr"), bean.getRequest().getParameter("pwd") });
				if (list.size() < 1) {
					bean.getResponse().setCode(BusinessService.errorcode);
					bean.getResponse().setDesc(configpropertyUtil.getValue("msg.login.02"));
				} else {
					String stoStat = String.valueOf(list.get(0).get("user_status"));
					list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(null,"login.04"),
							new Object[] { bean.getRequest().getParameter("usr") });
					if (list.size() < 1) {
						bean.getResponse().setCode(BusinessService.errorcode);
						bean.getResponse().setDesc(configpropertyUtil.getValue("msg.login.02"));
					}else{
						String s = String.valueOf(list.get(0).get("hhtstat"));
						if (s == null || "N".equals(s.trim())) {
							bean.getResponse().setCode(BusinessService.errorcode);
							bean.getResponse().setDesc(configpropertyUtil.getValue("msg.login.03"));
						} else {
							bean.getResponse().setDesc(stoStat);
							bean.getResponse().setStore(stoNo);
							//list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(null,"login.05"),new Object[] { bean.getRequest().getParameter("usr") });

						}
					}
				}
			}
		}
	}
	
//	public static void main(String[] args){
//		String ip = "192.168.2.100";
//		String[] a = ip.split("\\.");
//		System.out.println(a.length);
//	}
}
