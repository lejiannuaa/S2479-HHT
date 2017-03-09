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
 * 調撥單整箱收貨
 * @author s1713 modify s2139
 * 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_001_02 extends BusinessService implements ProcessUnit {

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
		log.info("operation hht001.02調撥單整箱收貨, response="+bean.getResponse().toString());

		// System.out.println("001_01:response="+response.toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		
		String store = "";
		String userName = "";
		if (bean.getUser() != null) {
			store = bean.getUser().getStore();
			userName = bean.getUser().getName();
		} else {
			throw new Exception("当前用户已登出，或系统异常，找不到操作用户");
		}
		
		
		String bc = bean.getRequest().getParameter(configpropertyUtil.getValue("bc"));
		
		List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(store,"hht001.02"),
				new Object[] { bc });

		
		if (list.size() < 1) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(configpropertyUtil.getValue("msg.01"));
		} else {
			String s=String.valueOf(list.get(0).get("HHTSTA"));
			String crtusr = list.get(0).get("CRTUSER").toString();
			if(s.trim().equals("2")){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc("该箱状态为"+PullDownUtils.getHHTStatusList().get(s)+"，不可整箱收货");
			}else if(s.trim().equals("1") && !crtusr.equals(userName)){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc("该箱正由用户："+crtusr+"执行收货中");
			}else {
				int sqlSize = 2;// 2+json.getData().get(configpropertyUtil.getValue("hht001nodeName.01")).length;

//				String sysId = "";
				String sysId = bean.getUser() != null?bean.getUser().getStore() + this.systemUtil.getSysid():this.systemUtil.getSysid();
//				if (bean.getUser() != null) {
//					sysId = bean.getUser().getStore() + this.systemUtil.getSysid();
//				} else {
//					sysId = 
//				}
				String sql[] = new String[sqlSize];

				Object o[] = new Object[sqlSize];
				sql[0] = sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht001.02.01");
				o[0] = new Object[] { sysId, bean.getRequest().getParameter(configpropertyUtil.getValue("bc")) };

				sql[sqlSize - 1] = sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht001.02.03");
				
				
				o[sqlSize - 1] = new Object[] { sysId, userName, bc };
				
//				this.jdbcTemplateUtil.update(sql, o);
				String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
				String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
				super.update(bean.getRequest().getRequestID(),sysId,sql,o,store,guid,requestValue,"" );
				//					
			}

		}

	}
}
