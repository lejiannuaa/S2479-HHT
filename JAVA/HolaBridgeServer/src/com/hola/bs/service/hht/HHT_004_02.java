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
import com.hola.bs.util.Root;

/**
 * PO申請收貨
 * @author s1713 modify s2139
 *
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_004_02  extends BusinessService implements ProcessUnit {
	


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
//		log.info("response=" + bean.getResponse().toString());
		MDC.put("userNo", bean.getUser().getName());
		MDC.put("stoNo", bean.getUser().getStore());
		log.info("operation hht004.02PO申請收貨, response="+bean.getResponse().toString());
		return  bean.getResponse().toString();
	}
	
	
	

	private void processData(BusinessBean bean) throws Exception{
			String sto=bean.getUser().getStore();
			String bc = bean.getRequest().getParameter(configpropertyUtil.getValue("bc"));
			while(bc.length()<6){
				
				bc = "0"+bc;
				
			}
			List<Map> list=jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(sto,"hht004.02"),new Object[]{bc});
			
			Root r=new Root();
			if(list.size()<1){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(configpropertyUtil.getValue("msg.01"));
			}else{
				String s=String.valueOf(list.get(0).get("D1STAT"));
				if(s!=null&&"1".equals(s.trim())){
//					String sysId = this.getSystemUtil().getSysid();
					String sysId = bean.getUser() != null?sto + this.systemUtil.getSysid():this.getSystemUtil().getSysid();
					String sql[]=new String[]{ sqlpropertyUtil.getValue(sto,"hht004.02.01")};
					String userName = bean.getUser().getName();
					Object o[]=new Object[1];
					o[0]=new Object[]{sysId, userName, bc};
					String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
					String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
					super.update(bean.getRequest().getRequestID(), sysId, sql, o,sto,guid,requestValue,"" );
				}else{
					bean.getResponse().setCode(BusinessService.errorcode);
					bean.getResponse().setDesc(configpropertyUtil.getValue("msg.03.01")+s+configpropertyUtil.getValue("msg.03.02"));	
				}
				
			}
		
	}
}
