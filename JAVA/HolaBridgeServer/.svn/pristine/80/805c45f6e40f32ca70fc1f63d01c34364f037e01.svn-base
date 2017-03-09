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
 * 调拨--采购 修改界面里的提交操作
 * @author s1713
 *
 */
@Transactional(propagation=Propagation.REQUIRED,rollbackFor=Exception.class)
public class HHT_106_02  extends BusinessService implements ProcessUnit {
	


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
		log.info("operation hht106.02, response="+bean.getResponse().toString());
		// System.out.println("001_01:response="+response.toString());
		return  bean.getResponse().toString();
	}
	
	

	private void processData(BusinessBean bean) throws Exception{
			List<Map> list=jdbcTemplateUtil.searchForList(sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht106.02"),new Object[]{bean.getRequest().getParameter(configpropertyUtil.getValue("bc"))});
			
			Root r=new Root();
			if(list.size()<1){
				bean.getResponse().setCode(BusinessService.errorcode);
				bean.getResponse().setDesc(configpropertyUtil.getValue("msg.01"));
		} else {
			String store = bean.getUser()!=null?bean.getUser().getStore():"13101";
			String instno=store+this.getSystemUtil().getSysid();
			String numid = this.getSystemUtil().getNumId(store);
			String hhthno = numid.substring(numid.length() - 10, numid.length());
			String sql[] = new String[3];
			Object o[] = new Object[3];
			String bc = bean.getRequest().getParameter(configpropertyUtil.getValue("bc"));
			sql[0]=sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht106.02.01");
			o[0]=new Object[]{instno,hhthno,bc};
			
			sql[1]=sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht106.02.02");
			o[1]=new Object[]{instno,hhthno,bc};
			
			sql[2]=sqlpropertyUtil.getValue(bean.getUser().getStore(),"hht106.02.03");
			o[2]=new Object[]{bc};
//			this.jdbcTemplateUtil.update(sql, o);
			String guid = bean.getRequest().getParameter(configpropertyUtil.getValue("guid"));
			String requestValue = bean.getRequest().getParameter(configpropertyUtil.getValue("requestValue"));
			super.update(bean.getRequest().getRequestID(), instno, sql, o ,bean.getUser().getStore(),guid,requestValue,"");

		}
		
	}
}
