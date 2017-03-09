package com.hola.bs.service.hht;

import java.util.List;
import java.util.Map;

import org.apache.log4j.MDC;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import com.hola.bs.bean.BusinessBean;
import com.hola.bs.impl.ProcessUnit;
import com.hola.bs.impl.Request;
import com.hola.bs.service.BusinessService;

/**
 * PO解除收貨
 * @author s1713 modify by s2139
 * 
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_004_03 extends BusinessService implements ProcessUnit {

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
		log.info("operation hht004.03PO解除收貨, response="+bean.getResponse().toString());
		// System.out.println("001_01:response="+response.toString());
		return bean.getResponse().toString();
	}

	private void processData(BusinessBean bean) throws Exception {
		
		String bc = bean.getRequest().getParameter(configpropertyUtil.getValue("bc"));
		while (bc.length() < 6) {
			bc = "0" + bc;
		}
		List<Map> list = jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht004.03"), new Object[] {bc });

		String userName = bean.getUser().getName();
		String s = "";
		String d1trlc = "";
		if (list.size() < 1) {
			bean.getResponse().setCode(BusinessService.errorcode);
			bean.getResponse().setDesc(s + configpropertyUtil.getValue("msg.04"));
		} else {
			s = String.valueOf(list.get(0).get("D1STAT"));
			String d1stat = "";
			Object o_d1trlc = list.get(0).get("D1TRLC");
			if(o_d1trlc==null || ((String)o_d1trlc).length()==0){
				d1stat = "1";
			}else {
				d1stat = "1";
			}
//			d1trlc = String.valueOf(list.get(0).get("D1TRLC")).trim();
			
//			if(d1trlc.equals(""))
//				d1stat = "1";
//			else
//				d1stat = "3";
			String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
			String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
			if (s != null && "3".equals(s.trim())) {
				String sysId = bean.getUser() != null?bean.getUser().getStore() + this.systemUtil.getSysid():this.getSystemUtil().getSysid();
				String sql[] = new String[] { sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht004.03.01") };
				Object o[] = new Object[1];
				o[0] = new Object[] {d1stat, sysId, userName,bc};
//				this.jdbcTemplateUtil.update(sql, o);
				super.update(bean.getRequest().getRequestID(), sysId, sql, o,bean.getUser().getStore(),guid,requestValue,"" );
			} else if (s != null && "4".equals(s.trim())) {
				String sysId = bean.getUser() != null?bean.getUser().getStore() + this.systemUtil.getSysid():this.getSystemUtil().getSysid();
				String sql[] = new String[] { sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht004.03.02") };
				Object o[] = new Object[1];
				o[0] = new Object[] {d1stat, sysId, userName,bc};
//				this.jdbcTemplateUtil.update(sql, o);
				super.update(bean.getRequest().getRequestID(), sysId, sql, o,bean.getUser().getStore(),guid,requestValue,"" );
			} else {
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(
						configpropertyUtil.getValue("msg.03.01") + s + configpropertyUtil.getValue("msg.03.02"));
			}
		}
	}
}
